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
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

//CHANGELOG
//Updated assembly information
//Fixed status label text changing during draw causing error

namespace PlaneAlerter
{
	public partial class PlaneAlerter : Form
	{
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

		public static void SuspendDrawing(Control parent) {
			SendMessage(parent.Handle, 11, false, 0);
		}

		public static void ResumeDrawing(Control parent) {
			SendMessage(parent.Handle, 11, true, 0);
			parent.Refresh();
		}

		public PlaneAlerter()
		{
			CheckForIllegalCrossThreadCalls = false;

			InitializeComponent();

			Core.UI = this;

			Shown += delegate {
				while (!Settings.SettingsLoaded || !Checker.ConditionsLoaded) {
					Thread.Sleep(100);
				}
				ThreadManager.StartLoopThread(false);
				writeToConsole("PlaneAlerter Started!", Color.White);
				writeToConsole("", Color.White);
			};
		}

		public void writeToConsole(string text, Color color) {
			try {
				console.Select(console.Text.Length, 0);
				console.SelectionBackColor = color;
				console.AppendText(text + Environment.NewLine);
			}
			catch (Exception) {
				
			}
		}
		
		public void updateConditionList() {
			conditionTreeView.Nodes[0].Nodes.Clear();
			foreach(int conditionid in Core.conditions.Keys) {
				TreeNode conditionNode;
				Core.Condition c = Core.conditions[conditionid];
				conditionNode = conditionTreeView.Nodes[0].Nodes.Add("Name: " + c.conditionName); 
				conditionNode.Nodes.Add("Id: " + conditionid);
				conditionNode.Nodes.Add("Email Parameter: " + c.emailProperty.ToString());
				conditionNode.Nodes.Add("Reciever Email: " + string.Join(", ", c.recieverEmails.ToArray()));
				conditionNode.Nodes.Add("Alert Type: " + c.alertType);
				conditionNode.Nodes.Add("Emails Sent: " + c.emailsThisSession.ToString());
				TreeNode triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach(Core.Trigger trigger in c.triggers.Values) {
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
				}
			}
		}

		public void updateStatusLabel(string text) {
			SuspendDrawing(statusStrip1);
			statusLabel.Text = "Status: " + text;
			ResumeDrawing(statusStrip1);
		}

		private void startConditionEditorToolStripMenuItem_Click(object sender, EventArgs e) {
			Form1 editor = new Form1();
			editor.Show();
			editor.FormClosing += delegate { 
				Checker.LoadConditions();
				updateConditionList();
			};		
		}

		private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e) {
			Process.Start("alerts.log");
		}
		
		void ReloadConditionsToolStripMenuItemClick(object sender, EventArgs e) {
			reloadConditionsToolStripMenuItem.Enabled = false;
			ThreadManager.Restart();
		}
		
		void PlaneAlerterFormClosing(object sender, FormClosingEventArgs e) {
			if (Core.statsThread != null)
				Core.statsThread.Abort();
			if (Core.loopThread != null)
				Core.loopThread.Abort();
			
			string serialisedSettings = JsonConvert.SerializeObject(Settings.returnSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);
			string serialisedEcc = JsonConvert.SerializeObject(Settings.returnECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}
		
		void ConsoleLinkClicked(object sender, LinkClickedEventArgs e) {
			Process.Start(e.LinkText);
		}

		private void clearConsoleToolStripMenuItem_Click(object sender, EventArgs e) {
			console.Text = "";
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			SettingsForm settingsForm = new SettingsForm();
			settingsForm.ShowDialog();
			Settings.updateSettings(true);
		}

		private void propertyInfoToolStripMenuItem_Click(object sender, EventArgs e) {
			PropertyInfoForm propertyInfoForm = new PropertyInfoForm();
			propertyInfoForm.Show();
		}

		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e) {
			Activate();
		}

		private void emailContentConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			Email_Content_Config ecc = new Email_Content_Config();
			ecc.ShowDialog();
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e) {
			Process.Start(@"http://donate.dom2364.com");
		}
	}
}