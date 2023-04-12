namespace PlaneAlerter.Forms {
	partial class PinPromptDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.pinTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.submitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pinTextBox
			// 
			this.pinTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.pinTextBox.Location = new System.Drawing.Point(14, 45);
			this.pinTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pinTextBox.MaxLength = 8;
			this.pinTextBox.Name = "pinTextBox";
			this.pinTextBox.Size = new System.Drawing.Size(163, 44);
			this.pinTextBox.TabIndex = 0;
			this.pinTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(14, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(163, 33);
			this.label1.TabIndex = 1;
			this.label1.Text = "Enter PIN";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// submitButton
			// 
			this.submitButton.Location = new System.Drawing.Point(13, 95);
			this.submitButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.submitButton.Name = "submitButton";
			this.submitButton.Size = new System.Drawing.Size(163, 27);
			this.submitButton.TabIndex = 2;
			this.submitButton.Text = "Submit";
			this.submitButton.UseVisualStyleBackColor = true;
			this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
			// 
			// PinPromptDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(191, 135);
			this.Controls.Add(this.submitButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pinTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PinPromptDialog";
			this.Text = "Enter PIN";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PinPromptDialog_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox pinTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button submitButton;
	}
}