using System;
using System.Windows.Forms;

namespace PlaneAlerter.Forms {
	internal partial class PinPromptDialog : Form {
		public string? Pin { get; set; }

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
				Pin = pinTextBox.Text;
				DialogResult = DialogResult.OK;
			}
			else {
				DialogResult = DialogResult.Cancel;
			}
		}
	}
}
