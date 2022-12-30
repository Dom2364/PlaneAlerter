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

		/// <summary>
		/// Thread for the checker loop
		/// </summary>
		Thread? CheckerThread { get; set; }

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
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly ICheckerService _checkerService;
		private readonly IStatsService _statsService;

		public ThreadManagerService(ISettingsManagerService settingsManagerService, IConditionManagerService conditionManagerService,
			ICheckerService checkerService, IStatsService statsService)
		{
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_checkerService = checkerService;
			_statsService = statsService;
		}

		/// <summary>
		/// Thread status of checker thread
		/// </summary>
		public CheckerStatus ThreadStatus { get; set; } = CheckerStatus.WaitingForLoad;

		/// <summary>
		/// Thread for checking operations
		/// </summary>
		public Thread? CheckerThread { get; set; }

		/// <summary>
		/// Start threads
		/// </summary>
		public void Start() {
			if (CheckerThread != null || ThreadStatus == CheckerStatus.Running) {
				Restart();
				return;
			}
			
			//Check for errors
			var error = false;
			if (_conditionManagerService.Conditions.Count == 0) {
				Core.Ui.WriteToConsole("No conditions, not running checks", Color.White);
				error = true;
			}
			if (_settingsManagerService.Settings.AircraftListUrl == "") {
				Core.Ui.WriteToConsole("ERROR: No AircraftList.json url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (_settingsManagerService.Settings.RadarUrl == "") {
				Core.Ui.WriteToConsole("ERROR: No radar url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (!Regex.IsMatch(_settingsManagerService.Settings.AircraftListUrl, @"(http|https):\/\/.+?\/VirtualRadar\/AircraftList\.json.*", RegexOptions.IgnoreCase)) {
				Core.Ui.WriteToConsole("ERROR: AircraftList.json url invalid. Example: http://127.0.0.1/VirtualRadar/AircraftList.json", Color.White);
				error = true;
			}
			if (error) {
				ThreadStatus = CheckerStatus.Stopped;
				Core.Ui.statusLabel.Text = "Status: Idle";
				return;
			}
			
			//Start thread
			ThreadStatus = CheckerStatus.Running;
			CheckerThread = new Thread(_checkerService.Start);
			CheckerThread.IsBackground = true;
			CheckerThread.Name = "Checker Thread";
			CheckerThread.Start();
			
			Core.Ui.WriteToConsole("Checker Started", Color.White);
			Core.Ui.restartToolStripMenuItem.Enabled = true;
			Core.Ui.startToolStripMenuItem.Text = "Stop";
		}

		/// <summary>
		/// Stop threads
		/// </summary>
		public void Stop()
		{
			if (ThreadStatus != CheckerStatus.Running)
				return;

			_checkerService.Stop();
			Core.Ui.startToolStripMenuItem.Enabled = false;
			Core.Ui.startToolStripMenuItem.Text = "Stopping";
			Core.Ui.restartToolStripMenuItem.Enabled = false;
			while (CheckerThread.ThreadState != ThreadState.Stopped) Thread.Sleep(200);
			CheckerThread = null;
			ThreadStatus = CheckerStatus.Stopped;

			//Clear matches
			Core.ActiveMatches.Clear();

			_statsService.UpdateStats();

			Core.Ui.statusLabel.Text = "Status: Idle";
			Core.Ui.startToolStripMenuItem.Text = "Start";
			Core.Ui.startToolStripMenuItem.Enabled = true;
			Core.Ui.WriteToConsole("Checker Stopped", Color.White);
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
