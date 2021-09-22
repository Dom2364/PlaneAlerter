namespace PlaneAlerter {
	partial class Email_Content_Config {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Email_Content_Config));
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
            this.MapOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.moLatLong = new System.Windows.Forms.RadioButton();
            this.moAircraft = new System.Windows.Forms.RadioButton();
            this.propertyListGroupBox.SuspendLayout();
            this.checkBoxesPanel.SuspendLayout();
            this.MapOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // receiverNameCheckBox
            // 
            this.receiverNameCheckBox.AutoSize = true;
            this.receiverNameCheckBox.Location = new System.Drawing.Point(4, 5);
            this.receiverNameCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.receiverNameCheckBox.Name = "receiverNameCheckBox";
            this.receiverNameCheckBox.Size = new System.Drawing.Size(143, 24);
            this.receiverNameCheckBox.TabIndex = 0;
            this.receiverNameCheckBox.Text = "Receiver Name";
            this.receiverNameCheckBox.UseVisualStyleBackColor = true;
            // 
            // transponderTypeCheckBox
            // 
            this.transponderTypeCheckBox.AutoSize = true;
            this.transponderTypeCheckBox.Location = new System.Drawing.Point(4, 40);
            this.transponderTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.transponderTypeCheckBox.Name = "transponderTypeCheckBox";
            this.transponderTypeCheckBox.Size = new System.Drawing.Size(163, 24);
            this.transponderTypeCheckBox.TabIndex = 1;
            this.transponderTypeCheckBox.Text = "Transponder Type";
            this.transponderTypeCheckBox.UseVisualStyleBackColor = true;
            // 
            // plHidden
            // 
            this.plHidden.AutoSize = true;
            this.plHidden.Location = new System.Drawing.Point(21, 29);
            this.plHidden.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.plHidden.Name = "plHidden";
            this.plHidden.Size = new System.Drawing.Size(85, 24);
            this.plHidden.TabIndex = 2;
            this.plHidden.Text = "Hidden";
            this.plHidden.UseVisualStyleBackColor = true;
            // 
            // propertyListGroupBox
            // 
            this.propertyListGroupBox.Controls.Add(this.plAll);
            this.propertyListGroupBox.Controls.Add(this.plEssentials);
            this.propertyListGroupBox.Controls.Add(this.plHidden);
            this.propertyListGroupBox.Location = new System.Drawing.Point(221, 53);
            this.propertyListGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyListGroupBox.Name = "propertyListGroupBox";
            this.propertyListGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyListGroupBox.Size = new System.Drawing.Size(206, 145);
            this.propertyListGroupBox.TabIndex = 3;
            this.propertyListGroupBox.TabStop = false;
            this.propertyListGroupBox.Text = "Property List";
            // 
            // plAll
            // 
            this.plAll.AutoSize = true;
            this.plAll.Checked = true;
            this.plAll.Location = new System.Drawing.Point(21, 100);
            this.plAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.plAll.Name = "plAll";
            this.plAll.Size = new System.Drawing.Size(126, 24);
            this.plAll.TabIndex = 4;
            this.plAll.TabStop = true;
            this.plAll.Text = "All properties";
            this.plAll.UseVisualStyleBackColor = true;
            // 
            // plEssentials
            // 
            this.plEssentials.AutoSize = true;
            this.plEssentials.Location = new System.Drawing.Point(21, 65);
            this.plEssentials.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.plEssentials.Name = "plEssentials";
            this.plEssentials.Size = new System.Drawing.Size(136, 24);
            this.plEssentials.TabIndex = 3;
            this.plEssentials.Text = "The essentials";
            this.plEssentials.UseVisualStyleBackColor = true;
            // 
            // radarLinkCheckBox
            // 
            this.radarLinkCheckBox.AutoSize = true;
            this.radarLinkCheckBox.Location = new System.Drawing.Point(4, 76);
            this.radarLinkCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radarLinkCheckBox.Name = "radarLinkCheckBox";
            this.radarLinkCheckBox.Size = new System.Drawing.Size(112, 24);
            this.radarLinkCheckBox.TabIndex = 4;
            this.radarLinkCheckBox.Text = "Radar Link";
            this.radarLinkCheckBox.UseVisualStyleBackColor = true;
            // 
            // afLookupCheckBox
            // 
            this.afLookupCheckBox.AutoSize = true;
            this.afLookupCheckBox.Location = new System.Drawing.Point(4, 147);
            this.afLookupCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.afLookupCheckBox.Name = "afLookupCheckBox";
            this.afLookupCheckBox.Size = new System.Drawing.Size(185, 24);
            this.afLookupCheckBox.TabIndex = 5;
            this.afLookupCheckBox.Text = "airframes.org Lookup";
            this.afLookupCheckBox.UseVisualStyleBackColor = true;
            // 
            // photosCheckBox
            // 
            this.photosCheckBox.AutoSize = true;
            this.photosCheckBox.Location = new System.Drawing.Point(4, 182);
            this.photosCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.photosCheckBox.Name = "photosCheckBox";
            this.photosCheckBox.Size = new System.Drawing.Size(140, 24);
            this.photosCheckBox.TabIndex = 6;
            this.photosCheckBox.Text = "Aircraft Photos";
            this.photosCheckBox.UseVisualStyleBackColor = true;
            // 
            // mapCheckBox
            // 
            this.mapCheckBox.AutoSize = true;
            this.mapCheckBox.Location = new System.Drawing.Point(4, 216);
            this.mapCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mapCheckBox.Name = "mapCheckBox";
            this.mapCheckBox.Size = new System.Drawing.Size(169, 24);
            this.mapCheckBox.TabIndex = 7;
            this.mapCheckBox.Text = "Position Track Map";
            this.mapCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(224, 336);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(206, 35);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Ok";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // reportCheckBox
            // 
            this.reportCheckBox.AutoSize = true;
            this.reportCheckBox.Location = new System.Drawing.Point(4, 111);
            this.reportCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.reportCheckBox.Name = "reportCheckBox";
            this.reportCheckBox.Size = new System.Drawing.Size(117, 24);
            this.reportCheckBox.TabIndex = 9;
            this.reportCheckBox.Text = "Report Link";
            this.reportCheckBox.UseVisualStyleBackColor = true;
            // 
            // twitterCheckBox
            // 
            this.twitterCheckBox.AutoSize = true;
            this.twitterCheckBox.Location = new System.Drawing.Point(221, 19);
            this.twitterCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.twitterCheckBox.Name = "twitterCheckBox";
            this.twitterCheckBox.Size = new System.Drawing.Size(157, 24);
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
            this.checkBoxesPanel.Location = new System.Drawing.Point(18, 14);
            this.checkBoxesPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxesPanel.Name = "checkBoxesPanel";
            this.checkBoxesPanel.Size = new System.Drawing.Size(195, 357);
            this.checkBoxesPanel.TabIndex = 11;
            // 
            // kmlCheckbox
            // 
            this.kmlCheckbox.AutoSize = true;
            this.kmlCheckbox.Location = new System.Drawing.Point(4, 250);
            this.kmlCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.kmlCheckbox.Name = "kmlCheckbox";
            this.kmlCheckbox.Size = new System.Drawing.Size(96, 24);
            this.kmlCheckbox.TabIndex = 10;
            this.kmlCheckbox.Text = "KML File";
            this.kmlCheckbox.UseVisualStyleBackColor = true;
            // 
            // MapOptionsGroupBox
            // 
            this.MapOptionsGroupBox.Controls.Add(this.moLatLong);
            this.MapOptionsGroupBox.Controls.Add(this.moAircraft);
            this.MapOptionsGroupBox.Location = new System.Drawing.Point(221, 208);
            this.MapOptionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MapOptionsGroupBox.Name = "MapOptionsGroupBox";
            this.MapOptionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MapOptionsGroupBox.Size = new System.Drawing.Size(206, 114);
            this.MapOptionsGroupBox.TabIndex = 12;
            this.MapOptionsGroupBox.TabStop = false;
            this.MapOptionsGroupBox.Text = "Map Options";
            // 
            // moLatLong
            // 
            this.moLatLong.AutoSize = true;
            this.moLatLong.Checked = true;
            this.moLatLong.Location = new System.Drawing.Point(21, 69);
            this.moLatLong.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.moLatLong.Name = "moLatLong";
            this.moLatLong.Size = new System.Drawing.Size(159, 24);
            this.moLatLong.TabIndex = 4;
            this.moLatLong.TabStop = true;
            this.moLatLong.Text = "Centre on lat/long";
            this.moLatLong.UseVisualStyleBackColor = true;
            // 
            // moAircraft
            // 
            this.moAircraft.AutoSize = true;
            this.moAircraft.Location = new System.Drawing.Point(21, 34);
            this.moAircraft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.moAircraft.Name = "moAircraft";
            this.moAircraft.Size = new System.Drawing.Size(157, 24);
            this.moAircraft.TabIndex = 3;
            this.moAircraft.Text = "Centre on aircraft";
            this.moAircraft.UseVisualStyleBackColor = true;
            // 
            // Email_Content_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 387);
            this.Controls.Add(this.MapOptionsGroupBox);
            this.Controls.Add(this.checkBoxesPanel);
            this.Controls.Add(this.twitterCheckBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.propertyListGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Email_Content_Config";
            this.Text = "Email Content Config";
            this.Load += new System.EventHandler(this.Email_Content_Config_Load);
            this.propertyListGroupBox.ResumeLayout(false);
            this.propertyListGroupBox.PerformLayout();
            this.checkBoxesPanel.ResumeLayout(false);
            this.checkBoxesPanel.PerformLayout();
            this.MapOptionsGroupBox.ResumeLayout(false);
            this.MapOptionsGroupBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox MapOptionsGroupBox;
        private System.Windows.Forms.RadioButton moLatLong;
        private System.Windows.Forms.RadioButton moAircraft;
    }
}
