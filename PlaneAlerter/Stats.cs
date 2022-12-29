using System;
using System.Windows.Forms;
using PlaneAlerter.Enums;

namespace PlaneAlerter {
	/// <summary>
	/// Class for statistic operations
	/// </summary>
	internal static class Stats {
		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		public static int TotalAlertsSent { get; set; } = 0;

		/// <summary>
		/// Time planealerter started
		/// </summary>
		public static DateTime TimeStarted { get; set; } = DateTime.Now;

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		public static void UpdateStats() {
			//Cancel if UI or other things are not in a state for displaying stuff
			if (Core.Ui.conditionTreeView.IsDisposed || ThreadManager.ThreadStatus == CheckerStatus.Stopping)
				return;

			Core.Ui.conditionTreeView.Invoke((MethodInvoker)delegate {
				try {
					Core.Ui.conditionTreeView.BeginUpdate();
					Core.Ui.conditionTreeView.Nodes[2].Nodes[0].Text = "Total Emails Sent: " + TotalAlertsSent;
					Core.Ui.conditionTreeView.Nodes[2].Nodes[1].Text = "Total Conditions: " + Core.Conditions.Count;
					Core.Ui.conditionTreeView.Nodes[2].Nodes[2].Text = "Time Started: " + TimeStarted;

					foreach (TreeNode conditionNode in Core.Ui.conditionTreeView.Nodes[0].Nodes) {
						var conditionId = Convert.ToInt32(conditionNode.Tag.ToString());
						conditionNode.Nodes[5].Text = "Alerts Sent: " + Core.Conditions[conditionId].AlertsThisSession;
					}
					Core.Ui.conditionTreeView.EndUpdate();
					
					Core.Ui.activeMatchesDataGridView.Rows.Clear();
					foreach (var match in Core.ActiveMatches.Values) {
						Core.Ui.activeMatchesDataGridView.Rows.Add(match.Icao, match.Conditions[0].AircraftInfo.GetProperty("Reg"), match.Conditions[0].AircraftInfo.GetProperty("Type"), match.Conditions[0].AircraftInfo.GetProperty("Call"), match.Conditions[0].Condition.Name);
					}	

					Core.Ui.activeAlertsLabel.Text = $"Active Alerts ({Core.ActiveMatches.Count}):";
				}
				catch (Exception) {

				}
			});
		}
	}
}
