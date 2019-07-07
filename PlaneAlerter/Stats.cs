using System;
using System.Threading;
using System.Windows.Forms;

namespace PlaneAlerter {
	/// <summary>
	/// Class for statistic operations
	/// </summary>
	static class Stats {
		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		public static int totalAlertsSent = 0;

		/// <summary>
		/// Time planealerter started
		/// </summary>
		public static DateTime timeStarted = DateTime.Now;

		/// <summary>
		/// Number of conditions
		/// </summary>
		public static int numOfConditions {
			get {
				return Core.conditions.Count;
			}
		}

		/// <summary>
		/// Uptime of planealerter
		/// </summary>
		public static string uptime {
			get {
				TimeSpan difference = DateTime.Now.Subtract(timeStarted);
				return difference.Hours + " Hrs " + difference.Minutes + " Mins " + difference.Seconds + " Secs";
			}
		}

		/// <summary>
		/// Method used in stats thread
		/// </summary>
		public static void updateStatsLoop() {
			while (true) {
				updateStats();
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		public static void updateStats() {
			//Cancel if UI or other things are not in a state for displaying stuff
			if (Core.UI.conditionTreeView.IsDisposed || Core.UI.conditionTreeView.Handle == null || ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping)
				return;
			Core.UI.conditionTreeView.Invoke((MethodInvoker)delegate {
				try {
					Core.UI.conditionTreeView.BeginUpdate();
					Core.UI.conditionTreeView.Nodes[2].Nodes[0].Text = "Total Emails Sent: " + totalAlertsSent.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[1].Text = "Total Conditions: " + numOfConditions.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[2].Text = "Time Started: " + timeStarted.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[3].Text = "Uptime: " + uptime.ToString();

					foreach (TreeNode conditionNode in Core.UI.conditionTreeView.Nodes[0].Nodes) {
						int conditionId = Convert.ToInt32(conditionNode.Tag.ToString());
						conditionNode.Nodes[5].Text = "Alerts Sent: " + Core.conditions[conditionId].alertsThisSession;
					}
					Core.UI.conditionTreeView.EndUpdate();

					Core.UI.activeMatchesList.Items.Clear();
					foreach (Core.Match match in Core.activeMatches.Values)
						Core.UI.activeMatchesList.Items.Add(match.Icao + " | " + match.Conditions[0].Condition.conditionName);

					Core.UI.activeMatchesLabel.Text = "Active Matches: " + Core.activeMatches.Count;
				}
				catch (Exception) {

				}
			});
		}
	}
}
