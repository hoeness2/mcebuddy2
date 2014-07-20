using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;

namespace MCEBuddy.Globals
{
    [ServiceContract]
    public interface ICore
    {
        /// <summary>
        /// Cancel a job currently in the conversion queue (not job queue)
        /// </summary>
        /// <param name="jobList">Job number from the conversion queue</param>
        [OperationContract]
        void CancelJob(int[] jobList);

        /// <summary>
        /// Start the MCEBuddy Monitor CORE engine thread
        /// </summary>
        [OperationContract]
        void Start();

        /// <summary>
        /// Stop the MCEBuddy Monitor CORE engine thread
        /// </summary>
        /// <param name="preserveState"></param>
        [OperationContract]
        void Stop(bool preserveState);

        /// <summary>
        /// Called when the System shuts down MCEBuddy from the windows service manager
        /// It will release all resources and terminate MCEBuddy (uPnP, Firewall etc)
        /// </summary>
        [OperationContract]
        void StopBySystem();

        /// <summary>
        /// Forces MCEBuddy to ReScan the monitor tasks locations for new files to convert
        /// </summary>
        [OperationContract]
        void Rescan();

        /// <summary>
        /// Returns the maximum number of simultaneous job queues
        /// </summary>
        /// <returns>Number of job queues</returns>
        [OperationContract]
        int NumConversionJobs();

        /// <summary>
        /// Gets the JobStatus for a given job in the active conversion tasks
        /// </summary>
        /// <param name="jobNumber">JobQueue Number</param>
        /// <returns>JobStatus</returns>
        [OperationContract]
        JobStatus GetJobStatus(int jobNumber);

        /// <summary>
        /// Gets the JobStatus for all jobs in the queue
        /// </summary>
        /// <returns>List of JobStatus</JobStatus></returns>
        [OperationContract]
        List<JobStatus> GetAllJobsInQueueStatus();

        /// <summary>
        /// Check if the engine is currently actively converting any jobs
        /// </summary>
        /// <returns>True if there are any active running conversions</returns>
        [OperationContract]
        bool Active();

        /// <summary>
        /// Used to indicate whether the service has been shutdown by the system
        /// </summary>
        /// <returns>True is system initiated shutdown</returns>
        [OperationContract]
        bool ServiceShutdownBySystem();

        /// <summary>
        /// Returns a list of all the files in the conversion queue
        /// </summary>
        /// <returns>List of Jobs with Source video fileName and Conversion task name</returns>
        [Obsolete("FileQueue is deprecated, please use GetAllJobsInQueueStatus instead.")]
        [OperationContract]
        List<string[]> FileQueue();

        /// <summary>
        /// Moves a job in the queue to a new location in the queue
        /// </summary>
        /// <param name="currentJobNo">Job no for the job to move</param>
        /// <param name="newJobNo">New location to move to (0 based index)</param>
        [OperationContract]
        bool UpdateFileQueue(int currentJobNo, int newJobNo);

        /// <summary>
        /// Checks if the MCEBuddy CORE engine is running
        /// </summary>
        /// <returns>True is engine is running</returns>
        [OperationContract]
        bool EngineRunning();

        /// <summary>
        /// Check if the cause of the engine stopping is if it has crashed. Should be called if the EngineRunning status is false.
        /// </summary>
        /// <returns>True is the engine stopped due to a crash</returns>
        [OperationContract]
        bool EngineCrashed();

        /// <summary>
        /// Checks if the current time is within the configured Conversion Start time slots
        /// </summary>
        /// <returns>True is it is within the configured conversion start time</returns>
        [OperationContract]
        bool WithinConversionTimes();

        /// <summary>
        /// Pause or Resume the conversion process
        /// </summary>
        /// <param name="suspend">True to pause, false to resume</param>
        [OperationContract]
        void SuspendConversion(bool suspend);

        /// <summary>
        /// Check if the conversions are paused
        /// </summary>
        /// <returns>True if tasks are paused</returns>
        [OperationContract]
        bool IsSuspended();

        /// <summary>
        /// Change the priority of MCEBuddy and child tasks
        /// </summary>
        /// <param name="processPriority">High, Normal or Low</param>
        [OperationContract]
        void ChangePriority(string processPriority);

