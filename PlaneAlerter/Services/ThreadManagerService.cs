using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using PlaneAlerter.Enums;

namespace PlaneAlerter.Services {
	internal interface IThreadManagerService
	{
		/// <summary>
		/// Thread status of checker thread
		/// </summary>
		CheckerStatus ThreadStatus { get; set; }

		event EventHandler StatusChanged;

		/// <summary>
		/// Start threads
		/// </summary>
		void Start();

		/// <summary>
		/// Stop threads
		/// </summary>
		void Stop();

		/// <summary>
		/// Restart threads
		/// </summary>
		void Restart();
	}

	/// <summary>
	/// Class for managing threads
	/// </summary>
	internal class ThreadManagerService : IThreadManagerService
	{
		public event EventHandler StatusChanged;

		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly ICheckerService _checkerService;
		private readonly ILoggerWithQueue _logger;

		public ThreadManagerService(ISettingsManagerService settingsManagerService, IConditionManagerService conditionManagerService,
			ICheckerService checkerService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_checkerService = checkerService;
			_logger = logger;
		}

		/// <summary>
		/// Thread status of checker thread
		/// </summary>
		private CheckerStatus _threadStatus = CheckerStatus.Stopped;
		public CheckerStatus ThreadStatus
		{
			get => _threadStatus;
			set
			{
				_threadStatus = value;
				StatusChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Thread for checking operations
		/// </summary>
		private Thread? _checkerThread;

		/// <summary>
		/// Start threads
		/// </summary>
		public void Start() {
			if (_checkerThread != null || ThreadStatus == CheckerStatus.Running) {
				Restart();
				return;
			}
			
			//Check for errors
			var error = false;
			if (_conditionManagerService.Conditions.Count == 0) {
				_logger.Log("No conditions, not running checks", Color.White);
				error = true;
			}
			if (_settingsManagerService.Settings.AircraftListUrl == "") {
				_logger.Log("ERROR: No AircraftList.json url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (_settingsManagerService.Settings.RadarUrl == "") {
				_logger.Log("ERROR: No radar url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (!Regex.IsMatch(_settingsManagerService.Settings.AircraftListUrl, @"(http|https):\/\/.+?\/VirtualRadar\/AircraftList\.json.*", RegexOptions.IgnoreCase)) {
				_logger.Log("ERROR: AircraftList.json url invalid. Example: http://127.0.0.1/VirtualRadar/AircraftList.json", Color.White);
				error = true;
			}
			if (error) {
				ThreadStatus = CheckerStatus.Stopped;
				return;
			}
			
			//Start thread
			_checkerThread = new Thread(_checkerService.Start);
			_checkerThread.IsBackground = true;
			_checkerThread.Name = "Checker Thread";
			_checkerThread.Start();
			
			_logger.Log("Checker Started", Color.White);

			ThreadStatus = CheckerStatus.Running;
		}

		/// <summary>
		/// Stop threads
		/// </summary>
		public void Stop()
		{
			if (ThreadStatus != CheckerStatus.Running || _checkerThread == null)
				return;

			_checkerService.Stop();
			ThreadStatus = CheckerStatus.Stopping;
			
			while (_checkerThread.ThreadState != ThreadState.Stopped) Thread.Sleep(200);

			//Clear matches
			_checkerService.ActiveMatches.Clear();

			_checkerThread = null;
			ThreadStatus = CheckerStatus.Stopped;

			_logger.Log("Checker Stopped", Color.White);
		}

		/// <summary>
		/// Restart threads
		/// </summary>
		public void Restart()
		{
			if (ThreadStatus != CheckerStatus.Running)
				return;

			Stop();
			Start();
		}
	}
}
