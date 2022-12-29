using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;

namespace PlaneAlerter {
	/// <summary>
	/// Class for managing threads
	/// </summary>
	static class ThreadManager {
		/// <summary>
		/// Thread status of checker thread
		/// </summary>
		public static CheckerStatus threadStatus = CheckerStatus.WaitingForLoad;

		/// <summary>
		/// Start threads
		/// </summary>
		public static void Start() {
			if (Core.LoopThread != null || threadStatus == CheckerStatus.Running || threadStatus == CheckerStatus.Waiting) {
				Restart();
				return;
			}
			
			//Check for errors
			bool error = false;
			if (Core.Conditions.Count == 0) {
				Core.Ui.writeToConsole("No conditions, not running checks", Color.White);
				error = true;
			}
			if (Settings.AircraftListUrl == "") {
				Core.Ui.writeToConsole("ERROR: No AircraftList.json url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (Settings.RadarUrl == "") {
				Core.Ui.writeToConsole("ERROR: No radar url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (!Regex.IsMatch(Settings.AircraftListUrl, @"(http|https):\/\/.+?\/VirtualRadar\/AircraftList\.json.*", RegexOptions.IgnoreCase)) {
				Core.Ui.writeToConsole("ERROR: AircraftList.json url invalid. Example: http://127.0.0.1/VirtualRadar/AircraftList.json", Color.White);
				error = true;
			}
			if (error) {
				threadStatus = CheckerStatus.Stopped;
				Core.Ui.statusLabel.Text = "Status: Idle";
				return;
			}
			
			//Start thread
			threadStatus = CheckerStatus.Running;
			Core.LoopThread = new Thread(new ThreadStart(Checker.Start));
			Core.LoopThread.IsBackground = true;
			Core.LoopThread.Name = "Checker Thread";
			Core.LoopThread.Start();
			
			Core.Ui.writeToConsole("Checker Started", Color.White);
			Core.Ui.restartToolStripMenuItem.Enabled = true;
			Core.Ui.startToolStripMenuItem.Text = "Stop";
		}

		/// <summary>
		/// Stop threads
		/// </summary>
		public static void Stop() {
			if (threadStatus == CheckerStatus.Running || threadStatus == CheckerStatus.Waiting) {
				threadStatus = CheckerStatus.Stopping;
				Core.Ui.startToolStripMenuItem.Enabled = false;
				Core.Ui.startToolStripMenuItem.Text = "Stopping";
				Core.Ui.restartToolStripMenuItem.Enabled = false;
				while (Core.LoopThread.ThreadState != ThreadState.Stopped) Thread.Sleep(200);
				Core.LoopThread = null;
				threadStatus = CheckerStatus.Stopped;

				//Clear matches
				Core.ActiveMatches.Clear();

				Stats.updateStats();

				Core.Ui.statusLabel.Text = "Status: Idle";
				Core.Ui.startToolStripMenuItem.Text = "Start";
				Core.Ui.startToolStripMenuItem.Enabled = true;
				Core.Ui.writeToConsole("Checker Stopped", Color.White);
			}
		}

		/// <summary>
		/// Restart threads
		/// </summary>
		public static void Restart() {
			if (threadStatus == CheckerStatus.Running || threadStatus == CheckerStatus.Waiting) {
				Stop();
				Start();
			}
		}

		/// <summary>
		/// Enum for thread status
		/// </summary>
		public enum CheckerStatus {
			WaitingForLoad,
			Running,
			Waiting,
			Stopping,
			Stopped
		}
	}
}
