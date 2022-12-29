using System.Windows.Forms;

namespace PlaneAlerter.Forms {
	public partial class PropertyInfoForm :Form {
		public PropertyInfoForm() {
			//Initialise form elements
			InitializeComponent();

			//Add vrs property info to form
			foreach (var property in Core.vrsPropertyData.Keys) {
				var propertyData = Core.vrsPropertyData[property];
                propertyDataGridView.Rows.Add(property.ToString(), propertyData[0], propertyData[2], propertyData[3]);
			}
		}
	}
}
