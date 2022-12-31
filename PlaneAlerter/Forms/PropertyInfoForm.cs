using System.Windows.Forms;

namespace PlaneAlerter.Forms {
	internal partial class PropertyInfoForm :Form {
		public PropertyInfoForm() {
			//Initialise form elements
			InitializeComponent();

			//Add vrs property info to form
			foreach (var property in VrsProperties.VrsPropertyData.Keys) {
				var propertyData = VrsProperties.VrsPropertyData[property];
                propertyDataGridView.Rows.Add(property.ToString(), propertyData[0], propertyData[2], propertyData[3]);
			}
		}
	}
}
