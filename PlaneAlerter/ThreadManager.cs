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
			if (Core.loopThread != null || threadStatus == CheckerStatus.Running || threadStatus == CheckerStatus.Waiting) {
				Restart();
				return;
			}
			
			//Check for errors
			bool error = false;
			if (Core.conditions.Count == 0) {
				Core.UI.writeToConsole("No conditions, not running checks", Color.White);
				error = true;
			}
			if (Settings.acListUrl == "") {
				Core.UI.writeToConsole("ERROR: No AircraftList.json url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (Settings.radarUrl == "") {
				Core.UI.writeToConsole("ERROR: No radar url specified, go to Options>Settings", Color.White);
				error = true;
			}
			if (!Regex.IsMatch(Settings.acListUrl, @"(http|https):\/\/.+?\/VirtualRadar\/AircraftList\.json.*", RegexOptions.IgnoreCase)) {
				Core.UI.writeToConsole("ERROR: AircraftList.json url invalid. Example: http://127.0.0.1/VirtualRadar/AircraftList.json", Color.White);
				error = true;
			}
			if (error) {
				threadStatus = CheckerStatus.Stopped;
				Core.UI.statusLabel.Text = "Status: Idle";
				return;
			}
			
			//Start thread
			threadStatus = CheckerStatus.Running;
			Core.loopThread = new Thread(new ThreadStart(Checker.Start));
			Core.loopThread.IsBackground = true;
			Core.loopThread.Name = "Checker Thread";
			Core.loopThread.Start();
			
			Core.UI.writeToConsole("Checker Started", Color.White);
			Core.UI.restartToolStripMenuItem.Enabled = true;
			Core.UI.startToolStripMenuItem.Text = "Stop";
		}

		/// <summary>
		/// Stop threads
		/// </summary>
		public static void Stop() {
			if (threadStatus == CheckerStatus.Running || threadStatus == CheckerStatus.Waiting) {
				threadStatus = CheckerStatus.Stopping;
				Core.UI.startToolStripMenuItem.Enabled = false;
				Core.UI.startToolStripMenuItem.Text = "Stopping";
				Core.UI.restartToolStripMenuItem.Enabled = false;
				while (Core.loopThread.ThreadState != ThreadState.Stopped) Thread.Sleep(200);
				Core.loopThread = null;
				threadStatus = CheckerStatus.Stopped;

				//Clear matches
				Core.activeMatches.Clear();

				Stats.updateStats();

				Core.UI.statusLabel.Text = "Status: Idle";
				Core.UI.startToolStripMenuItem.Text = "Start";
				Core.UI.startToolStripMenuItem.Enabled = true;
				Core.UI.writeToConsole("Checker Stopped", Color.White);
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
