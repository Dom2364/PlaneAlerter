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
			this.propertyListGroupBox.SuspendLayout();
			this.checkBoxesPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// receiverNameCheckBox
			// 
			this.receiverNameCheckBox.AutoSize = true;
			this.receiverNameCheckBox.Location = new System.Drawing.Point(3, 3);
			this.receiverNameCheckBox.Name = "receiverNameCheckBox";
			this.receiverNameCheckBox.Size = new System.Drawing.Size(100, 17);
			this.receiverNameCheckBox.TabIndex = 0;
			this.receiverNameCheckBox.Text = "Receiver Name";
			this.receiverNameCheckBox.UseVisualStyleBackColor = true;
			// 
			// transponderTypeCheckBox
			// 
			this.transponderTypeCheckBox.AutoSize = true;
			this.transponderTypeCheckBox.Location = new System.Drawing.Point(3, 26);
			this.transponderTypeCheckBox.Name = "transponderTypeCheckBox";
			this.transponderTypeCheckBox.Size = new System.Drawing.Size(113, 17);
			this.transponderTypeCheckBox.TabIndex = 1;
			this.transponderTypeCheckBox.Text = "Transponder Type";
			this.transponderTypeCheckBox.UseVisualStyleBackColor = true;
			// 
			// plHidden
			// 
			this.plHidden.AutoSize = true;
			this.plHidden.Location = new System.Drawing.Point(14, 19);
			this.plHidden.Name = "plHidden";
			this.plHidden.Size = new System.Drawing.Size(59, 17);
			this.plHidden.TabIndex = 2;
			this.plHidden.Text = "Hidden";
			this.plHidden.UseVisualStyleBackColor = true;
			// 
			// propertyListGroupBox
			// 
			this.propertyListGroupBox.Controls.Add(this.plAll);
			this.propertyListGroupBox.Controls.Add(this.plEssentials);
			this.propertyListGroupBox.Controls.Add(this.plHidden);
			this.propertyListGroupBox.Location = new System.Drawing.Point(147, 34);
			this.propertyListGroupBox.Name = "propertyListGroupBox";
			this.propertyListGroupBox.Size = new System.Drawing.Size(137, 94);
			this.propertyListGroupBox.TabIndex = 3;
			this.propertyListGroupBox.TabStop = false;
			this.propertyListGroupBox.Text = "Property List";
			// 
			// plAll
			// 
			this.plAll.AutoSize = true;
			this.plAll.Checked = true;
			this.plAll.Location = new System.Drawing.Point(14, 65);
			this.plAll.Name = "plAll";
			this.plAll.Size = new System.Drawing.Size(85, 17);
			this.plAll.TabIndex = 4;
			this.plAll.TabStop = true;
			this.plAll.Text = "All properties";
			this.plAll.UseVisualStyleBackColor = true;
			// 
			// plEssentials
			// 
			this.plEssentials.AutoSize = true;
			this.plEssentials.Location = new System.Drawing.Point(14, 42);
			this.plEssentials.Name = "plEssentials";
			this.plEssentials.Size = new System.Drawing.Size(93, 17);
			this.plEssentials.TabIndex = 3;
			this.plEssentials.Text = "The essentials";
			this.plEssentials.UseVisualStyleBackColor = true;
			// 
			// radarLinkCheckBox
			// 
			this.radarLinkCheckBox.AutoSize = true;
			this.radarLinkCheckBox.Location = new System.Drawing.Point(3, 49);
			this.radarLinkCheckBox.Name = "radarLinkCheckBox";
			this.radarLinkCheckBox.Size = new System.Drawing.Size(78, 17);
			this.radarLinkCheckBox.TabIndex = 4;
			this.radarLinkCheckBox.Text = "Radar Link";
			this.radarLinkCheckBox.UseVisualStyleBackColor = true;
			// 
			// afLookupCheckBox
			// 
			this.afLookupCheckBox.AutoSize = true;
			this.afLookupCheckBox.Location = new System.Drawing.Point(3, 96);
			this.afLookupCheckBox.Name = "afLookupCheckBox";
			this.afLookupCheckBox.Size = new System.Drawing.Size(125, 17);
			this.afLookupCheckBox.TabIndex = 5;
			this.afLookupCheckBox.Text = "airframes.org Lookup";
			this.afLookupCheckBox.UseVisualStyleBackColor = true;
			// 
			// photosCheckBox
			// 
			this.photosCheckBox.AutoSize = true;
			this.photosCheckBox.Location = new System.Drawing.Point(3, 118);
			this.photosCheckBox.Name = "photosCheckBox";
			this.photosCheckBox.Size = new System.Drawing.Size(95, 17);
			this.photosCheckBox.TabIndex = 6;
			this.photosCheckBox.Text = "Aircraft Photos";
			this.photosCheckBox.UseVisualStyleBackColor = true;
			// 
			// mapCheckBox
			// 
			this.mapCheckBox.AutoSize = true;
			this.mapCheckBox.Location = new System.Drawing.Point(3, 140);
			this.mapCheckBox.Name = "mapCheckBox";
			this.mapCheckBox.Size = new System.Drawing.Size(118, 17);
			this.mapCheckBox.TabIndex = 7;
			this.mapCheckBox.Text = "Position Track Map";
			this.mapCheckBox.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(149, 170);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(137, 23);
			this.saveButton.TabIndex = 8;
			this.saveButton.Text = "Ok";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// reportCheckBox
			// 
			this.reportCheckBox.AutoSize = true;
			this.reportCheckBox.Location = new System.Drawing.Point(3, 72);
			this.reportCheckBox.Name = "reportCheckBox";
			this.reportCheckBox.Size = new System.Drawing.Size(81, 17);
			this.reportCheckBox.TabIndex = 9;
			this.reportCheckBox.Text = "Report Link";
			this.reportCheckBox.UseVisualStyleBackColor = true;
			// 
			// twitterCheckBox
			// 
			this.twitterCheckBox.AutoSize = true;
			this.twitterCheckBox.Location = new System.Drawing.Point(147, 12);
			this.twitterCheckBox.Name = "twitterCheckBox";
			this.twitterCheckBox.Size = new System.Drawing.Size(107, 17);
			this.twitterCheckBox.TabIndex = 10;
			this.twitterCheckBox.Text = "Twitter Optimised";
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
			this.checkBoxesPanel.Location = new System.Drawing.Point(12, 9);
			this.checkBoxesPanel.Name = "checkBoxesPanel";
			this.checkBoxesPanel.Size = new System.Drawing.Size(130, 184);
			this.checkBoxesPanel.TabIndex = 11;
			// 
			// kmlCheckbox
			// 
			this.kmlCheckbox.AutoSize = true;
			this.kmlCheckbox.Location = new System.Drawing.Point(3, 162);
			this.kmlCheckbox.Name = "kmlCheckbox";
			this.kmlCheckbox.Size = new System.Drawing.Size(67, 17);
			this.kmlCheckbox.TabIndex = 10;
			this.kmlCheckbox.Text = "KML File";
			this.kmlCheckbox.UseVisualStyleBackColor = true;
			// 
			// Email_Content_Config
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(294, 201);
			this.Controls.Add(this.checkBoxesPanel);
			this.Controls.Add(this.twitterCheckBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.propertyListGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}
