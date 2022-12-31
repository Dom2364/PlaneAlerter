/*
 * Created by SharpDevelop.
 * User: HP Pavillion
 * Date: 8/09/2015
 * Time: 5:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Main form
	/// </summary>
	internal partial class MainForm : Form
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly ITwitterService _twitterService;
		private readonly IStatsService _statsService;
		private readonly IThreadManagerService _threadManagerService;
		private readonly ICheckerService _checkerService;
		private readonly ILoggerWithQueue _logger;

		/// <summary>
		/// Send message to windows
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

		/// <summary>
		/// Suspend drawing for control
		/// </summary>
		/// <param name="parent">Control to suspend drawing for</param>
		public static void SuspendDrawing(Control parent) {
			SendMessage(parent.Handle, 11, false, 0);
		}

		/// <summary>
		/// Resume drawing for control
		/// </summary>
		/// <param name="parent">Control to resume drawing for</param>
		public static void ResumeDrawing(Control parent) {
			SendMessage(parent.Handle, 11, true, 0);
			parent.Refresh();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public MainForm(ISettingsManagerService settingsManagerService, IConditionManagerService conditionManagerService,
			ITwitterService twitterService, IStatsService statsService, IThreadManagerService threadManagerService,
			ICheckerService checkerService, ILoggerWithQueue logger) {
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_twitterService = twitterService;
			_statsService = statsService;
			_threadManagerService = threadManagerService;
			_checkerService = checkerService;
			_logger = logger;

			//Stop the annoying cross-thread problems
			//This is probably not a good thing but I'm too lazy
			CheckForIllegalCrossThreadCalls = false;

			//Set culture to invariant
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			//Initialise UI components
			InitializeComponent();

			_logger.NewLogMessage += NewLogMessageHandler;
			_twitterService.AccountsUpdated += (sender, e) => UpdateTwitterAccounts();
			_checkerService.StatusChanged += (sender, status) => UpdateStatusLabel(status);
			_threadManagerService.StatusChanged += ThreadManagerServiceOnStatusChanged;
			_checkerService.SendingAlert += CheckerServiceOnSendingAlert;
			_statsService.StatsChanged += (sender, args) => UpdateStats();

			//When shown, wait until everything has loaded then start threads
			Shown += delegate
			{
				_settingsManagerService.Load();
				_conditionManagerService.LoadConditions();

				if (_settingsManagerService.Settings.StartOnStart)
					_threadManagerService.Start();

				UpdateSettings();
				UpdateTwitterAccounts();
				UpdateStats();
				UpdateConditionList();
				conditionTreeView.Nodes[0].Expand();

				//Notify user if no conditions are found
				if (_conditionManagerService.Conditions.Count == 0)
				{
					MessageBox.Show("No Conditions! Click Options then Open Condition Editor to add conditions.",
						"No Conditions!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			};
		}

		private void ThreadManagerServiceOnStatusChanged(object? sender, EventArgs e)
		{
			switch (_threadManagerService.ThreadStatus)
			{
				case CheckerStatus.Stopped:
					startToolStripMenuItem.Text = "Start";
					startToolStripMenuItem.Enabled = true;
					restartToolStripMenuItem.Enabled = false;
					UpdateStatusLabel("Idle");
					break;
				case CheckerStatus.Running:
					startToolStripMenuItem.Text = "Stop";
					startToolStripMenuItem.Enabled = true;
					restartToolStripMenuItem.Enabled = true;
					break;
				case CheckerStatus.Stopping:
					startToolStripMenuItem.Text = "Stopping";
					startToolStripMenuItem.Enabled = false;
					restartToolStripMenuItem.Enabled = false;
					break;
			}
		}

		private void CheckerServiceOnSendingAlert(Condition condition, Aircraft aircraft, string receiver, bool isFirst)
		{
			if (_settingsManagerService.Settings.ShowNotifications)
				notifyIcon.ShowBalloonTip(5000, "Plane Alert!",
					$"Condition: {condition.Name}\nAircraft: {aircraft.GetProperty("Icao")} | {aircraft.GetProperty("Reg")} | {aircraft.GetProperty("Type")} | {aircraft.GetProperty("Call")}",
					ToolTipIcon.Info);
		}

		/// <summary>
		/// Writes new log messages to the console
		/// </summary>
		private void NewLogMessageHandler(object? sender, EventArgs e)
		{
			var messages = _logger.DequeueLogsForUi();
			try {
				foreach (var message in messages)
				{
					console.Select(console.Text.Length, 0);
					if (message.Color.HasValue)
						console.SelectionBackColor = message.Color.Value;
					console.AppendText(message.Message + Environment.NewLine);
				}
			}
			catch (Exception) {
			}
		}

		/// <summary>
		/// Updates the condition list
		/// </summary>
		private void UpdateConditionList() {
			conditionTreeView.Nodes[0].Nodes.Clear();
			foreach(var conditionId in _conditionManagerService.Conditions.Keys) {
				var c = _conditionManagerService.Conditions[conditionId];

				var conditionNode = conditionTreeView.Nodes[0].Nodes.Add("Name: " + c.Name);
				conditionNode.Tag = conditionId;
				conditionNode.Nodes.Add("Id: " + conditionId);
				conditionNode.Nodes.Add("Alert Type: " + c.AlertType.ToString().Replace("_", " "));
				conditionNode.Nodes.Add("Email Enabled: " + c.EmailEnabled);
				conditionNode.Nodes.Add("Twitter Enabled: " + c.TwitterEnabled);
				conditionNode.Nodes.Add("Twitter Account: " + c.TwitterAccount);
				conditionNode.Nodes.Add("Alerts Sent: " + c.AlertsThisSession.ToString());

				var triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach(var trigger in c.Triggers.Values)
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
			}
		}

		/// <summary>
		/// Update settings
		/// </summary>
		private void UpdateSettings()
		{
			_settingsManagerService.Settings.VRSAuthenticate = (_settingsManagerService.Settings.VRSUser != "");
			
			foreach (TreeNode settingsGroupNode in conditionTreeView.Nodes[1].Nodes)
				settingsGroupNode.Nodes.Clear();

			conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + _settingsManagerService.Settings.SenderEmail);
			conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + _settingsManagerService.Settings.SMTPHost + ":" +
			                                              _settingsManagerService.Settings.SMTPPort);
			conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + _settingsManagerService.Settings.SMTPSSL);
			conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + _settingsManagerService.Settings.SMTPUser);

			conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " +
			                                              _settingsManagerService.Settings.AircraftListUrl);
			conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + _settingsManagerService.Settings.RadarUrl);
			if (_settingsManagerService.Settings.VRSAuthenticate)
			{
				conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " +
				                                              _settingsManagerService.Settings.VRSAuthenticate);
				conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + _settingsManagerService.Settings.VRSUser);
			}
			else
				conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " +
				                                              _settingsManagerService.Settings.VRSAuthenticate);
			conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + _settingsManagerService.Settings.RemovalTimeout + " secs");
			conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft further than: " +
			                                              _settingsManagerService.Settings.IgnoreDistance + " km away");
			conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft higher than: " +
			                                              _settingsManagerService.Settings.IgnoreAltitude + " ft");
		}

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		private void UpdateStats()
		{
			//Cancel if UI or other things are not in a state for displaying stuff
			if (conditionTreeView.IsDisposed)
				return;

			conditionTreeView.Invoke((MethodInvoker)delegate
			{
				try
				{
					conditionTreeView.BeginUpdate();
					conditionTreeView.Nodes[2].Nodes[0].Text = "Total Emails Sent: " + _statsService.TotalAlertsSent;
					conditionTreeView.Nodes[2].Nodes[1].Text =
						"Total Conditions: " + _conditionManagerService.Conditions.Count;
					conditionTreeView.Nodes[2].Nodes[2].Text = "Time Started: " + _statsService.TimeStarted;

					foreach (TreeNode conditionNode in conditionTreeView.Nodes[0].Nodes)
					{
						var conditionId = Convert.ToInt32(conditionNode.Tag.ToString());
						conditionNode.Nodes[5].Text = "Alerts Sent: " +
						                              _conditionManagerService.Conditions[conditionId]
							                              .AlertsThisSession;
					}

					conditionTreeView.EndUpdate();

					activeMatchesDataGridView.Rows.Clear();
					foreach (var match in _checkerService.ActiveMatches.Values)
					{
						activeMatchesDataGridView.Rows.Add(match.Icao,
							match.Conditions[0].AircraftInfo.GetProperty("Reg"),
							match.Conditions[0].AircraftInfo.GetProperty("Type"),
							match.Conditions[0].AircraftInfo.GetProperty("Call"), match.Conditions[0].Condition.Name);
					}

					activeAlertsLabel.Text = $"Active Alerts ({_checkerService.ActiveMatches.Count}):";
				}
				catch (Exception)
				{

				}
			});
		}

		/// <summary>
		/// Update the remove twitter account menu options
		/// </summary>
		private void UpdateTwitterAccounts() {
			removeAccountToolStripMenuItem.DropDownItems.Clear();
			removeAccountToolStripMenuItem.Enabled = _settingsManagerService.Settings.TwitterUsers.Count > 0;

			foreach (var screenName in _settingsManagerService.Settings.TwitterUsers.Keys) {
				var item = removeAccountToolStripMenuItem.DropDownItems.Add(screenName);
				item.Click += (sender, args) =>
				{
					if (sender == null)
						return;

					var clickedItem = (ToolStripItem)sender;
					_twitterService.RemoveAccount(clickedItem.Text);
				};
			}
		}

		/// <summary>
		/// Updates the status label
		/// </summary>
		/// <param name="text">Text to update label with</param>
		private void UpdateStatusLabel(string text) {
			//Suspend and resume drawing so it doesn't try to draw as it's being changed
			SuspendDrawing(statusStrip1);
			statusLabel.Text = "Status: " + text;
			ResumeDrawing(statusStrip1);
		}

		private void startConditionEditorToolStripMenuItem_Click(object sender, EventArgs e) {
			var editor = Program.ServiceProvider.GetRequiredService<ConditionListForm>();
			editor.Show();
			editor.FormClosing += delegate {
				_conditionManagerService.LoadConditions();
				_checkerService.ActiveMatches.Clear();
				UpdateConditionList();
				conditionTreeView.Nodes[0].Expand();
			};		
		}

		/// <summary>
		/// Open log file button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e) {
			//Open log file
			Process.Start("alerts.log");
		}
		
		/// <summary>
		/// Restart button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void RestartToolStripMenuItemClick(object sender, EventArgs e) {
			_threadManagerService.Restart();
		}
		
		/// <summary>
		/// Form closing
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void PlaneAlerterFormClosing(object sender, FormClosingEventArgs e) {
			//Abort threads if running
			_threadManagerService.CheckerThread?.Abort();

			_settingsManagerService.Save();
		}
		
		/// <summary>
		/// Link clicked in console
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void ConsoleLinkClicked(object sender, LinkClickedEventArgs e) {
			//Open link
			Process.Start(new ProcessStartInfo(e.LinkText)
			{
				UseShellExecute = true
			});

		}

		/// <summary>
		/// Clear console button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void clearConsoleToolStripMenuItem_Click(object sender, EventArgs e) {
			//Set text to nothing
			console.Text = "";
		}

		/// <summary>
		/// Settings button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			//Show settings form then update settings
			var settingsForm = Program.ServiceProvider.GetRequiredService<SettingsForm>();
			settingsForm.ShowDialog();

			_logger.Log("Reloading Settings...", Color.White);

			UpdateSettings();

			_threadManagerService.Restart();
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void propertyInfoToolStripMenuItem_Click(object sender, EventArgs e) {
			//Show property info form
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show();
		}

		/// <summary>
		/// Tray icon balloon clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e) {
			//Show form
			Activate();
		}

		/// <summary>
		/// Email content config button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void emailContentConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			//Show email content config form
			var ecc = Program.ServiceProvider.GetRequiredService<EmailContentConfigForm>();
			ecc.ShowDialog();
		}

		/// <summary>
		/// Donate button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void donateToolStripMenuItem_Click(object sender, EventArgs e) {
			//Open donate link
			Process.Start(new ProcessStartInfo("https://www.paypal.me/dom2364")
			{
				UseShellExecute = true
			});
		}

		private void addAccountToolStripMenuItem_Click(object sender, EventArgs e) {
			_twitterService.AddAccount();
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e) {
			if (_threadManagerService.ThreadStatus == CheckerStatus.Running) {
				_threadManagerService.Stop();
			}
			else {
				_threadManagerService.Start();
			}
		}
	}
}