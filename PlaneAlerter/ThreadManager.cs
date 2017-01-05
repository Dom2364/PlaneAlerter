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
		public static CheckerStatus threadStatus = CheckerStatus.Stopping;

		/// <summary>
		/// Start threads
		/// </summary>
		/// <param name="restart">Is this a restart?</param>
		public static void StartLoopThread(bool restart) {
			//Check for errors
			bool error = false;
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
			if (error) return;

			//Clear matches
			Core.activeMatches.Clear();
			Core.waitingMatches.Clear();

			//Start stats thread if stats thread is null
			if (Core.statsThread == null) {
				Core.statsThread = new Thread(Stats.updateStatsLoop);
				Core.statsThread.Name = "Stats Thread";
				Core.statsThread.Start();
			}

			//Start checker thread
			Thread startThread = new Thread(() => {
				//If thread is not null, wait for it to stop
				if (Core.loopThread != null)
					while (threadStatus == CheckerStatus.Stopping && Core.loopThread.ThreadState != ThreadState.Stopped)
						Thread.Sleep(200);
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
			});
			startThread.Name = "Checker Starter Thread";
			startThread.Start();
		}

		/// <summary>
		/// Restart threads
		/// </summary>
		public static void Restart() {
			threadStatus = CheckerStatus.Stopping;
			StartLoopThread(true);
		}

		/// <summary>
		/// Enum for thread status
		/// </summary>
		public enum CheckerStatus {
			Running,
			Stopped,
			Waiting,
			Stopping
		}
	}
}
