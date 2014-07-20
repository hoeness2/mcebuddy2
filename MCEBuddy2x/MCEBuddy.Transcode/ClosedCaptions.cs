using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MCEBuddy.Util;
using MCEBuddy.Globals;
using MCEBuddy.AppWrapper;

namespace MCEBuddy.Transcode
{
    public class ClosedCaptions
    {
        private string _profile;
        private JobStatus _jobStatus;
        private Log _jobLog;
        private string _extractedSRTFile = "";

        public ClosedCaptions(string profile, JobStatus jobStatus, Log jobLog)
        {
            _profile = profile;
            _jobLog = jobLog;
            _jobStatus = jobStatus;
        }

        public string SRTFile
        { get { return _extractedSRTFile; } }

        public bool Extract(string sourceFile, string workingPath, string ccOptions, int startTrim, int endTrim, double ccOffset)
        {
            _jobLog.WriteEntry(this, Localise.GetPhrase("Extracting Closed Captions as SRT file"), Log.LogEntryType.Information);
            _jobLog.WriteEntry(this, "Source File : " + sourceFile, Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Working Path " + workingPath, Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "CC Options : " + ccOptions, Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Start Trim : " + startTrim.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Stop Trim : " + endTrim.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Offset : " + ccOffset.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Debug);

            if (String.IsNullOrWhiteSpace(ccOptions))
                return true; // nothing to do, accidentally called

            // Output SRT file has to be working directory, will be copied to output afterwards
            string tmpExtractedSRTFile = Path.Combine(workingPath, Path.GetFileNameWithoutExtension(sourceFile)) + ".srt";
            string ccExtractorParams = "";
            string field = "";
            string channel = "";

            if (ccOptions.ToLower() != "default") // let ccextrator choose defaults if specified
            {
                // CCOptions are encoded as field,channel
                field = ccOptions.Split(',')[0];
                channel = ccOptions.Split(',')[1];
            }

            if (field == "1" || field == "2")
                ccExtractorParams += " -" + field; // Field is -1 or -2 (we don't support -12 since it creates 2 SRT files and there's no way to handle that)

            if (channel == "2")
                ccExtractorParams += " -cc2"; // By default is Channel 1, there is no parameter for it

            // Adjust for any offset required during extraction (opposite direction, so -ve)
            if (ccOffset != 0)
                ccExtractorParams += " -delay " + (-ccOffset * 1000).ToString(System.Globalization.CultureInfo.InvariantCulture); // ccOffset is in seconds while -delay required milliseconds

            // Get the length of the video, needed to calculate end point
            float Duration = 0;
            Duration = VideoParams.VideoDuration(sourceFile);
            if (Duration <= 0)
            {
                FFmpegMediaInfo ffmpegStreamInfo = new FFmpegMediaInfo(sourceFile, _jobStatus, _jobLog);
                if (ffmpegStreamInfo.Success && !ffmpegStreamInfo.ParseError)
                {
                    // Converted file should contain only 1 audio stream
                    Duration = ffmpegStreamInfo.MediaInfo.VideoInfo.Duration;
                    _jobLog.WriteEntry(this, Localise.GetPhrase("Video duration") + " : " + Duration.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Information);

                    if (Duration == 0)
                    {
                        _jobLog.WriteEntry(this, Localise.GetPhrase("Video duration 0"), Log.LogEntryType.Error);
                        return false;
                    }
                }
                else
                {
                    _jobLog.WriteEntry(this, Localise.GetPhrase("Cannot read video duration"), Log.LogEntryType.Error);
                    return false;
                }
            }

            // Set the start trim time
            if (startTrim != 0)
                ccExtractorParams += " -startat " + TimeSpan.FromSeconds((double)startTrim).ToString();

            // Set the end trim time
            if (endTrim != 0)
            {
                // startTime = startTrim, stopTime = video_duration - endTrim
                int encDuration = (((int)Duration) - endTrim) - (startTrim); // by default _startTrim is 0

                ccExtractorParams += " -endat " + TimeSpan.FromSeconds((double)encDuration).ToString();
            }

            // Set the input file
            ccExtractorParams += " " + Util.FilePaths.FixSpaces(sourceFile);

            // set output file
            ccExtractorParams += " -o " + Util.FilePaths.FixSpaces(tmpExtractedSRTFile);

            // Run the command
            CCExtractor ccExtractor = new CCExtractor(ccExtractorParams, _jobStatus, _jobLog);
            ccExtractor.Run();

            if (!ccExtractor.Success) // check for termination/success
            {
                _jobLog.WriteEntry("CCExtractor failed. Disabling detection , retrying using TS format", Log.LogEntryType.Warning);

                // TODO: Right format to pick (TS/ES/PS etc) - doing TS for now
                // Sometimes it doesn't detect the format correctly so try to force it (TS)
                ccExtractorParams += " -ts";
                ccExtractor = new CCExtractor(ccExtractorParams, _jobStatus, _jobLog);
                ccExtractor.Run();

                if (!ccExtractor.Success) // check for termination/success
                {
                    _jobLog.WriteEntry("CCExtractor failed to extract closed captions", Log.LogEntryType.Error);
                    return false;
                }
            }

            // Check if we have a format identification error (sometimes ccextractor misidentifies files)
            if ((Util.FileIO.FileSize(tmpExtractedSRTFile) <= 0) && ccExtractor.FormatReadingError)
            {
                FileIO.TryFileDelete(tmpExtractedSRTFile); // Delete the empty file
                _jobLog.WriteEntry(this, "No SRT file found and format error detected. CCExtractor may not have identified the format correctly, forcing TS format and retrying extraction.", Log.LogEntryType.Warning);
                ccExtractorParams += " -in=ts"; // force TS format and retry
                ccExtractor = new CCExtractor(ccExtractorParams, _jobStatus, _jobLog);
                ccExtractor.Run();

                if (!ccExtractor.Success) // check for termination/success
                {
                    _jobLog.WriteEntry(Localise.GetPhrase("Retrying: CCExtractor failed to extract closed captions"), Log.LogEntryType.Error);
                    return false;
                }
            }

            // Check for empty file
            if (Util.FileIO.FileSize(tmpExtractedSRTFile) <= 0)
            {
                FileIO.TryFileDelete(tmpExtractedSRTFile); // Delete the empty file
                _jobLog.WriteEntry(this, Localise.GetPhrase("No valid SRT file found"), Log.LogEntryType.Warning);
                return true; // no error
            }

            _extractedSRTFile = tmpExtractedSRTFile; // We got the file
            _jobLog.WriteEntry(this, Localise.GetPhrase("Extracted closed captions to") + " " + _extractedSRTFile, Log.LogEntryType.Information);

            return true;
        }

