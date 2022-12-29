using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;
using PlaneAlerter.Enums;

namespace PlaneAlerter {
	/// <summary>
	/// Class for managing threads
	/// </summary>
	internal class ThreadManager {
		/// <summary>
		/// Thread status of checker thread
		/// </summary>
		public static CheckerStatus ThreadStatus { get; set; } = CheckerStatus.WaitingForLoad;

		/// <summary>
		/// Start threads
		/// </summary>
		public static void Start() {
			if (Core.LoopThread != null || ThreadStatus == CheckerStatus.Running || ThreadStatus == CheckerStatus.Waiting) {
				Restart();
				return;
			}
			
			//Check for errors
			var error = false;
			if (Core.Conditions.Count == 0) {
				Core.Ui.WriteToConsole("No conditions, not running checks", Color.White);
				error = true;
			}
			if (Settings.AircraftListUrl == "") {
				Core.Ui.WriteToConsole("ERROR: No AircraftList.json url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (Settings.RadarUrl == "") {
				Core.Ui.WriteToConsole("ERROR: No radar url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (!Regex.IsMatch(Settings.AircraftListUrl, @"(http|https):\/\/.+?\/VirtualRadar\/AircraftList\.json.*", RegexOptions.IgnoreCase)) {
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
			Core.LoopThread = new Thread(Checker.Start);
			Core.LoopThread.IsBackground = true;
			Core.LoopThread.Name = "Checker Thread";
			Core.LoopThread.Start();
			
			Core.Ui.WriteToConsole("Checker Started", Color.White);
			Core.Ui.restartToolStripMenuItem.Enabled = true;
			Core.Ui.startToolStripMenuItem.Text = "Stop";
		}

		/// <summary>
		/// Stop threads
		/// </summary>
		public static void Stop()
		{
			if (ThreadStatus != CheckerStatus.Running && ThreadStatus != CheckerStatus.Waiting)
				return;

			ThreadStatus = CheckerStatus.Stopping;
			Core.Ui.startToolStripMenuItem.Enabled = false;
			Core.Ui.startToolStripMenuItem.Text = "Stopping";
			Core.Ui.restartToolStripMenuItem.Enabled = false;
			while (Core.LoopThread.ThreadState != ThreadState.Stopped) Thread.Sleep(200);
			Core.LoopThread = null;
			ThreadStatus = CheckerStatus.Stopped;

			//Clear matches
			Core.ActiveMatches.Clear();

			Stats.UpdateStats();

			Core.Ui.statusLabel.Text = "Status: Idle";
			Core.Ui.startToolStripMenuItem.Text = "Start";
			Core.Ui.startToolStripMenuItem.Enabled = true;
			Core.Ui.WriteToConsole("Checker Stopped", Color.White);
		}

		/// <summary>
		/// Restart threads
		/// </summary>
		public static void Restart()
		{
			if (ThreadStatus != CheckerStatus.Running && ThreadStatus != CheckerStatus.Waiting)
				return;

			Stop();
			Start();
		}
	}
}
