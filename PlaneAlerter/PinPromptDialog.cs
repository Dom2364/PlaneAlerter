using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneAlerter {
	public partial class PinPromptDialog : Form {
		public string PIN { get; set; }

		public PinPromptDialog() {
			InitializeComponent();
		}

		private void submitButton_Click(object sender, EventArgs e) {
			if (!string.IsNullOrWhiteSpace(pinTextBox.Text)) {
				Close();
			}
		}

		private void PinPromptDialog_FormClosing(object sender, FormClosingEventArgs e) {
			if (!string.IsNullOrWhiteSpace(pinTextBox.Text)) {
				PIN = pinTextBox.Text;
				DialogResult = DialogResult.OK;
			}
			else {
				DialogResult = DialogResult.Cancel;
			}
		}
	}
}
