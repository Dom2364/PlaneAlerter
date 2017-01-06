using System; 
using System.Windows.Forms;

namespace PlaneAlerter {
	/// <summary>
	/// Form for changing email content config
	/// </summary>
	public partial class Email_Content_Config :Form {
		/// <summary>
		/// Constructor
		/// </summary>
		public Email_Content_Config() {
			//Initialise form components
			InitializeComponent();

			//Update form elements with current settings
			receiverNameCheckBox.Checked = Settings.EmailContentConfig.ReceiverName;
			transponderTypeCheckBox.Checked = Settings.EmailContentConfig.TransponderType;
			radarLinkCheckBox.Checked = Settings.EmailContentConfig.RadarLink;
			afLookupCheckBox.Checked = Settings.EmailContentConfig.AfLookup;
			mapCheckBox.Checked = Settings.EmailContentConfig.Map;
			photosCheckBox.Checked = Settings.EmailContentConfig.AircraftPhotos;
			reportCheckBox.Checked = Settings.EmailContentConfig.ReportLink;
			switch (Settings.EmailContentConfig.PropertyList) {
				case Core.PropertyListType.All:
					plAll.Checked = true;
					plEssentials.Checked = false;
					plHidden.Checked = false;
					break;
				case Core.PropertyListType.Essentials:
					plAll.Checked = false;
					plEssentials.Checked = true;
					plHidden.Checked = false;
					break;
				case Core.PropertyListType.Hidden:
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
		private void saveButton_Click(object sender, EventArgs e) {
			//Save settings to current settings
			Settings.EmailContentConfig.ReceiverName = receiverNameCheckBox.Checked;
			Settings.EmailContentConfig.TransponderType = transponderTypeCheckBox.Checked;
			Settings.EmailContentConfig.RadarLink = radarLinkCheckBox.Checked;
			Settings.EmailContentConfig.AfLookup = afLookupCheckBox.Checked;
			Settings.EmailContentConfig.AircraftPhotos = photosCheckBox.Checked;
			Settings.EmailContentConfig.Map = mapCheckBox.Checked;
			Settings.EmailContentConfig.ReportLink = reportCheckBox.Checked;
			if (plAll.Checked)
				Settings.EmailContentConfig.PropertyList = Core.PropertyListType.All;
			else if (plEssentials.Checked)
				Settings.EmailContentConfig.PropertyList = Core.PropertyListType.Essentials;
			else
				Settings.EmailContentConfig.PropertyList = Core.PropertyListType.Hidden;

			//Close form
			Close();
		}
	}
}
