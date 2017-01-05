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
			this.propertyListGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// receiverNameCheckBox
			// 
			this.receiverNameCheckBox.AutoSize = true;
			this.receiverNameCheckBox.Location = new System.Drawing.Point(12, 12);
			this.receiverNameCheckBox.Name = "receiverNameCheckBox";
			this.receiverNameCheckBox.Size = new System.Drawing.Size(100, 17);
			this.receiverNameCheckBox.TabIndex = 0;
			this.receiverNameCheckBox.Text = "Receiver Name";
			this.receiverNameCheckBox.UseVisualStyleBackColor = true;
			// 
			// transponderTypeCheckBox
			// 
			this.transponderTypeCheckBox.AutoSize = true;
			this.transponderTypeCheckBox.Location = new System.Drawing.Point(12, 35);
			this.transponderTypeCheckBox.Name = "transponderTypeCheckBox";
			this.transponderTypeCheckBox.Size = new System.Drawing.Size(110, 17);
			this.transponderTypeCheckBox.TabIndex = 1;
			this.transponderTypeCheckBox.Text = "Tansponder Type";
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
			this.propertyListGroupBox.Location = new System.Drawing.Point(139, 12);
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
			this.radarLinkCheckBox.Location = new System.Drawing.Point(12, 58);
			this.radarLinkCheckBox.Name = "radarLinkCheckBox";
			this.radarLinkCheckBox.Size = new System.Drawing.Size(78, 17);
			this.radarLinkCheckBox.TabIndex = 4;
			this.radarLinkCheckBox.Text = "Radar Link";
			this.radarLinkCheckBox.UseVisualStyleBackColor = true;
			// 
			// afLookupCheckBox
			// 
			this.afLookupCheckBox.AutoSize = true;
			this.afLookupCheckBox.Location = new System.Drawing.Point(12, 104);
			this.afLookupCheckBox.Name = "afLookupCheckBox";
			this.afLookupCheckBox.Size = new System.Drawing.Size(125, 17);
			this.afLookupCheckBox.TabIndex = 5;
			this.afLookupCheckBox.Text = "airframes.org Lookup";
			this.afLookupCheckBox.UseVisualStyleBackColor = true;
			// 
			// photosCheckBox
			// 
			this.photosCheckBox.AutoSize = true;
			this.photosCheckBox.Location = new System.Drawing.Point(12, 127);
			this.photosCheckBox.Name = "photosCheckBox";
			this.photosCheckBox.Size = new System.Drawing.Size(95, 17);
			this.photosCheckBox.TabIndex = 6;
			this.photosCheckBox.Text = "Aircraft Photos";
			this.photosCheckBox.UseVisualStyleBackColor = true;
			// 
			// mapCheckBox
			// 
			this.mapCheckBox.AutoSize = true;
			this.mapCheckBox.Location = new System.Drawing.Point(12, 150);
			this.mapCheckBox.Name = "mapCheckBox";
			this.mapCheckBox.Size = new System.Drawing.Size(118, 17);
			this.mapCheckBox.TabIndex = 7;
			this.mapCheckBox.Text = "Position Track Map";
			this.mapCheckBox.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(139, 144);
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
			this.reportCheckBox.Location = new System.Drawing.Point(12, 81);
			this.reportCheckBox.Name = "reportCheckBox";
			this.reportCheckBox.Size = new System.Drawing.Size(81, 17);
			this.reportCheckBox.TabIndex = 9;
			this.reportCheckBox.Text = "Report Link";
			this.reportCheckBox.UseVisualStyleBackColor = true;
			// 
			// Email_Content_Config
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(288, 182);
			this.Controls.Add(this.reportCheckBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.mapCheckBox);
			this.Controls.Add(this.photosCheckBox);
			this.Controls.Add(this.afLookupCheckBox);
			this.Controls.Add(this.radarLinkCheckBox);
			this.Controls.Add(this.propertyListGroupBox);
			this.Controls.Add(this.transponderTypeCheckBox);
			this.Controls.Add(this.receiverNameCheckBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Email_Content_Config";
			this.Text = "Email Content Config";
			this.propertyListGroupBox.ResumeLayout(false);
			this.propertyListGroupBox.PerformLayout();
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
	}
}