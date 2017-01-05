using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlaneAlerter {
	public partial class PropertyInfoForm :Form {
		public PropertyInfoForm() {
			InitializeComponent();

			foreach (Core.vrsProperty property in Core.vrsPropertyData.Keys) {
				string[] propertyData = Core.vrsPropertyData[property];
                propertyDataGridView.Rows.Add(property.ToString(), propertyData[0], propertyData[2], propertyData[3]);
			}
		}
	}
}
