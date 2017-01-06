using System.Windows.Forms;

namespace PlaneAlerter {
	public partial class PropertyInfoForm :Form {
		public PropertyInfoForm() {
			//Initialise form elements
			InitializeComponent();

			//Add vrs property info to form
			foreach (Core.vrsProperty property in Core.vrsPropertyData.Keys) {
				string[] propertyData = Core.vrsPropertyData[property];
                propertyDataGridView.Rows.Add(property.ToString(), propertyData[0], propertyData[2], propertyData[3]);
			}
		}
	}
}
