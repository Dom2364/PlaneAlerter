using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for changing settings
	/// </summary>
	internal partial class SettingsForm :Form {
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
			senderEmailTextBox.Text = Settings.SenderEmail;
			aircraftListTextBox.Text = Settings.AircraftListUrl;
			radarURLTextBox.Text = Settings.RadarUrl;
			VRSUsrTextBox.Text = Settings.VRSUser;
			VRSPwdTextBox.Text = Settings.VRSPassword;
			latTextBox.Value = Settings.Lat;
			longTextBox.Value = Settings.Long;
			removalTimeoutTextBox.Value = Settings.RemovalTimeout;
			refreshTextBox.Value = Settings.RefreshRate;
			startOnStartCheckBox.Checked = Settings.StartOnStart;
			soundAlertsCheckBox.Checked = Settings.SoundAlerts;
			notificationsCheckBox.Checked = Settings.ShowNotifications;
			centreAircraftRadioButton.Checked = Settings.CentreMapOnAircraft;
			centreLatLngRadioButton.Checked = !Settings.CentreMapOnAircraft;
			smtpHostComboBox.Text = Settings.SMTPHost;
			smtpHostPortTextBox.Value = Settings.SMTPPort;
			smtpUsrTextBox.Text = Settings.SMTPUser;
			smtpPwdTextBox.Text = Settings.SMTPPassword;
			smtpSSLCheckBox.Checked = Settings.SMTPSSL;
			timeoutTextBox.Value = Settings.Timeout;
			filterDstCheckBox.Checked = Settings.FilterDistance;
			filterDstCheckBox_CheckedChanged(this, new EventArgs());
			filterAltCheckBox.Checked = Settings.FilterAltitude;
			filterAltCheckBox_CheckedChanged(this, new EventArgs());
			ignoreModeSCheckBox.Checked = Settings.IgnoreModeS;
			ignoreDistTextBox.Value = Convert.ToDecimal(Settings.IgnoreDistance);
			ignoreAltTextBox.Value = Settings.IgnoreAltitude;
			if (filterReceiverCheckBox.Checked) filterReceiverCheckBox.Checked = Settings.FilterReceiver; //Will be unchecked if there was an error getting receivers
			filterReceiverCheckBox_CheckedChanged(this, new EventArgs());
			trailsAgeNumericUpDown.Value = Settings.TrailsUpdateFrequency;
		}

		private async void UpdateReceivers() {
			if (string.IsNullOrWhiteSpace(Settings.AircraftListUrl)) {
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
				receiverComboBox.SelectedValue = Settings.FilterReceiverId.ToString();
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
			Settings.SenderEmail = senderEmailTextBox.Text;
			Settings.AircraftListUrl = aircraftListTextBox.Text;
			Settings.RadarUrl = radarURLTextBox.Text;
			Settings.VRSUser = VRSUsrTextBox.Text;
			Settings.VRSPassword = VRSPwdTextBox.Text;
			Settings.Lat = latTextBox.Value;
			Settings.Long = longTextBox.Value;
			Settings.RemovalTimeout = Convert.ToInt32(removalTimeoutTextBox.Value);
			Settings.RefreshRate = Convert.ToInt32(refreshTextBox.Value);
			Settings.StartOnStart = startOnStartCheckBox.Checked;
			Settings.SoundAlerts = soundAlertsCheckBox.Checked;
			Settings.ShowNotifications = notificationsCheckBox.Checked;
			Settings.CentreMapOnAircraft = centreAircraftRadioButton.Checked;
			Settings.SMTPHost = smtpHostComboBox.Text;
			Settings.SMTPPort = Convert.ToInt32(smtpHostPortTextBox.Value);
			Settings.SMTPUser = smtpUsrTextBox.Text;
			Settings.SMTPPassword = smtpPwdTextBox.Text;
			Settings.SMTPSSL = smtpSSLCheckBox.Checked;
			Settings.Timeout = Convert.ToInt32(timeoutTextBox.Value);
			Settings.FilterDistance = filterDstCheckBox.Checked;
			Settings.FilterAltitude = filterAltCheckBox.Checked;
			Settings.IgnoreModeS = ignoreModeSCheckBox.Checked;
			Settings.IgnoreAltitude = Convert.ToInt32(ignoreAltTextBox.Value);
			Settings.IgnoreDistance = Convert.ToDouble(ignoreDistTextBox.Value);
			Settings.FilterReceiver = filterReceiverCheckBox.Checked;
			Settings.FilterReceiverId = Convert.ToInt32(receiverComboBox.SelectedValue);
			Settings.TrailsUpdateFrequency = Convert.ToInt32(trailsAgeNumericUpDown.Value);
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
		private void gmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			//Open gmail link
			Process.Start(new ProcessStartInfo("https://www.google.com/settings/security/lesssecureapps")
			{
				UseShellExecute = true
			});
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
			Settings.AircraftListUrl = aircraftListTextBox.Text;
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
