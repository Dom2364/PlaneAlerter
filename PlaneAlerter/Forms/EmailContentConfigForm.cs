using System;
using System.Windows.Forms;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for changing email content config
	/// </summary>
	internal partial class EmailContentConfigForm :Form {
		private readonly ISettingsManagerService _settingsManagerService;

		/// <summary>
		/// Constructor
		/// </summary>
		public EmailContentConfigForm(ISettingsManagerService settingsManagerService) {
			_settingsManagerService = settingsManagerService;

			//Initialise form components
			InitializeComponent();

			//Update form elements with current settings
			receiverNameCheckBox.Checked = _settingsManagerService.EmailContentConfig.ReceiverName;
			transponderTypeCheckBox.Checked = _settingsManagerService.EmailContentConfig.TransponderType;
			radarLinkCheckBox.Checked = _settingsManagerService.EmailContentConfig.RadarLink;
			afLookupCheckBox.Checked = _settingsManagerService.EmailContentConfig.AfLookup;
			mapCheckBox.Checked = _settingsManagerService.EmailContentConfig.Map;
			photosCheckBox.Checked = _settingsManagerService.EmailContentConfig.AircraftPhotos;
			reportCheckBox.Checked = _settingsManagerService.EmailContentConfig.ReportLink;
            twitterCheckBox.Checked = _settingsManagerService.EmailContentConfig.TwitterOptimised;
			kmlCheckbox.Checked = _settingsManagerService.EmailContentConfig.KmlFile;
            switch (_settingsManagerService.EmailContentConfig.PropertyList) {
				case PropertyListType.All:
					plAll.Checked = true;
					plEssentials.Checked = false;
					plHidden.Checked = false;
					break;
				case PropertyListType.Essentials:
					plAll.Checked = false;
					plEssentials.Checked = true;
					plHidden.Checked = false;
					break;
				case PropertyListType.Hidden:
					plAll.Checked = false;
					plEssentials.Checked = false;
					plHidden.Checked = true;
					break;
			}
		}

		/// <summary>
		/// Save button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void saveButton_Click(object sender, EventArgs e)
		{
			PropertyListType propertyList;

			if (plAll.Checked)
				propertyList = PropertyListType.All;
			else if (plEssentials.Checked)
				propertyList = PropertyListType.Essentials;
			else
				propertyList = PropertyListType.Hidden;

			//Save settings to current settings
			_settingsManagerService.EmailContentConfig = new EmailContentConfig(
				afLookupCheckBox.Checked,
				photosCheckBox.Checked,
				mapCheckBox.Checked,
				kmlCheckbox.Checked,
				propertyList,
				radarLinkCheckBox.Checked,
				reportCheckBox.Checked,
				receiverNameCheckBox.Checked,
				transponderTypeCheckBox.Checked,
				twitterCheckBox.Checked
			);

			//Close form
			Close();
		}

        /// <summary>
        /// Twitter check box click
        /// </summary>
        private void twitterCheckBox_CheckedChanged(object sender, EventArgs e) {
            checkBoxesPanel.Enabled = !twitterCheckBox.Checked;
            propertyListGroupBox.Enabled = !twitterCheckBox.Checked;
        }
    }
}
