using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

			UpdateReceivers();

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
			centreAircraftRadioButton.Checked = Settings.centreMapOnAircraft;
			centreLatLngRadioButton.Checked = !Settings.centreMapOnAircraft;
			smtpHostComboBox.Text = Settings.SMTPHost;
			smtpHostPortTextBox.Value = Settings.SMTPPort;
			smtpUsrTextBox.Text = Settings.SMTPUsr;
			smtpPwdTextBox.Text = Settings.SMTPPwd;
			smtpSSLCheckBox.Checked = Settings.SMTPSSL;
			timeoutTextBox.Value = Settings.timeout;
			filterDstCheckBox.Checked = Settings.filterDistance;
			filterDstCheckBox_CheckedChanged(this, new EventArgs());
			filterAltCheckBox.Checked = Settings.filterAltitude;
			filterAltCheckBox_CheckedChanged(this, new EventArgs());
			ignoreModeSCheckBox.Checked = Settings.ignoreModeS;
			ignoreDistTextBox.Value = Convert.ToDecimal(Settings.ignoreDistance);
			ignoreAltTextBox.Value = Settings.ignoreAltitude;
			if (filterReceiverCheckBox.Checked) filterReceiverCheckBox.Checked = Settings.filterReceiver; //Will be unchecked if there was an error getting receivers
			filterReceiverCheckBox_CheckedChanged(this, new EventArgs());
			trailsAgeNumericUpDown.Value = Settings.trailsUpdateFrequency;
		}

		private async void UpdateReceivers() {
			if (string.IsNullOrWhiteSpace(Settings.acListUrl)) {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				filterReceiverCheckBox.Checked = false;
				return;
			}

			Dictionary<string, string> receivers = await Task.Run(() => Checker.GetReceivers());
			if (receivers != null) {
				receivers = receivers.OrderBy(x => x.Value, StringComparer.Ordinal).ToDictionary(x => x.Key, x => x.Value);

				receiverComboBox.DataSource = new BindingSource(receivers, null);
				receiverComboBox.DisplayMember = "Value";
				receiverComboBox.ValueMember = "Key";
				receiverComboBox.SelectedValue = Settings.filterReceiverId.ToString();
			}
			else {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				filterReceiverCheckBox.Checked = false;
			}
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
			Settings.centreMapOnAircraft = centreAircraftRadioButton.Checked;
			Settings.SMTPHost = smtpHostComboBox.Text;
			Settings.SMTPPort = Convert.ToInt32(smtpHostPortTextBox.Value);
			Settings.SMTPUsr = smtpUsrTextBox.Text;
			Settings.SMTPPwd = smtpPwdTextBox.Text;
			Settings.SMTPSSL = smtpSSLCheckBox.Checked;
			Settings.timeout = Convert.ToInt32(timeoutTextBox.Value);
			Settings.filterDistance = filterDstCheckBox.Checked;
			Settings.filterAltitude = filterAltCheckBox.Checked;
			Settings.ignoreModeS = ignoreModeSCheckBox.Checked;
			Settings.ignoreAltitude = Convert.ToInt32(ignoreAltTextBox.Value);
			Settings.ignoreDistance = Convert.ToDouble(ignoreDistTextBox.Value);
			Settings.filterReceiver = filterReceiverCheckBox.Checked;
			Settings.filterReceiverId = Convert.ToInt32(receiverComboBox.SelectedValue);
			Settings.trailsUpdateFrequency = Convert.ToInt32(trailsAgeNumericUpDown.Value);
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

		private void filterDstCheckBox_CheckedChanged(object sender, EventArgs e) {
			ignoreDistTextBox.Enabled = filterDstCheckBox.Checked;
			ignoreModeSCheckBox.Enabled = filterDstCheckBox.Checked;
		}

		private void filterAltCheckBox_CheckedChanged(object sender, EventArgs e) {
			ignoreAltTextBox.Enabled = filterAltCheckBox.Checked;
		}

		private void filterReceiverCheckBox_CheckedChanged(object sender, EventArgs e) {
			receiverComboBox.Enabled = filterReceiverCheckBox.Checked;
		}

		private void refreshReceiversButton_Click(object sender, EventArgs e) {
			Settings.acListUrl = aircraftListTextBox.Text;
			UpdateReceivers();
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
