using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MCEBuddy.Globals
{
    public enum DRMType
    {
        All = 0,
        Protected = 1,
        Unprotected = 2,
    }

    public enum CommercialRemovalOptions
    {
        None = 0,
        Comskip = 1,
        ShowAnalyzer = 2,
    }

    public enum ShowType
    {
        Default = 0,
        Series = 1,
        Movie = 2,
        Sports = 3,
    }

    [Serializable]
    public struct ConfSettings
    {
        public List<ConversionJobOptions> conversionTasks; // List of Conversion Tasks and conversion options for each task
        public List<MonitorJobOptions> monitorTasks; // List of Monitor Tasks and options for each task
        public GeneralOptions generalOptions; // General settings for MCEBuddy

        public ConfSettings(List<ConversionJobOptions> cjo, List<MonitorJobOptions> mjo, GeneralOptions go)
        {
            conversionTasks = cjo;
            monitorTasks = mjo;
            generalOptions = go;
        }
    }

    [Serializable]
    public class ConversionJobOptions
    {
        [Serializable]
        public class MetadataCorrectionOptions
        {
            public string originalTitle; // Original title to compare
            public string correctedTitle; // Corrected title to replace
            public string tvdbSeriesId; // TVDB Series Id force
            public string imdbSeriesId; // IMDB Series Id force

            public override string ToString()
            {
                string allOpts = "";

                allOpts += "Original Title -> " + originalTitle + "\r\n";
                allOpts += "Corrected Title -> " + correctedTitle + "\r\n";
                allOpts += "TVDB Series Id -> " + tvdbSeriesId + "\r\n";
                allOpts += "IMDB Series Id -> " + imdbSeriesId + "\r\n";

                return allOpts;
            }

            /// <summary>
            /// Clones the current object and returns a new instance of the object
            /// </summary>
            /// <returns>New clone object</returns>
            public MetadataCorrectionOptions Clone()
            {
                MetadataCorrectionOptions clone = (MetadataCorrectionOptions)this.MemberwiseClone();
                return clone;
            }
        };

        public string taskName; // Name of conversion task
        public string profile; // Profile name used for this task

        public string sourceVideo; // Filepath of source video
        public string destinationPath; // Output path
        public string workingPath; // Temp working path
        public bool fallbackToSourcePath; // Fallback to source directory if destination path is unavailable (for network drives)

        public bool skipReprocessing; // Skip conversion if destination file exists
        public bool checkReprocessingHistory; // Check history file for previous conversions
        public bool autoIncrementFilename; // Increment the filenames if the destination file exists

        public bool addToiTunes; // Add to iTunes library
        public bool addToWMP; // Add to WMP Library

        public int maxWidth; // Maximum width
        public double qualityMultiplier; // Quality
        public string FPS; // Frame Rate
        public bool autoDeInterlace; // Automatically detect interlacing and process it
        public bool preferHardwareEncoding; // Automatically detect and enable hardware encoding if available

        public double volumeMultiplier; // Volume
        public bool drc; // Dynamic Range Control
        public bool stereoAudio; // Limit output to 2 channels
        public bool encoderSelectBestAudioTrack; // Let encoder choose the best audio track
        public string audioLanguage; // Audio Language selection
        public double audioOffset; // Audio offset

        public int startTrim; // Trim initial video
        public int endTrim; // Trim end of video

        public string extractCC; // Extract closed captions
        public double ccOffset; // Closed Captions offset
        public bool embedSubtitlesChapters; // Embed the subtitles and chapters into the converted video

        public CommercialRemovalOptions commercialRemoval; // Commercial removal program
        public string comskipIni; // Path to custom Comskip INI file

        public bool downloadSeriesDetails; // Download information from internet
        public bool downloadBanner; // Download the banner file from the internet
        public MetadataCorrectionOptions[] metadataCorrections; // Set of metadata correction options
        public bool prioritizeOriginalBroadcastDateMatch; // Prioritize matching original broadcast date while retrieving metadata
        public ShowType forceShowType; // Force movies or tv series
        public bool writeMetadata; // Write the metadata to the converted file

        public bool renameBySeries; // rename by information
        public bool altRenameBySeries; // alternate renaming
        public string customRenameBySeries; // custom rename file
        public bool renameOnly; // Only rename and move the original file, do not convert or process the video

        public string fileSelection; // File selection filter
        public string metaShowSelection; // Showname Metadata selection filter
        public string metaNetworkSelection; // Network/Channel name Metadata selection filter
        public ShowType metaShowTypeSelection; // Showtype selection
        public DRMType metaDRMSelection; // Filter based on Copy protection type
        public string[] monitorTaskNames; // Filter for related Monitor Task name matching

        public bool insertQueueTop; // Insert new conversions at the beginning/top of the queue
        public bool extractXML; // Create XML file from video properties

        public bool disableCropping; // Disable auto cropping
        public bool commercialSkipCut; // do commercial scan keep EDL file but skip cutting the commercials
        public bool skipCopyBackup; // Don't copy to create a backup of original file - DANGEROUS
        public bool skipRemuxing; // Don't remux the original file to a TS
        public bool ignoreCopyProtection; // Ignore Copy Protection flags on recording while converting

        public string tivoMAKKey; // TiVO MAK key for decrypting and remuxing files and extracting metadata

        public string domainName = ""; // domain name for network credentials
        public string userName = ""; // user name for network credentials
        public string password = ""; // password for network credentials

        public bool enabled; // If the conversion task enabled

        public override string ToString()
        {
            string allOpts = "";

            allOpts += "Task -> " + taskName + "\r\n";
            allOpts += "Profile -> " + profile + "\r\n";
            allOpts += "Source File -> " + sourceVideo + "\r\n";
            allOpts += "Destination Path -> " + destinationPath + "\r\n";
            allOpts += "Working Path -> " + workingPath + "\r\n";
            allOpts += "Fallback Destination -> " + fallbackToSourcePath.ToString() + "\r\n";
            allOpts += "Skip ReProcessing -> " + skipReprocessing.ToString() + "\r\n";
            allOpts += "Check Reprocessing History -> " + checkReprocessingHistory.ToString() + "\r\n";
            allOpts += "Auto Increment Filename -> " + autoIncrementFilename.ToString() + "\r\n";
            allOpts += "Add to iTunes Library -> " + addToiTunes.ToString() + "\r\n";
            allOpts += "Add to WMP Library -> " + addToWMP.ToString() + "\r\n";
            allOpts += "Max Width -> " + maxWidth.ToString() + "\r\n";
            allOpts += "Quality Multipltier -> " + qualityMultiplier.ToString(CultureInfo.InvariantCulture) + "\r\n";
            allOpts += "FPS -> " + FPS + "\r\n";
            allOpts += "Auto DeInterlacing -> " + autoDeInterlace.ToString() + "\r\n";
            allOpts += "Prefer Hardware Encoding -> " + preferHardwareEncoding.ToString() + "\r\n";
            allOpts += "Volume Multipltier -> " + volumeMultiplier.ToString(CultureInfo.InvariantCulture) + "\r\n";
            allOpts += "DRC -> " + drc.ToString() + "\r\n";
            allOpts += "Force Stereo -> " + stereoAudio.ToString() + "\r\n";
            allOpts += "Encoder Select Best Audio Track -> " + encoderSelectBestAudioTrack.ToString() + "\r\n";
            allOpts += "Profile Audio Language -> " + audioLanguage.ToUpper() + "\r\n";
            allOpts += "Audio Offset -> " + audioOffset.ToString(CultureInfo.InvariantCulture) + "\r\n";
            allOpts += "Start Trim -> " + startTrim.ToString() + "\r\n";
            allOpts += "End Trim -> " + endTrim.ToString() + "\r\n";
            allOpts += "Closed Captions -> " + extractCC + "\r\n";
            allOpts += "Closed Captions Offset -> " + ccOffset.ToString(CultureInfo.InvariantCulture) + "\r\n";
            allOpts += "Embed Subtitles and Chapters -> " + embedSubtitlesChapters.ToString() + "\r\n";
            allOpts += "Commercial Removal -> " + commercialRemoval.ToString() + "\r\n";
            allOpts += "Custom Comskip INI Path -> " + comskipIni + "\r\n";
            allOpts += "Download Series Details -> " + downloadSeriesDetails.ToString() + "\r\n";
            if (metadataCorrections != null)
            {
                for (int i = 0; i < metadataCorrections.Length; i++ )
                {
                    allOpts += "Metadata Correction => Option " + i.ToString() + "\r\n";
                    allOpts += metadataCorrections[i].ToString();
                }
            }
            allOpts += "Prioritize matching by Original Broadcast Date -> " + prioritizeOriginalBroadcastDateMatch.ToString() + "\r\n";
            allOpts += "Force Show Type -> " + forceShowType.ToString() + "\r\n";
            allOpts += "Write Metadata -> " + writeMetadata.ToString() + "\r\n";
            allOpts += "Rename by Series -> " + renameBySeries.ToString() + "\r\n";
            allOpts += "Alt Rename by Series -> " + altRenameBySeries.ToString() + "\r\n";
            allOpts += "Custom Rename by Series -> " + customRenameBySeries + "\r\n";
            allOpts += "Rename Only -> " + renameOnly.ToString() + "\r\n";
            allOpts += "File Selection Pattern -> " + fileSelection + "\r\n";
            allOpts += "Show Selection Pattern -> " + metaShowSelection + "\r\n";
            allOpts += "Channel Selection Pattern -> " + metaNetworkSelection + "\r\n";
            allOpts += "Show Type Selection -> " + metaShowTypeSelection.ToString() + "\r\n";
            allOpts += "DRM Type Selection -> " + metaDRMSelection.ToString() + "\r\n";
            allOpts += "Monitor Tasks Selection -> " + (monitorTaskNames == null ? "" : String.Join(",", monitorTaskNames)) + "\r\n";
            allOpts += "Insert at Top of Queue -> " + insertQueueTop.ToString() + "\r\n";
            allOpts += "Extract XML -> " + extractXML.ToString() + "\r\n";
            allOpts += "Disable Cropping -> " + disableCropping.ToString() + "\r\n";
            allOpts += "Task Commercial Skip Cut -> " + commercialSkipCut.ToString() + "\r\n";
            allOpts += "Skip Copying Original File for Backup -> " + skipCopyBackup.ToString() + "\r\n";
            allOpts += "Skip Remuxing Original File to TS -> " + skipRemuxing.ToString() + "\r\n";
            allOpts += "Ignore Copy Protection -> " + ignoreCopyProtection.ToString() + "\r\n";
            allOpts += "TiVO MAK Key -> " + tivoMAKKey + "\r\n";
            allOpts += "Domain Name -> " + domainName + "\r\n";
            allOpts += "User Name -> " + userName + "\r\n";
            allOpts += "Password -> " + new String('*', password.Length) + "\r\n"; // mask the password, preserve the length
            allOpts += "Task Enabled -> " + enabled.ToString() + "\r\n";

            return allOpts;
        }

        /// <summary>
        /// Clones the current object and returns a new instance of the object
        /// </summary>
        /// <returns>New clone object</returns>
        public ConversionJobOptions Clone()
        {
            ConversionJobOptions clone = (ConversionJobOptions) this.MemberwiseClone();
            if (metadataCorrections != null)
            {
                clone.metadataCorrections = new MetadataCorrectionOptions[metadataCorrections.Length];
                for (int i = 0; i < metadataCorrections.Length; i++)
                {
                    clone.metadataCorrections[i] = this.metadataCorrections[i].Clone(); // Clone this object as MemberwiseClone only does a shallow copy
                }
            }

            return clone;
        }
    }

    [Serializable]
    public class MonitorJobOptions
    {
        public string taskName; // Name of monitor task
        public string searchPath; // directory to search for files
        public string searchPattern; // file search pattern
        public bool monitorSubdirectories; // monitor sub directories
        public bool monitorConvertedFiles; // Queue converted files also (by default converted files are ignored)
        public bool reMonitorRecordedFiles; // Ignore the history and remonitor all files

        public string domainName = ""; // domain name for network credentials
        public string userName = ""; // user name for network credentials
        public string password = ""; // password for network credentials

        public override string ToString()
        {
            string allOpts = "";

            allOpts += "Task -> " + taskName + "\r\n";
            allOpts += "Search Path -> " + searchPath + "\r\n";
            allOpts += "Search Pattern -> " + searchPattern + "\r\n";
            allOpts += "Monitor SubDirectories -> " + monitorSubdirectories.ToString() + "\r\n";
            allOpts += "Monitor Converted Files -> " + monitorConvertedFiles.ToString() + "\r\n";
            allOpts += "ReMonitor Recorded Files -> " + reMonitorRecordedFiles.ToString() + "\r\n";
            allOpts += "Domain Name -> " + domainName + "\r\n";
            allOpts += "User Name -> " + userName + "\r\n";
            allOpts += "Password -> " + new String('*', password.Length) + "\r\n"; // mask the password, preserve the length

            return allOpts;
        }

        /// <summary>
        /// Clones the current object and returns a new instance of the object
        /// </summary>
        /// <returns>New clone object</returns>
        public MonitorJobOptions Clone()
        {
            return (MonitorJobOptions)this.MemberwiseClone();
        }
    }

    [Serializable]
    public class EmailBasicSettings
    {
        public string smtpServer; // Name of SMTP server
        public int port; // SMTP port number
        public bool ssl; // Use SSL

        public string userName; // username for SMTP server
        public string password; // password for SMTP server

        public string fromAddress; // From eMail address
        public string toAddresses; // to eMail addresses (multiple separated by ;)
        public string bccAddress; // Bcc eMail addresses (multiple separated by ;)


        public override string ToString()
        {
            string allOpts = "";

            allOpts += "SMTP Server -> " + smtpServer + "\r\n";
            allOpts += "Port -> " + port.ToString() + "\r\n";
            allOpts += "SSL -> " + ssl.ToString() + "\r\n";
            allOpts += "User Name -> " + userName + "\r\n";
            allOpts += "Password -> " + new String('*', password.Length) + "\r\n"; // mask the password, preserve the length
            allOpts += "From -> " + fromAddress + "\r\n";
            allOpts += "To -> " + toAddresses + "\r\n";
            allOpts += "Bcc -> " + bccAddress + "\r\n";

            return allOpts;
        }

        /// <summary>
        /// Clones the current object and returns a new instance of the object
        /// </summary>
        /// <returns>New clone object</returns>
        public EmailBasicSettings Clone()
        {
            return (EmailBasicSettings)this.MemberwiseClone();
        }
    }

    [Serializable]
    public class EMailOptions
    {
        public EmailBasicSettings eMailBasicSettings = new EmailBasicSettings(); // Basic settings for sending an eMail

        public bool successEvent; // send eMail on successful conversion
        public bool failedEvent; // send eMail on failed conversion
        public bool cancelledEvent; // send eMail on cancelled conversion
        public bool startEvent; // send eMail on start of conversion
        public bool downloadFailedEvent; // send eMail if unable to download series information
        public bool queueEvent; // send eMail on adding a file to the queue

        public string successSubject; // Custom subject for successful conversion
        public string failedSubject; // Custom subject for failed conversion
        public string cancelledSubject;  // Custom subject for cancelled conversion
        public string startSubject;  // Custom subject for start of conversion
        public string downloadFailedSubject;  // Custom subject if unable to dowload series information
        public string queueSubject; // Custom subject when adding a file to the queue for conversion
        public bool skipBody; // blank eMail body for notification eMails

        public override string ToString()
        {
            string allOpts = "";

            allOpts += "Send eMail Settings -> " + eMailBasicSettings.ToString() + "\r\n";
            allOpts += "eMail On Success -> " + successEvent.ToString() + "\r\n";
            allOpts += "eMail On Failure -> " + failedEvent.ToString() + "\r\n";
            allOpts += "eMail On Cancellation -> " + cancelledEvent.ToString() + "\r\n";
            allOpts += "eMail On Start -> " + startEvent.ToString() + "\r\n";
            allOpts += "eMail On Download Failure -> " + downloadFailedEvent.ToString() + "\r\n";
            allOpts += "eMail On Queueing -> " + queueEvent.ToString() + "\r\n";
            allOpts += "Custom subject for Successful conversion -> " + successSubject + "\r\n";
            allOpts += "Custom subject for Failed conversion -> " + failedSubject + "\r\n";
            allOpts += "Custom subject for Cancelled conversion -> " + cancelledSubject + "\r\n";
            allOpts += "Custom subject for Start of conversion -> " + startSubject + "\r\n";
            allOpts += "Custom subject for Download Failure -> " + downloadFailedSubject + "\r\n";
            allOpts += "Custom subject for Queueing conversion -> " + queueSubject + "\r\n";
            allOpts += "Skip eMail Body for notifications -> " + skipBody.ToString() + "\r\n";

            return allOpts;
        }

        /// <summary>
        /// Clones the current object and returns a new instance of the object
        /// </summary>
        /// <returns>New clone object</returns>
        public EMailOptions Clone()
        {
            EMailOptions clone = (EMailOptions)this.MemberwiseClone();
            clone.eMailBasicSettings = this.eMailBasicSettings.Clone(); // Clone this object as MemberwiseClone only does a shallow copy
            return clone;
        }
    }

    [Serializable]
    public class GeneralOptions
    {
        public string domainName = ""; // domain name for network credentials
        public string userName = ""; // user name for network credentials
        public string password = ""; // password for network credentials

        public int wakeHour; // Hour to wake system
        public int wakeMinute; // Minute to wake system
        public int startHour; // New conversion start hour
        public int startMinute; // New conversion start minute
        public int stopHour; // New conversion stop hour
        public int stopMinute; // New conversion stop minute
        public string daysOfWeek; // Days of week to start new conversions and wake up system

        public int minimumAge; // Minimum number of days to keep the file before converting
        public int maxConcurrentJobs; // Max no of simutaneous conversions

        public bool logJobs; // Enable job logs
        public int logLevel; // Amount of details of logs
        public int logKeepDays; // number of days to keep the logs

        public bool deleteOriginal; // Delete original file after successful conversion
        public bool useRecycleBin; // Use recycle bin for original recordings
        public bool archiveOriginal; // Archive original file after successful conversion
        public bool deleteConverted; // Delete converted file when source file is deleted

        public bool allowSleep; // Allow system to enter sleep during active conversion
        public bool suspendOnBattery; // Suspend the conversion when the computer switches to battery mode

        public bool sendEmail; // Send emails on various events
        public EMailOptions eMailSettings = new EMailOptions(); // Settings used for sending eMails

        public string locale; // Locale to be used

        public string tempWorkingPath; // Temp folder
        public string archivePath; // Archive folder
        public string failedPath; // Path for moving original files on failure
        public bool spaceCheck; // Check for enough empty space
        public string comskipPath; // Path to custom Comskip (donator version) to use
        public string customProfilePath; // Custom profiles.conf
        
        public int hangTimeout; // Timeout for apps console output before they are determined as hung
        public int pollPeriod; // Polling period for scanning for new files in the monitor tasks
        public string processPriority; // Priority of the applications
        public IntPtr CPUAffinity; // Affinity of CPU set by user
        public bool engineRunning; // Last state of the MCEBuddy engine
        public double subtitleSegmentOffset; // For each commercial segment cut, incremental amount of seconds to offset the subtitles

        public int localServerPort; // Port of the local MCEBuddy server engine, to host the service and enable UPnP
        public bool uPnPEnable; // UPnP support
        public bool firewallExceptionEnabled; // Open a port in the firewall

        public override string ToString()
        {
            string allOpts = "";

            allOpts += "Domain Name -> " + domainName + "\r\n";
            allOpts += "User Name -> " + userName + "\r\n";
            allOpts += "Password -> " + new String('*', password.Length) + "\r\n"; // mask the password, preserve the length
            allOpts += "Wake Hour -> " + wakeHour.ToString() + "\r\n";
            allOpts += "Wake Minute -> " + wakeMinute.ToString() + "\r\n";
            allOpts += "Start Hour -> " + startHour.ToString() + "\r\n";
            allOpts += "Start Minute -> " + startMinute.ToString() + "\r\n";
            allOpts += "Stop Hour -> " + stopHour.ToString() + "\r\n";
            allOpts += "Stop Minute -> " + stopMinute.ToString() + "\r\n";
            allOpts += "Days of Week -> " + daysOfWeek + "\r\n";
            allOpts += "Minimum Age -> " + minimumAge.ToString() + "\r\n";
            allOpts += "Max Concurrent Jobs -> " + maxConcurrentJobs.ToString() + "\r\n";
            allOpts += "Enable Job Logs -> " + logJobs.ToString() + "\r\n";
            allOpts += "Log Level -> " + logLevel.ToString() + "\r\n";
            allOpts += "Log Keep Days -> " + logKeepDays.ToString() + "\r\n";
            allOpts += "Delete Original -> " + deleteOriginal.ToString() + "\r\n";
            allOpts += "Use Recycle Bin -> " + useRecycleBin.ToString() + "\r\n";
            allOpts += "Archive Original -> " + archiveOriginal.ToString() + "\r\n";
            allOpts += "Sync Converted -> " + deleteConverted.ToString() + "\r\n";
            allOpts += "Allow Sleep During Conversions -> " + allowSleep.ToString() + "\r\n";
            allOpts += "Pause Conversion on Battery Power -> " + suspendOnBattery.ToString() + "\r\n";
            allOpts += "Send eMails -> " + sendEmail.ToString() + "\r\n";
            allOpts += "eMail settings -> " + eMailSettings.ToString() + "\r\n";
            allOpts += "Locale -> " + locale + "\r\n";
            allOpts += "Temp Working Path -> " + tempWorkingPath + "\r\n";
            allOpts += "Archive Folder -> " + archivePath + "\r\n";
            allOpts += "Failed Folder -> " + failedPath + "\r\n";
            allOpts += "Space Check -> " + spaceCheck.ToString() + "\r\n";
            allOpts += "Custom Comskip Path -> " + comskipPath + "\r\n";
            allOpts += "Custom profiles.conf -> " + customProfilePath + "\r\n";
            allOpts += "App Hang Timeout -> " + hangTimeout.ToString() + "\r\n";
            allOpts += "Scan New Files Poll Period -> " + pollPeriod.ToString() + "\r\n";
            allOpts += "Process Priority -> " + processPriority + "\r\n";
            allOpts += "CPU Affinity -> " + CPUAffinity.ToString("d") + "\r\n";
            allOpts += "Engine Running -> " + engineRunning.ToString() + "\r\n";
            allOpts += "Subtitle Cut Segment Incremental Offset -> " + subtitleSegmentOffset.ToString(CultureInfo.InvariantCulture) + "\r\n";
            allOpts += "Local Server Port -> " + localServerPort + "\r\n";
            allOpts += "UPnP Enabled -> " + uPnPEnable.ToString() + "\r\n";
            allOpts += "Firewall Exception Enabled -> " + firewallExceptionEnabled.ToString() + "\r\n";

            return allOpts;
        }

        /// <summary>
        /// Clones the current object and returns a new instance of the object
        /// </summary>
        /// <returns>New clone object</returns>
        public GeneralOptions Clone()
        {
            GeneralOptions clone = (GeneralOptions)this.MemberwiseClone();
            clone.eMailSettings = this.eMailSettings.Clone(); // Clone this object as MemberwiseClone only does a shallow copy
            return clone;
        }
    }
}
