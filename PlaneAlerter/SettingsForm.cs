using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace PlaneAlerter {
	/// <summary>
	/// Form for changing settings
	/// </summary>
	public partial class SettingsForm :Form {
		/// <summary>
		/// Constructor
		/// </summary>
		public SettingsForm() {
			//Initialise form elements
			InitializeComponent();

			//Add smtp host info
			smtpHostInfo.hostInfo.Clear();
			smtpHostInfo.hostInfo.Add("smtp.gmail.com", new object[] { 587, true });
			smtpHostInfo.hostInfo.Add("smtp.live.com", new object[] { 587, true });
			smtpHostInfo.hostInfo.Add("smtp.office365.com", new object[] { 587, true });
			smtpHostInfo.hostInfo.Add("smtp.mail.yahoo.com", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("plus.smtp.mail.yahoo.com", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("smtp.mail.yahoo.co.uk", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("smtp.mail.yahoo.com.au", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("smtp.att.yahoo.com", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("smtp.comcast.net", new object[] { 587, false });
			smtpHostInfo.hostInfo.Add("outgoing.verizon.net", new object[] { 465, true });
			smtpHostInfo.hostInfo.Add("smtp.mail.com", new object[] { 465, true });

			//Add smtp host info to combobox
			foreach (string smtpHost in smtpHostInfo.hostInfo.Keys)
				smtpHostComboBox.Items.Add(smtpHost);

			//Set settings from current settings
			senderEmailTextBox.Text = Settings.senderEmail;
			aircraftListTextBox.Text = Settings.acListUrl;
			radarURLTextBox.Text = Settings.radarUrl;
			VRSUsrTextBox.Text = Settings.VRSUsr;
			VRSPwdTextBox.Text = Settings.VRSPwd;
			latTextBox.Value = Settings.Lat;
			longTextBox.Value = Settings.Long;
			removalTimeoutTextBox.Value = Settings.removalTimeout;
			refreshTextBox.Value = Settings.refreshRate;
			startOnStartCheckBox.Checked = Settings.startOnStart;
			soundAlertsCheckBox.Checked = Settings.soundAlerts;
			notificationsCheckBox.Checked = Settings.showNotifications;
			smtpHostComboBox.Text = Settings.SMTPHost;
			smtpHostPortTextBox.Value = Settings.SMTPPort;
			smtpUsrTextBox.Text = Settings.SMTPUsr;
			smtpPwdTextBox.Text = Settings.SMTPPwd;
			smtpSSLCheckBox.Checked = Settings.SMTPSSL;
			timeoutTextBox.Value = Settings.timeout;
		}

		/// <summary>
		/// Smtp combobox value changed
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void smtpHostComboBox_SelectedValueChanged(object sender, EventArgs e) {
			//If exists in smtp host info, set port and ssl values
			if (smtpHostInfo.hostInfo.ContainsKey(smtpHostComboBox.Text)) {
				smtpHostPortTextBox.Value = Convert.ToDecimal(smtpHostInfo.hostInfo[smtpHostComboBox.Text][0]);
				smtpSSLCheckBox.Checked = (bool)smtpHostInfo.hostInfo[smtpHostComboBox.Text][1];
			}
		}

		/// <summary>
		/// Settings form closing
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
			//Set settings to form element values
			Settings.senderEmail = senderEmailTextBox.Text;
			Settings.acListUrl = aircraftListTextBox.Text;
			Settings.radarUrl = radarURLTextBox.Text;
			Settings.VRSUsr = VRSUsrTextBox.Text;
			Settings.VRSPwd = VRSPwdTextBox.Text;
			Settings.Lat = latTextBox.Value;
			Settings.Long = longTextBox.Value;
			Settings.removalTimeout = Convert.ToInt32(removalTimeoutTextBox.Value);
			Settings.refreshRate = Convert.ToInt32(refreshTextBox.Value);
			Settings.startOnStart = startOnStartCheckBox.Checked;
			Settings.soundAlerts = soundAlertsCheckBox.Checked;
			Settings.showNotifications = notificationsCheckBox.Checked;
			Settings.SMTPHost = smtpHostComboBox.Text;
			Settings.SMTPPort = Convert.ToInt32(smtpHostPortTextBox.Value);
			Settings.SMTPUsr = smtpUsrTextBox.Text;
			Settings.SMTPPwd = smtpPwdTextBox.Text;
			Settings.SMTPSSL = smtpSSLCheckBox.Checked;
			Settings.timeout = Convert.ToInt32(timeoutTextBox.Value);
			Settings.Save();
		}

		/// <summary>
		/// Save settings button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void saveSettingsButton_Click(object sender, EventArgs e) {
			//Close form
			Close();
		}

		/// <summary>
		/// Gmail link clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void gmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			//Open gmail link
			Process.Start("https://www.google.com/settings/security/lesssecureapps");
		}
	}

	/// <summary>
	/// Smtp host info
	/// </summary>
	public static class smtpHostInfo {
		/// <summary>
		/// Smtp host info
		/// </summary>
		public static Dictionary<string, object[]> hostInfo = new Dictionary<string, object[]>();
	}
}
