using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;

namespace PlaneAlerter {
	static class ThreadManager {
		public static CheckerStatus threadStatus = CheckerStatus.Stopping;

		public static void StartLoopThread(bool restart) {
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

			Core.activeMatches.Clear();
			Core.waitingMatches.Clear();

			if (Core.statsThread == null) {
				Core.statsThread = new Thread(Stats.updateStatsLoop);
				Core.statsThread.Name = "Stats Thread";
				Core.statsThread.Start();
			}
			Thread startThread = new Thread(() => {
				if (Core.loopThread != null)
					while (threadStatus == CheckerStatus.Stopping && Core.loopThread.ThreadState != ThreadState.Stopped) {
						Thread.Sleep(200);
					}
				threadStatus = CheckerStatus.Running;
				Core.loopThread = new Thread(new ThreadStart(Checker.Start));
				Core.loopThread.IsBackground = true;
				Core.loopThread.Name = "Checker Thread";
				Core.loopThread.Start();
				if (restart) {
					Core.UI.writeToConsole("Checker Restarted", Color.White);
					Core.UI.reloadConditionsToolStripMenuItem.Enabled = true;
				}
					
			});
			startThread.Name = "Checker Starter Thread";
			startThread.Start();
		}

		public static void Restart() {
			threadStatus = CheckerStatus.Stopping;
			StartLoopThread(true);
		}

		public enum CheckerStatus {
			Running,
			Stopped,
			Waiting,
			Stopping
		}
	}
}
