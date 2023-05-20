namespace PlaneAlerter.Forms {
	partial class EmailContentConfigForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailContentConfigForm));
			this.receiverNameCheckBox = new System.Windows.Forms.CheckBox();
			this.transponderTypeCheckBox = new System.Windows.Forms.CheckBox();
			this.plHidden = new System.Windows.Forms.RadioButton();
			this.propertyListGroupBox = new System.Windows.Forms.GroupBox();
			this.plAll = new System.Windows.Forms.RadioButton();
			this.plEssentials = new System.Windows.Forms.RadioButton();
			this.radarLinkCheckBox = new System.Windows.Forms.CheckBox();
			this.afLookupCheckBox = new System.Windows.Forms.CheckBox();
			this.photosCheckBox = new System.Windows.Forms.CheckBox();
			this.mapCheckBox = new System.Windows.Forms.CheckBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.reportCheckBox = new System.Windows.Forms.CheckBox();
			this.twitterCheckBox = new System.Windows.Forms.CheckBox();
			this.checkBoxesPanel = new System.Windows.Forms.Panel();
			this.kmlCheckbox = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.propertyListGroupBox.SuspendLayout();
			this.checkBoxesPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// receiverNameCheckBox
			// 
			this.receiverNameCheckBox.AutoSize = true;
			this.receiverNameCheckBox.Location = new System.Drawing.Point(4, 3);
			this.receiverNameCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.receiverNameCheckBox.Name = "receiverNameCheckBox";
			this.receiverNameCheckBox.Size = new System.Drawing.Size(105, 19);
			this.receiverNameCheckBox.TabIndex = 0;
			this.receiverNameCheckBox.Text = "Receiver Name";
			this.receiverNameCheckBox.UseVisualStyleBackColor = true;
			// 
			// transponderTypeCheckBox
			// 
			this.transponderTypeCheckBox.AutoSize = true;
			this.transponderTypeCheckBox.Location = new System.Drawing.Point(4, 30);
			this.transponderTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.transponderTypeCheckBox.Name = "transponderTypeCheckBox";
			this.transponderTypeCheckBox.Size = new System.Drawing.Size(118, 19);
			this.transponderTypeCheckBox.TabIndex = 1;
			this.transponderTypeCheckBox.Text = "Transponder Type";
			this.transponderTypeCheckBox.UseVisualStyleBackColor = true;
			// 
			// plHidden
			// 
			this.plHidden.AutoSize = true;
			this.plHidden.Location = new System.Drawing.Point(16, 22);
			this.plHidden.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.plHidden.Name = "plHidden";
			this.plHidden.Size = new System.Drawing.Size(64, 19);
			this.plHidden.TabIndex = 2;
			this.plHidden.Text = "Hidden";
			this.plHidden.UseVisualStyleBackColor = true;
			// 
			// propertyListGroupBox
			// 
			this.propertyListGroupBox.Controls.Add(this.plAll);
			this.propertyListGroupBox.Controls.Add(this.plEssentials);
			this.propertyListGroupBox.Controls.Add(this.plHidden);
			this.propertyListGroupBox.Location = new System.Drawing.Point(172, 39);
			this.propertyListGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.propertyListGroupBox.Name = "propertyListGroupBox";
			this.propertyListGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.propertyListGroupBox.Size = new System.Drawing.Size(160, 108);
			this.propertyListGroupBox.TabIndex = 3;
			this.propertyListGroupBox.TabStop = false;
			this.propertyListGroupBox.Text = "Property List";
			// 
			// plAll
			// 
			this.plAll.AutoSize = true;
			this.plAll.Checked = true;
			this.plAll.Location = new System.Drawing.Point(16, 75);
			this.plAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.plAll.Name = "plAll";
			this.plAll.Size = new System.Drawing.Size(95, 19);
			this.plAll.TabIndex = 4;
			this.plAll.TabStop = true;
			this.plAll.Text = "All properties";
			this.plAll.UseVisualStyleBackColor = true;
			// 
			// plEssentials
			// 
			this.plEssentials.AutoSize = true;
			this.plEssentials.Location = new System.Drawing.Point(16, 48);
			this.plEssentials.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.plEssentials.Name = "plEssentials";
			this.plEssentials.Size = new System.Drawing.Size(97, 19);
			this.plEssentials.TabIndex = 3;
			this.plEssentials.Text = "The essentials";
			this.plEssentials.UseVisualStyleBackColor = true;
			// 
			// radarLinkCheckBox
			// 
			this.radarLinkCheckBox.AutoSize = true;
			this.radarLinkCheckBox.Location = new System.Drawing.Point(4, 57);
			this.radarLinkCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.radarLinkCheckBox.Name = "radarLinkCheckBox";
			this.radarLinkCheckBox.Size = new System.Drawing.Size(81, 19);
			this.radarLinkCheckBox.TabIndex = 4;
			this.radarLinkCheckBox.Text = "Radar Link";
			this.radarLinkCheckBox.UseVisualStyleBackColor = true;
			// 
			// afLookupCheckBox
			// 
			this.afLookupCheckBox.AutoSize = true;
			this.afLookupCheckBox.Location = new System.Drawing.Point(4, 111);
			this.afLookupCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.afLookupCheckBox.Name = "afLookupCheckBox";
			this.afLookupCheckBox.Size = new System.Drawing.Size(139, 19);
			this.afLookupCheckBox.TabIndex = 5;
			this.afLookupCheckBox.Text = "airframes.org Lookup";
			this.afLookupCheckBox.UseVisualStyleBackColor = true;
			// 
			// photosCheckBox
			// 
			this.photosCheckBox.AutoSize = true;
			this.photosCheckBox.Location = new System.Drawing.Point(4, 136);
			this.photosCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.photosCheckBox.Name = "photosCheckBox";
			this.photosCheckBox.Size = new System.Drawing.Size(105, 19);
			this.photosCheckBox.TabIndex = 6;
			this.photosCheckBox.Text = "Aircraft Photos";
			this.photosCheckBox.UseVisualStyleBackColor = true;
			// 
			// mapCheckBox
			// 
			this.mapCheckBox.AutoSize = true;
			this.mapCheckBox.Location = new System.Drawing.Point(4, 162);
			this.mapCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.mapCheckBox.Name = "mapCheckBox";
			this.mapCheckBox.Size = new System.Drawing.Size(126, 19);
			this.mapCheckBox.TabIndex = 7;
			this.mapCheckBox.Text = "Position Track Map";
			this.mapCheckBox.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(174, 196);
			this.saveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(160, 27);
			this.saveButton.TabIndex = 8;
			this.saveButton.Text = "Ok";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// reportCheckBox
			// 
			this.reportCheckBox.AutoSize = true;
			this.reportCheckBox.Location = new System.Drawing.Point(4, 83);
			this.reportCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.reportCheckBox.Name = "reportCheckBox";
			this.reportCheckBox.Size = new System.Drawing.Size(86, 19);
			this.reportCheckBox.TabIndex = 9;
			this.reportCheckBox.Text = "Report Link";
			this.reportCheckBox.UseVisualStyleBackColor = true;
			// 
			// twitterCheckBox
			// 
			this.twitterCheckBox.AutoSize = true;
			this.twitterCheckBox.Location = new System.Drawing.Point(172, 14);
			this.twitterCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.twitterCheckBox.Name = "twitterCheckBox";
			this.twitterCheckBox.Size = new System.Drawing.Size(119, 19);
			this.twitterCheckBox.TabIndex = 10;
			this.twitterCheckBox.Text = "Twitter Optimised";
			this.toolTip1.SetToolTip(this.twitterCheckBox, "A short single-line message for use in services that foward emails to tweets");
			this.twitterCheckBox.UseVisualStyleBackColor = true;
			this.twitterCheckBox.CheckedChanged += new System.EventHandler(this.twitterCheckBox_CheckedChanged);
			// 
			// checkBoxesPanel
			// 
			this.checkBoxesPanel.Controls.Add(this.kmlCheckbox);
			this.checkBoxesPanel.Controls.Add(this.receiverNameCheckBox);
			this.checkBoxesPanel.Controls.Add(this.transponderTypeCheckBox);
			this.checkBoxesPanel.Controls.Add(this.reportCheckBox);
			this.checkBoxesPanel.Controls.Add(this.radarLinkCheckBox);
			this.checkBoxesPanel.Controls.Add(this.afLookupCheckBox);
			this.checkBoxesPanel.Controls.Add(this.mapCheckBox);
			this.checkBoxesPanel.Controls.Add(this.photosCheckBox);
			this.checkBoxesPanel.Location = new System.Drawing.Point(14, 10);
			this.checkBoxesPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.checkBoxesPanel.Name = "checkBoxesPanel";
			this.checkBoxesPanel.Size = new System.Drawing.Size(152, 212);
			this.checkBoxesPanel.TabIndex = 11;
			// 
			// kmlCheckbox
			// 
			this.kmlCheckbox.AutoSize = true;
			this.kmlCheckbox.Location = new System.Drawing.Point(4, 187);
			this.kmlCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.kmlCheckbox.Name = "kmlCheckbox";
			this.kmlCheckbox.Size = new System.Drawing.Size(71, 19);
			this.kmlCheckbox.TabIndex = 10;
			this.kmlCheckbox.Text = "KML File";
			this.kmlCheckbox.UseVisualStyleBackColor = true;
			// 
			// EmailContentConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(343, 232);
			this.Controls.Add(this.checkBoxesPanel);
			this.Controls.Add(this.twitterCheckBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.propertyListGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.Name = "EmailContentConfigForm";
			this.Text = "Email Content Config";
			this.propertyListGroupBox.ResumeLayout(false);
			this.propertyListGroupBox.PerformLayout();
			this.checkBoxesPanel.ResumeLayout(false);
			this.checkBoxesPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox receiverNameCheckBox;
		private System.Windows.Forms.CheckBox transponderTypeCheckBox;
		private System.Windows.Forms.RadioButton plHidden;
		private System.Windows.Forms.GroupBox propertyListGroupBox;
		private System.Windows.Forms.RadioButton plEssentials;
		private System.Windows.Forms.RadioButton plAll;
		private System.Windows.Forms.CheckBox radarLinkCheckBox;
		private System.Windows.Forms.CheckBox afLookupCheckBox;
		private System.Windows.Forms.CheckBox photosCheckBox;
		private System.Windows.Forms.CheckBox mapCheckBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.CheckBox reportCheckBox;
        private System.Windows.Forms.CheckBox twitterCheckBox;
        private System.Windows.Forms.Panel checkBoxesPanel;
        private System.Windows.Forms.CheckBox kmlCheckbox;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