        /// <summary>
        /// Used to change the state of UPnP Mappings
        /// </summary>
        /// <param name="enable">True to enable UPnP, false to disable it</param>
        [OperationContract]
        void SetUPnPState(bool enable);

        /// <summary>
        /// Create an exception in the firewall for MCEBuddy (port and application)
        /// </summary>
        /// <param name="createException">True to create, false to remove</param>
        [OperationContract]
        void SetFirewallException(bool createException);

        /// <summary>
        /// Updates the MCEBuddyConf global configuration object and writes the settings to MCEBuddy.conf
        /// Calling this assumes that the Engine is in a stopped state
        /// </summary>
        /// <param name="configOptions">MCEBuddyConf parameters object</param>
        /// <returns>False if engine was not stopped when calling this function, true on a successful update</returns>
        [OperationContract]
        bool UpdateConfigParameters(ConfSettings configOptions);

        /// <summary>
        /// Returns a copy of the current MCEBuddyConf global object configuration.
        /// The return object cannot read or write to a config file and is only a static set of configuration parameters.
        /// </summary>
        /// <returns>MCEBuddyConf global object parameters</returns>
        [OperationContract]
        ConfSettings GetConfigParameters();

        /// <summary>
        /// Reloads the latest configuration from the MCEBuddy.conf file and returns a copy of the MCEBuddyConf global object configuration.
        /// The return object cannot read or write to a config file and is only a static set of configuration parameters.
        /// </summary>
        /// <returns>Null if the engine was not stopped else MCEBuddyConf global object parameters</returns>
        [OperationContract]
        ConfSettings? ReloadAndGetConfigParameters();

        /// <summary>
        /// Returns an 2 array string List which contains the Name and Description of all the Profiles in profiles.conf
        /// </summary>
        /// <returns>2 array string List</returns>
        [OperationContract]
        List<string[]> GetProfilesSummary();

        /// <summary>
        /// Adds a manual job to the queue for conversion
        /// </summary>
        /// <param name="videoFilePath">Fully qualified path to video file</param>
        [OperationContract]
        void AddManualJob(string videoFilePath);

        /// <summary>
        /// Gets the Audio, Video and other media info for a file
        /// </summary>
        /// <param name="videoFilePath">Path to the video file</param>
        /// <returns>Media information</returns>
        [OperationContract]
        MediaInfo GetFileInfo(string videoFilePath);

        /// <summary>
        /// Checks if ShowAnalyzer is installed on the machine with the engine
        /// </summary>
        /// <returns>True if ShowAnalyzer is installed</returns>
        [OperationContract]
        bool ShowAnalyzerInstalled();

        /// <summary>
        /// Returns a List to that contains the Windows Event logs entries created by MCEBuddy (recent 500 limit)
        /// </summary>
        [OperationContract]
        List<EventLogEntry> GetWindowsEventLogs();

        /// <summary>
        /// Is MCEBuddy ready to allow the system to go into standby
        /// </summary>
        /// <returns>True if the system can enter standby mode</returns>
        [OperationContract]
        bool AllowSuspend();

        /// <summary>
        /// Delete the History File (put in recycle bin)
        /// </summary>
        [OperationContract]
        void ClearHistory();

        /// <summary>
        /// Get the details of all converted files from the History File
        /// </summary>
        /// <returns>A Dictionary containing a list of all converted files and a Sorted List array containing the history file entries for each file</returns>
        [OperationContract]
        Dictionary<string, SortedList<string, string>> GetConversionHistory();

        /// <summary>
        /// Gets the number of logical processors on the system where the MCEBuddy engine is running
        /// </summary>
        /// <returns>Number of logical processors</returns>
        [OperationContract]
        int GetProcessorCount();

        /// <summary>
        /// Sends an test eMail synchronously
        /// </summary>
        /// <param name="emailSettings">Basic settings for the eMail</param>
        /// <returns>True if sent successfully</returns>
        [OperationContract]
        bool TestEmailSettings(EmailBasicSettings emailSettings);

        /// <summary>
        /// Deletes a file entry from the History file causing MCEBuddy to rescan and possibly reconvert the file
        /// </summary>
        /// <param name="sourceFileName">Filename and path for the file to be reconverted</param>
        [OperationContract]
        void DeleteHistoryItem(string sourceFileName);
    }
}
