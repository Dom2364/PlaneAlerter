using System;
using System.Windows.Forms;
using PlaneAlerter.Enums;

namespace PlaneAlerter.Services {
	internal interface IStatsService
	{
		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		int TotalAlertsSent { get; set; }

		/// <summary>
		/// Time planealerter started
		/// </summary>
		DateTime TimeStarted { get; set; }

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		void UpdateStats();
	}

	/// <summary>
	/// Class for statistic operations
	/// </summary>
	internal class StatsService : IStatsService
	{
		private readonly IConditionManagerService _conditionManagerService;

		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		public int TotalAlertsSent { get; set; } = 0;

		/// <summary>
		/// Time planealerter started
		/// </summary>
		public DateTime TimeStarted { get; set; } = DateTime.Now;

		public StatsService(IConditionManagerService conditionManagerService)
		{
			_conditionManagerService = conditionManagerService;
		}

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		public void UpdateStats() {
			//Cancel if UI or other things are not in a state for displaying stuff
			if (Core.Ui.conditionTreeView.IsDisposed)
				return;

			Core.Ui.conditionTreeView.Invoke((MethodInvoker)delegate {
				try {
					Core.Ui.conditionTreeView.BeginUpdate();
					Core.Ui.conditionTreeView.Nodes[2].Nodes[0].Text = "Total Emails Sent: " + TotalAlertsSent;
					Core.Ui.conditionTreeView.Nodes[2].Nodes[1].Text = "Total Conditions: " + _conditionManagerService.Conditions.Count;
					Core.Ui.conditionTreeView.Nodes[2].Nodes[2].Text = "Time Started: " + TimeStarted;

					foreach (TreeNode conditionNode in Core.Ui.conditionTreeView.Nodes[0].Nodes) {
						var conditionId = Convert.ToInt32(conditionNode.Tag.ToString());
						conditionNode.Nodes[5].Text = "Alerts Sent: " + _conditionManagerService.Conditions[conditionId].AlertsThisSession;
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
