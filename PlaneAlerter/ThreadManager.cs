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
		/// <param name="restart">Is this a restart?</param>
		public static void StartOrRestart() {
			bool restart = Core.loopThread != null;

			//Start checker thread
			Thread startThread = new Thread(() => {
				//If thread is not null, wait for it to stop
				if (restart) {
					threadStatus = CheckerStatus.Stopping;
					while (threadStatus == CheckerStatus.Stopping && Core.loopThread.ThreadState != ThreadState.Stopped)
						Thread.Sleep(200);
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
				//Clear matches
				Core.activeMatches.Clear();
				Core.waitingMatches.Clear();

				//Start stats thread if stats thread is null
				if (Core.statsThread == null) {
					Core.statsThread = new Thread(Stats.updateStatsLoop);
					Core.statsThread.Name = "Stats Thread";
					Core.statsThread.Start();
				}
				//Start thread
				threadStatus = CheckerStatus.Running;
				Core.loopThread = new Thread(new ThreadStart(Checker.Start));
				Core.loopThread.IsBackground = true;
				Core.loopThread.Name = "Checker Thread";
				Core.loopThread.Start();
				//If restart, log to UI
				if (restart) {
					Core.UI.writeToConsole("Checker Restarted", Color.White);
					Core.UI.reloadConditionsToolStripMenuItem.Enabled = true;
				}
				else {
					Core.UI.writeToConsole("Checker Started", Color.White);
				}
			});
			startThread.Name = "Checker Starter Thread";
			startThread.Start();
		}

		/// <summary>
		/// Enum for thread status
		/// </summary>
		public enum CheckerStatus {
			Running,
			Stopped,
			Waiting,
			Stopping,
			WaitingForLoad
		}
	}
}
