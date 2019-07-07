/*
 * Created by SharpDevelop.
 * User: HP Pavillion
 * Date: 8/09/2015
 * Time: 5:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PlaneAlerter
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

			//Initialise UI components
			InitializeComponent();

			//Set UI to this form
			Core.UI = this;

			//When shown, wait until everything has loaded then start threads
			Shown += delegate {
				while (!Settings.SettingsLoaded || !Checker.ConditionsLoaded)
					Thread.Sleep(100);
				ThreadManager.StartOrRestart();
			};
		}

		/// <summary>
		/// Write a message to the console
		/// </summary>
		/// <param name="text">Text to display</param>
		/// <param name="color">Color of text</param>
		public void writeToConsole(string text, Color color) {
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
		public void updateConditionList() {
			conditionTreeView.Nodes[0].Nodes.Clear();
			foreach(int conditionid in Core.conditions.Keys) {
				TreeNode conditionNode;
				Core.Condition c = Core.conditions[conditionid];
				conditionNode = conditionTreeView.Nodes[0].Nodes.Add("Name: " + c.conditionName);
				conditionNode.Tag = conditionid;
				conditionNode.Nodes.Add("Id: " + conditionid);
				conditionNode.Nodes.Add("Alert Type: " + c.alertType.ToString().Replace("_", " "));
				conditionNode.Nodes.Add("Email Enabled: " + c.emailEnabled);
				conditionNode.Nodes.Add("Twitter Enabled: " + c.twitterEnabled);
				conditionNode.Nodes.Add("Twitter Account: " + c.twitterAccount);
				conditionNode.Nodes.Add("Alerts Sent: " + c.alertsThisSession.ToString());
				TreeNode triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach(Core.Trigger trigger in c.triggers.Values)
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
			}
		}

		/// <summary>
		/// Update the remove twitter account menu options
		/// </summary>
		public void updateTwitterAccounts() {
			removeAccountToolStripMenuItem.DropDownItems.Clear();
			removeAccountToolStripMenuItem.Enabled = Settings.TwitterUsers.Count > 0;
			foreach (string screenname in Settings.TwitterUsers.Keys) {
				ToolStripItem item = removeAccountToolStripMenuItem.DropDownItems.Add(screenname);
				item.Click += (object sender, EventArgs args) => {
					ToolStripItem clickeditem = (ToolStripItem)sender;
					Twitter.RemoveAccount(clickeditem.Text);
				};
			}
		}

		/// <summary>
		/// Updates the status label
		/// </summary>
		/// <param name="text">Text to update label with</param>
		public void updateStatusLabel(string text) {
			//Suspend and resume drawing so it doesn't try to draw as it's being changed
			SuspendDrawing(statusStrip1);
			statusLabel.Text = "Status: " + text;
			ResumeDrawing(statusStrip1);
		}

		private void startConditionEditorToolStripMenuItem_Click(object sender, EventArgs e) {
			ConditionEditor editor = new ConditionEditor();
			editor.Show();
			editor.FormClosing += delegate { 
				Checker.LoadConditions();
				updateConditionList();
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
		/// Reload conditions button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void ReloadConditionsToolStripMenuItemClick(object sender, EventArgs e) {
			//Disable button then restart threads
			reloadConditionsToolStripMenuItem.Enabled = false;
			ThreadManager.StartOrRestart();
		}
		
		/// <summary>
		/// Form closing
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void PlaneAlerterFormClosing(object sender, FormClosingEventArgs e) {
			//Abort threads if running
			if (Core.statsThread != null)
				Core.statsThread.Abort();
			if (Core.loopThread != null)
				Core.loopThread.Abort();

			Settings.Save();
		}
		
		/// <summary>
		/// Link clicked in console
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void ConsoleLinkClicked(object sender, LinkClickedEventArgs e) {
			//Open link
			Process.Start(e.LinkText);
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
			SettingsForm settingsForm = new SettingsForm();
			settingsForm.ShowDialog();
			Settings.updateSettings(true);
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void propertyInfoToolStripMenuItem_Click(object sender, EventArgs e) {
			//Show property info form
			PropertyInfoForm propertyInfoForm = new PropertyInfoForm();
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
			Email_Content_Config ecc = new Email_Content_Config();
			ecc.ShowDialog();
		}

		/// <summary>
		/// Donate button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void donateToolStripMenuItem_Click(object sender, EventArgs e) {
			//Open donate link
			Process.Start(@"http://donate.dom2364.com");
		}

		private void addAccountToolStripMenuItem_Click(object sender, EventArgs e) {
			Twitter.AddAccount();
		}
	}
}