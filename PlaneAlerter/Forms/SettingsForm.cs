using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for changing settings
	/// </summary>
	internal partial class SettingsForm :Form {
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IVrsService _vrsService;

		/// <summary>
		/// Smtp host info
		/// </summary>
		private static Dictionary<string, object[]> HostInfo { get; } = new();

		/// <summary>
		/// Constructor
		/// </summary>
		public SettingsForm(ISettingsManagerService settingsManagerService, IVrsService vrsService) {
			_settingsManagerService = settingsManagerService;
			_vrsService = vrsService;

			//Initialise form elements
			InitializeComponent();

			//Add smtp host info
			HostInfo.Clear();
			HostInfo.Add("smtp.gmail.com", new object[] { 587, true });
			HostInfo.Add("smtp.live.com", new object[] { 587, true });
			HostInfo.Add("smtp.office365.com", new object[] { 587, true });
			HostInfo.Add("smtp.mail.yahoo.com", new object[] { 465, true });
			HostInfo.Add("plus.smtp.mail.yahoo.com", new object[] { 465, true });
			HostInfo.Add("smtp.mail.yahoo.co.uk", new object[] { 465, true });
			HostInfo.Add("smtp.mail.yahoo.com.au", new object[] { 465, true });
			HostInfo.Add("smtp.att.yahoo.com", new object[] { 465, true });
			HostInfo.Add("smtp.comcast.net", new object[] { 587, false });
			HostInfo.Add("outgoing.verizon.net", new object[] { 465, true });
			HostInfo.Add("smtp.mail.com", new object[] { 465, true });

			//Add smtp host info to combobox
			foreach (var smtpHost in HostInfo.Keys)
				smtpHostComboBox.Items.Add(smtpHost);

			UpdateReceivers();

			//Set settings from current settings
			senderEmailTextBox.Text = _settingsManagerService.Settings.SenderEmail;
			aircraftListTextBox.Text = _settingsManagerService.Settings.AircraftListUrl;
			radarURLTextBox.Text = _settingsManagerService.Settings.RadarUrl;
			VRSUsrTextBox.Text = _settingsManagerService.Settings.VRSUser;
			VRSPwdTextBox.Text = _settingsManagerService.Settings.VRSPassword;
			latTextBox.Value = _settingsManagerService.Settings.Lat;
			longTextBox.Value = _settingsManagerService.Settings.Long;
			removalTimeoutTextBox.Value = _settingsManagerService.Settings.RemovalTimeout;
			refreshTextBox.Value = _settingsManagerService.Settings.RefreshRate;
			startOnStartCheckBox.Checked = _settingsManagerService.Settings.StartOnStart;
			soundAlertsCheckBox.Checked = _settingsManagerService.Settings.SoundAlerts;
			flashWindowCheckBox.Checked = _settingsManagerService.Settings.FlashWindow;
			notificationsCheckBox.Checked = _settingsManagerService.Settings.ShowNotifications;
			centreAircraftRadioButton.Checked = _settingsManagerService.Settings.CentreMapOnAircraft;
			centreLatLngRadioButton.Checked = !_settingsManagerService.Settings.CentreMapOnAircraft;
			smtpHostComboBox.Text = _settingsManagerService.Settings.SMTPHost;
			smtpHostPortTextBox.Value = _settingsManagerService.Settings.SMTPPort;
			smtpUsrTextBox.Text = _settingsManagerService.Settings.SMTPUser;
			smtpPwdTextBox.Text = _settingsManagerService.Settings.SMTPPassword;
			smtpSSLCheckBox.Checked = _settingsManagerService.Settings.SMTPSSL;
			timeoutTextBox.Value = _settingsManagerService.Settings.Timeout;
			filterDstCheckBox.Checked = _settingsManagerService.Settings.FilterDistance;
			filterDstCheckBox_CheckedChanged(this, EventArgs.Empty);
			filterAltCheckBox.Checked = _settingsManagerService.Settings.FilterAltitude;
			filterAltCheckBox_CheckedChanged(this, EventArgs.Empty);
			ignoreModeSCheckBox.Checked = _settingsManagerService.Settings.IgnoreModeS;
			ignoreDistTextBox.Value = Convert.ToDecimal(_settingsManagerService.Settings.IgnoreDistance);
			ignoreAltTextBox.Value = _settingsManagerService.Settings.IgnoreAltitude;
			if (filterReceiverCheckBox.Checked) filterReceiverCheckBox.Checked = _settingsManagerService.Settings.FilterReceiver; //Will be unchecked if there was an error getting receivers
			filterReceiverCheckBox_CheckedChanged(this, EventArgs.Empty);
			trailsAgeNumericUpDown.Value = _settingsManagerService.Settings.TrailsUpdateFrequency;
		}

		private async void UpdateReceivers() {
			if (string.IsNullOrWhiteSpace(_settingsManagerService.Settings.AircraftListUrl)) {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				filterReceiverCheckBox.Checked = false;
				return;
			}

			var receivers = await Task.Run(_vrsService.GetReceivers);
			if (receivers != null) {
				receivers = receivers.OrderBy(x => x.Value, StringComparer.Ordinal).ToDictionary(x => x.Key, x => x.Value);

				receiverComboBox.DataSource = new BindingSource(receivers, null);
				receiverComboBox.DisplayMember = "Value";
				receiverComboBox.ValueMember = "Key";
				receiverComboBox.SelectedValue = _settingsManagerService.Settings.FilterReceiverId.ToString();
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
		private void smtpHostComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			//If exists in smtp host info, set port and ssl values
			if (!HostInfo.ContainsKey(smtpHostComboBox.Text))
				return;

			smtpHostPortTextBox.Value = Convert.ToDecimal(HostInfo[smtpHostComboBox.Text][0]);
			smtpSSLCheckBox.Checked = (bool)HostInfo[smtpHostComboBox.Text][1];
		}

		/// <summary>
		/// Settings form closing
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
			//Set settings to form element values
			_settingsManagerService.Settings = new Settings(
				senderEmailTextBox.Text,
				aircraftListTextBox.Text,
				VRSUsrTextBox.Text,
				VRSPwdTextBox.Text,
				latTextBox.Value,
				longTextBox.Value,
				radarURLTextBox.Text,
				centreAircraftRadioButton.Checked,
				Convert.ToInt32(removalTimeoutTextBox.Value),
				Convert.ToInt32(refreshTextBox.Value),
				startOnStartCheckBox.Checked,
				Convert.ToInt32(timeoutTextBox.Value),
				notificationsCheckBox.Checked,
				soundAlertsCheckBox.Checked,
				flashWindowCheckBox.Checked,
				filterDstCheckBox.Checked,
				filterAltCheckBox.Checked,
				ignoreModeSCheckBox.Checked,
				Convert.ToDouble(ignoreDistTextBox.Value),
				Convert.ToInt32(ignoreAltTextBox.Value),
				filterReceiverCheckBox.Checked,
				Convert.ToInt32(receiverComboBox.SelectedValue),
				Convert.ToInt32(trailsAgeNumericUpDown.Value),
				smtpHostComboBox.Text,
				Convert.ToInt32(smtpHostPortTextBox.Value),
				smtpUsrTextBox.Text,
				smtpPwdTextBox.Text,
				smtpSSLCheckBox.Checked,
				_settingsManagerService.Settings.TwitterUsers);
			
			_settingsManagerService.Save();
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
			_settingsManagerService.Settings.AircraftListUrl = aircraftListTextBox.Text;
			UpdateReceivers();
		}
	}
}