        /// <summary>
        /// Adjust the Subtitle file to compensate for the cut segments represented in a EDL file
        /// </summary>
        /// <param name="edlFile">EDL file containing segment information</param>
        /// <param name="srtFile">Subtitle file</param>
        /// <param name="ccOffset">Offset to shift the timebase for all subtitles (+ve or -ve)</param>
        /// <param name="segmentOffset">Incremental offset to shift the timebase for subtitles AFTER each segment is cut (+ve or -ve). Used to compensate for progressive shifting in EDL cutting due to GOP boundary issues</param>
        /// <returns>True if successful</returns>
        public bool EDLTrim(string edlFile, string srtFile, double ccOffset, double segmentOffset)
        {
            _jobLog.WriteEntry(this, Localise.GetPhrase("Syncing SRT file with EDL file"), Log.LogEntryType.Information);
            _jobLog.WriteEntry(this, "EDL File " + edlFile, Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "SRT File " + srtFile, Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Offset : " + ccOffset.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Debug);
            _jobLog.WriteEntry(this, "Progressive Segment Cut Correction : " + segmentOffset.ToString(System.Globalization.CultureInfo.InvariantCulture), Log.LogEntryType.Debug);

            if (String.IsNullOrEmpty(srtFile) || String.IsNullOrEmpty(edlFile))
                return true; // nothing to do

            if (!File.Exists(srtFile) || !File.Exists(edlFile) || FileIO.FileSize(srtFile) == 0 || FileIO.FileSize(edlFile) == 0)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("SRT/EDL file does not exist or is 0 bytes in size"), Log.LogEntryType.Warning);
                return true; // nothing to do
            }

