using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PlaneAlerter {
	static class Stats {
		public static int totalEmailsSent = 0;
		public static DateTime timeStarted = DateTime.Now;
		public static int numOfConditions {
			get {
				return Core.conditions.Count;
			}
		}
		public static string uptime {
			get {
				TimeSpan difference = DateTime.Now.Subtract(timeStarted);
				return difference.Hours + " Hrs " + difference.Minutes + " Mins " + difference.Seconds + " Secs";
			}
		}
		public static void updateStatsLoop() {
			while (true) {
				updateStats();
				Thread.Sleep(1000);
			}
		}

		public static void updateStats() {
			if (Core.UI.conditionTreeView.IsDisposed || Core.UI.conditionTreeView.Handle == null || ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping)
				return;
			Core.UI.conditionTreeView.Invoke((MethodInvoker)delegate {
				try {
					Core.UI.conditionTreeView.Nodes[2].Nodes[0].Text = "Total Emails Sent: " + Stats.totalEmailsSent.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[1].Text = "Total Conditions: " + Stats.numOfConditions.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[2].Text = "Time Started: " + Stats.timeStarted.ToString();
					Core.UI.conditionTreeView.Nodes[2].Nodes[3].Text = "Uptime: " + Stats.uptime.ToString();

					foreach (TreeNode conditionNode in Core.UI.conditionTreeView.Nodes[0].Nodes) {
						int conditionId = Convert.ToInt32(conditionNode.Nodes[0].Text.ToString().Substring(4));
						conditionNode.Nodes[4].Text = "Emails Sent: " + Core.conditions[conditionId].emailsThisSession.ToString();
					}

					Core.UI.activeMatchesList.Items.Clear();
					foreach (Core.Match match in Core.activeMatches.Values)
						Core.UI.activeMatchesList.Items.Add(match.Icao + " | " + match.DisplayName);

					Core.UI.activeMatchesLabel.Text = "Active Matches: " + Core.activeMatches.Count;
				}
				catch (Exception) {

				}
			});
		}
	}
}
