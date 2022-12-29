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

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Main form
	/// </summary>
	public partial class PlaneAlerter : Form
	{
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
		public PlaneAlerter() {
			//Stop the annoying cross-thread problems
			//This is probably not a good thing but I'm too lazy
			CheckForIllegalCrossThreadCalls = false;

			//Set culture to invariant
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			//Initialise UI components
			InitializeComponent();

			//Set UI to this form
			Core.Ui = this;

			//When shown, wait until everything has loaded then start threads
			Shown += delegate {
				while (!Settings.SettingsLoaded || !Checker.ConditionsLoaded)
					Thread.Sleep(100);
				if (Settings.StartOnStart) ThreadManager.Start();
				Stats.UpdateStats();
			};
		}

		/// <summary>
		/// Write a message to the console
		/// </summary>
		/// <param name="text">Text to display</param>
		/// <param name="color">Color of text</param>
		public void WriteToConsole(string text, Color color) {
			try {
				console.Select(console.Text.Length, 0);
				console.SelectionBackColor = color;
				console.AppendText(text + Environment.NewLine);
			}
			catch (Exception) {
				
			}
		}
		
		/// <summary>
		/// Updates the condition list
		/// </summary>
		public void UpdateConditionList() {
			conditionTreeView.Nodes[0].Nodes.Clear();
			foreach(var conditionId in Core.Conditions.Keys) {
				var c = Core.Conditions[conditionId];

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
		/// Update the remove twitter account menu options
		/// </summary>
		public void UpdateTwitterAccounts() {
			removeAccountToolStripMenuItem.DropDownItems.Clear();
			removeAccountToolStripMenuItem.Enabled = Settings.TwitterUsers.Count > 0;

			foreach (var screenName in Settings.TwitterUsers.Keys) {
				var item = removeAccountToolStripMenuItem.DropDownItems.Add(screenName);
				item.Click += (sender, args) =>
				{
					if (sender == null)
						return;

					var clickedItem = (ToolStripItem)sender;
					Twitter.RemoveAccount(clickedItem.Text);
				};
			}
		}

		/// <summary>
		/// Updates the status label
		/// </summary>
		/// <param name="text">Text to update label with</param>
		public void UpdateStatusLabel(string text) {
			//Suspend and resume drawing so it doesn't try to draw as it's being changed
			SuspendDrawing(statusStrip1);
			statusLabel.Text = "Status: " + text;
			ResumeDrawing(statusStrip1);
		}

		private void startConditionEditorToolStripMenuItem_Click(object sender, EventArgs e) {
			var editor = new ConditionEditor();
			editor.Show();
			editor.FormClosing += delegate { 
				Checker.LoadConditions();
				UpdateConditionList();
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
			ThreadManager.Restart();
		}
		
		/// <summary>
		/// Form closing
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void PlaneAlerterFormClosing(object sender, FormClosingEventArgs e) {
			//Abort threads if running
			if (Core.StatsThread != null)
				Core.StatsThread.Abort();
			if (Core.LoopThread != null)
				Core.LoopThread.Abort();
			Settings.Save();
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
			var settingsForm = new SettingsForm();
			settingsForm.ShowDialog();
			Settings.UpdateSettings(true);
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void propertyInfoToolStripMenuItem_Click(object sender, EventArgs e) {
			//Show property info form
			var propertyInfoForm = new PropertyInfoForm();
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
			var ecc = new EmailContentConfigForm();
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
			Twitter.AddAccount();
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e) {
			if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Running || ThreadManager.threadStatus == ThreadManager.CheckerStatus.Waiting) {
				ThreadManager.Stop();
			}
			else {
				ThreadManager.Start();
			}
		}
	}
}