            // Taken from MACHYY1's srt_edl_cutter.ps1 (translated and enhanced from PS1 to C#)
            List<List<double>> edl_array = new List<List<double>>();
            List<List<string>> srt_array = new List<List<string>>();

            // Read the EDL File
            try
            {
                System.IO.StreamReader edlS = new System.IO.StreamReader(edlFile);
                string line;
                double edl_start_keep = 0, edl_end_keep = 0, cut_start_time = 0, cut_end_time = 0, edl_offset = ccOffset;
                string[] fields;
                List<double> edl_line;

                while ((line = edlS.ReadLine()) != null)
                {
                    fields = line.Split('\t');
                    int actionType = Int32.Parse(fields[2]);
                    if (actionType == 0)
                    {
                        cut_start_time = double.Parse(fields[0], System.Globalization.CultureInfo.InvariantCulture); // throw an expcetion if it's an invalid EDL
                        cut_end_time = double.Parse(fields[1], System.Globalization.CultureInfo.InvariantCulture); // throw an expcetion if it's an invalid EDL

                        if (cut_start_time == 0)
                        {
                            edl_start_keep = cut_end_time;
                        }
                        else if (cut_start_time > edl_start_keep)
                        {
                            edl_offset += edl_start_keep - edl_end_keep;
                            edl_end_keep = cut_start_time;

                            edl_line = new List<double>();
                            edl_line.Add(edl_start_keep); // Start of keep video/srt
                            edl_line.Add(edl_end_keep); // End of keep video/srt
                            edl_line.Add(edl_offset); // How much was cut inbetween
                            edl_array.Add(edl_line);

                            edl_start_keep = cut_end_time;
                        }
                    }
                }

                edl_offset += edl_start_keep - edl_end_keep;

                edl_line = new List<double>();
                edl_line.Add(edl_start_keep);
                edl_line.Add((double)9999999999); // till end of file
                edl_line.Add(edl_offset);
                edl_array.Add(edl_line);

                edlS.Close();
                edlS.Dispose();
            }
            catch (Exception e)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("Error processing EDL file") + " " + e.ToString(), Log.LogEntryType.Error);
                return false;
            }

            // Read the SRT File
            try
            {
                System.IO.StreamReader srtS = new System.IO.StreamReader(srtFile);
                string line;
                int edl_array_position = 0;
                int sequence = 1, temp;
                List<string> line_text = new List<string>();
                double srt_start_time = 0, srt_end_time = 0;
                List<string> srt_line;
                string[] fields;

                while ((line = srtS.ReadLine()) != null)
                {
                    if (line == "") //blank line - so write the info to an array (end of current entry)
                    {
                        if (sequence > 0)
                        {
                            if (srt_start_time > edl_array[edl_array_position][1]) // in the EDL cut area
                                edl_array_position++;
                            else if (srt_start_time >= edl_array[edl_array_position][0] && srt_start_time <= edl_array[edl_array_position][1]) // record to keep
                            {
                                srt_line = new List<string>(); // Build the SRT entry
                                string startTimeCode = (seconds_to_hhmmss(srt_start_time - edl_array[edl_array_position][2] + (edl_array_position * segmentOffset))); // Compensate progressively for each segment
                                if (srt_end_time > edl_array[edl_array_position][1])  // SANITY CHECK: end time should not exceed the end of keep video/srt time (otherwise it looks like a burned in video with previous subtitle overlapping the next at the cut boundary)
                                    srt_end_time = edl_array[edl_array_position][1];
                                string endTimeCode = (seconds_to_hhmmss(srt_end_time - edl_array[edl_array_position][2] + (edl_array_position * segmentOffset))); // Compensate progressively for each segment.

                                /* SRT FORMAT:
                                 * SubRip (SubRip Text) files are named with the extension .srt, and contain formatted plain text.
                                 * The time format used is hours:minutes:seconds,milliseconds. The decimal separator used is the comma, since the program was written in France.
                                 * The line break used is often the CR+LF pair.
                                 * Subtitles are numbered sequentially, starting at 1.
                                    Subtitle number
                                    Start time --> End time
                                    Text of subtitle (one or more lines)
                                    Blank line
                                 */
                                srt_line.Add(sequence.ToString()); // Subtitle No
                                srt_line.Add(startTimeCode + " --> " + endTimeCode); // Start time --> End time
                                foreach (string text in line_text) // Text of subtitle (one or more lines)
                                    srt_line.Add(text);
                                srt_line.Add(""); // Blank line

                                srt_array.Add(srt_line); // Build the SRT file
                                sequence++;
                            }

                            srt_start_time = srt_end_time = 0;
                            line_text.Clear(); // Reset the content
                        }
                    }
                    else if (int.TryParse(line, out temp)) // Line number
                        continue; // do nothing here
                    else if (line.Contains(" --> ")) // Timecodes
                    {
                        line = line.Replace(" --> ", "\t");
                        fields = line.Split('\t');
                        srt_start_time = hhmmss_to_seconds(fields[0]);
                        srt_end_time = hhmmss_to_seconds(fields[1]);
                    }
                    else if (line != "") // Text Content
                    {
                        if (!String.IsNullOrWhiteSpace(line)) // don't add blank lines, it causes programs like MP4Box to choke
                            line_text.Add(line);
                    }
                    else
                        throw new System.ArgumentException("Invalid SRT file format");
                }

                srtS.Close();
                srtS.Dispose();
            }
            catch (Exception e)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("Error processing SRT file") + " " + e.ToString(), Log.LogEntryType.Error);
                return false;
            }

            // Delete the original SRT file
            FileIO.TryFileDelete(srtFile);

            try
            {
                // Write the new SRT file
                StreamWriter srtWrite = new System.IO.StreamWriter(srtFile);
                foreach (List<string> srt_line in srt_array)
                    foreach (string entry in srt_line)
                        srtWrite.WriteLine(entry); // Format is already prebuilt above, just dump into the file

                srtWrite.Flush();
                srtWrite.Close();
                srtWrite.Dispose();
            }
            catch (Exception e)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("Error writing to SRT file") + " " + e.ToString(), Log.LogEntryType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate the SRT format and clean up if possible to avoid issues with other programs
        /// </summary>
        /// <param name="srtFile">SRT file to check and clean</param>
        /// <returns>True if successful</returns>
        public bool SRTValidateAndClean(string srtFile)
        {
            List<List<string>> srt_array = new List<List<string>>();

            _jobLog.WriteEntry(this, Localise.GetPhrase("Validating and cleaning SRT file"), Log.LogEntryType.Information);
            _jobLog.WriteEntry(this, "SRT File " + srtFile, Log.LogEntryType.Debug);

            if (String.IsNullOrEmpty(srtFile))
                return true; // nothing to do

            if (!File.Exists(srtFile) || FileIO.FileSize(srtFile) == 0)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("SRT file does not exist or is 0 bytes in size"), Log.LogEntryType.Warning);
                return true; // nothing to do
            }

            // Read the SRT File
            try
            {
                System.IO.StreamReader srtS = new System.IO.StreamReader(srtFile);
                string line;
                int sequence = 1, temp;
                List<string> line_text = new List<string>();
                double srt_start_time = 0, srt_end_time = 0;
                List<string> srt_line;
                string[] fields;

                while ((line = srtS.ReadLine()) != null)
                {
                    if (line == "") //blank line - so write the info to an array (end of current entry)
                    {
                        if (sequence > 0)
                        {
                            srt_line = new List<string>(); // Build the SRT entry
                            string startTimeCode = (seconds_to_hhmmss(srt_start_time));
                            string endTimeCode = (seconds_to_hhmmss(srt_end_time));

                            /* SRT FORMAT:
                                * SubRip (SubRip Text) files are named with the extension .srt, and contain formatted plain text.
                                * The time format used is hours:minutes:seconds,milliseconds. The decimal separator used is the comma, since the program was written in France.
                                * The line break used is often the CR+LF pair.
                                * Subtitles are numbered sequentially, starting at 1.
                                Subtitle number
                                Start time --> End time
                                Text of subtitle (one or more lines)
                                Blank line
                                */
                            srt_line.Add(sequence.ToString()); // Subtitle No
                            srt_line.Add(startTimeCode + " --> " + endTimeCode); // Start time --> End time
                            foreach (string text in line_text) // Text of subtitle (one or more lines)
                                srt_line.Add(text);
                            srt_line.Add(""); // Blank line

                            srt_array.Add(srt_line); // Build the SRT file
                            sequence++;

                            srt_start_time = srt_end_time = 0;
                            line_text.Clear(); // Reset the content
                        }
                    }
                    else if (int.TryParse(line, out temp)) // Line number
                        continue; // do nothing here
                    else if (line.Contains(" --> ")) // Timecodes
                    {
                        line = line.Replace(" --> ", "\t");
                        fields = line.Split('\t');
                        srt_start_time = hhmmss_to_seconds(fields[0]);
                        srt_end_time = hhmmss_to_seconds(fields[1]);
                    }
                    else if (line != "") // Text Content
                    {
                        if (!String.IsNullOrWhiteSpace(line)) // don't add blank lines, it causes programs like MP4Box to choke
                            line_text.Add(line);
                    }
                    else
                        throw new System.ArgumentException("Invalid SRT file format");
                }

                srtS.Close();
                srtS.Dispose();
            }
            catch (Exception e)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("Error validating SRT file") + " " + e.ToString(), Log.LogEntryType.Error);
                return false;
            }

            // Delete the original SRT file
            FileIO.TryFileDelete(srtFile);

            try
            {
                // Write the new SRT file
                StreamWriter srtWrite = new System.IO.StreamWriter(srtFile);
                foreach (List<string> srt_line in srt_array)
                    foreach (string entry in srt_line)
                        srtWrite.WriteLine(entry); // Format is already prebuilt above, just dump into the file

                srtWrite.Flush();
                srtWrite.Close();
                srtWrite.Dispose();
            }
            catch (Exception e)
            {
                _jobLog.WriteEntry(this, Localise.GetPhrase("Error writing to clean SRT file") + " " + e.ToString(), Log.LogEntryType.Error);
                return false;
            }

            return true;
        }

        private double hhmmss_to_seconds(string hhmmss)
        {
            double hour = 3600 * double.Parse(hhmmss.Substring(0, 2), System.Globalization.CultureInfo.InvariantCulture); // throw an expcetion if it's an invalid
            double minute = 60 * double.Parse(hhmmss.Substring(3, 2), System.Globalization.CultureInfo.InvariantCulture); // throw an expcetion if it's an invalid
            double second = double.Parse(hhmmss.Substring(6, 2), System.Globalization.CultureInfo.InvariantCulture); // throw an expcetion if it's an invalid
            double millisecond = double.Parse(hhmmss.Substring(9, 3), System.Globalization.CultureInfo.InvariantCulture) / 1000; // throw an expcetion if it's an invalid

            double secondsTime = hour + minute + second + millisecond;

            return secondsTime;
        }

        private string seconds_to_hhmmss(double seconds)
        {
            TimeSpan st = TimeSpan.FromSeconds(seconds);
            if (seconds < 0)
                return st.ToString(@"\-hh\:mm\:ss\,fff", System.Globalization.CultureInfo.InvariantCulture); //-00:25:30,978
            else
                return st.ToString(@"hh\:mm\:ss\,fff", System.Globalization.CultureInfo.InvariantCulture); //00:25:30,978
        }
    }
}
