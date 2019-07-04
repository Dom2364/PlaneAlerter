namespace PlaneAlerter {
	partial class Condition_Editor {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Condition_Editor));
			this.conditionNameTextBox = new System.Windows.Forms.TextBox();
			this.conditionNameLabel = new System.Windows.Forms.Label();
			this.emailPropertyLabel = new System.Windows.Forms.Label();
			this.emailPropertyComboBox = new System.Windows.Forms.ComboBox();
			this.triggerDataGridView = new System.Windows.Forms.DataGridView();
			this.propertyColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.comparisonTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.alertTypeLabel = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.alertTypeComboBox = new System.Windows.Forms.ComboBox();
			this.emailToSendToLabel = new System.Windows.Forms.Label();
			this.recieverEmailTextBox = new System.Windows.Forms.TextBox();
			this.propertyInfoButton = new System.Windows.Forms.Button();
			this.helpLabel = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ignoreFollowingCheckbox = new System.Windows.Forms.CheckBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.triggersTabPage = new System.Windows.Forms.TabPage();
			this.emailTabPage = new System.Windows.Forms.TabPage();
			this.emailCheckBox = new System.Windows.Forms.CheckBox();
			this.twitterTabPage = new System.Windows.Forms.TabPage();
			this.twitterLinkLabel = new System.Windows.Forms.Label();
			this.tweetLinkComboBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.twitterContentTextBox = new System.Windows.Forms.RichTextBox();
			this.tweetContentLabel = new System.Windows.Forms.Label();
			this.twitterAccountComboBox = new System.Windows.Forms.ComboBox();
			this.twitterAccountLabel = new System.Windows.Forms.Label();
			this.twitterCheckBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).BeginInit();
			this.tabControl.SuspendLayout();
			this.triggersTabPage.SuspendLayout();
			this.emailTabPage.SuspendLayout();
			this.twitterTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// conditionNameTextBox
			// 
			this.conditionNameTextBox.Location = new System.Drawing.Point(99, 12);
			this.conditionNameTextBox.Name = "conditionNameTextBox";
			this.conditionNameTextBox.Size = new System.Drawing.Size(394, 20);
			this.conditionNameTextBox.TabIndex = 0;
			// 
			// conditionNameLabel
			// 
			this.conditionNameLabel.AutoSize = true;
			this.conditionNameLabel.Location = new System.Drawing.Point(8, 15);
			this.conditionNameLabel.Name = "conditionNameLabel";
			this.conditionNameLabel.Size = new System.Drawing.Size(85, 13);
			this.conditionNameLabel.TabIndex = 1;
			this.conditionNameLabel.Text = "Condition Name:";
			this.toolTip.SetToolTip(this.conditionNameLabel, "The name for the condition");
			// 
			// emailPropertyLabel
			// 
			this.emailPropertyLabel.AutoSize = true;
			this.emailPropertyLabel.Location = new System.Drawing.Point(196, 35);
			this.emailPropertyLabel.Name = "emailPropertyLabel";
			this.emailPropertyLabel.Size = new System.Drawing.Size(137, 13);
			this.emailPropertyLabel.TabIndex = 2;
			this.emailPropertyLabel.Text = "Property to show in subject:";
			this.toolTip.SetToolTip(this.emailPropertyLabel, "The property that will show in the email subject");
			// 
			// emailPropertyComboBox
			// 
			this.emailPropertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.emailPropertyComboBox.FormattingEnabled = true;
			this.emailPropertyComboBox.Location = new System.Drawing.Point(338, 32);
			this.emailPropertyComboBox.Name = "emailPropertyComboBox";
			this.emailPropertyComboBox.Size = new System.Drawing.Size(152, 21);
			this.emailPropertyComboBox.TabIndex = 2;
			// 
			// triggerDataGridView
			// 
			this.triggerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.triggerDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.triggerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.triggerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyColumn,
            this.comparisonTypeColumn,
            this.valueColumn});
			this.triggerDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.triggerDataGridView.Location = new System.Drawing.Point(3, 3);
			this.triggerDataGridView.MultiSelect = false;
			this.triggerDataGridView.Name = "triggerDataGridView";
			this.triggerDataGridView.RowHeadersVisible = false;
			this.triggerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.triggerDataGridView.Size = new System.Drawing.Size(491, 194);
			this.triggerDataGridView.TabIndex = 7;
			this.triggerDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.triggerDataGridView_CellValueChanged);
			this.triggerDataGridView.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.TriggerDataGridViewUserAddedRow);
			// 
			// propertyColumn
			// 
			this.propertyColumn.HeaderText = "Property";
			this.propertyColumn.Name = "propertyColumn";
			// 
			// comparisonTypeColumn
			// 
			this.comparisonTypeColumn.HeaderText = "Comparison Type";
			this.comparisonTypeColumn.Name = "comparisonTypeColumn";
			this.comparisonTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.comparisonTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// valueColumn
			// 
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			// 
			// alertTypeLabel
			// 
			this.alertTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.alertTypeLabel.AutoSize = true;
			this.alertTypeLabel.Location = new System.Drawing.Point(39, 41);
			this.alertTypeLabel.Name = "alertTypeLabel";
			this.alertTypeLabel.Size = new System.Drawing.Size(58, 13);
			this.alertTypeLabel.TabIndex = 5;
			this.alertTypeLabel.Text = "Alert Type:";
			this.toolTip.SetToolTip(this.alertTypeLabel, "Disabled: Never alert\r\nFirst: Alert on first contact\r\nLast: Alert on last contact" +
        "\r\nBoth: Alert on first and last contact");
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(373, 298);
			this.saveButton.Margin = new System.Windows.Forms.Padding(2);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(121, 21);
			this.saveButton.TabIndex = 5;
			this.saveButton.Text = "Save Condition";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			// 
			// alertTypeComboBox
			// 
			this.alertTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.alertTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.alertTypeComboBox.FormattingEnabled = true;
			this.alertTypeComboBox.Location = new System.Drawing.Point(99, 37);
			this.alertTypeComboBox.Name = "alertTypeComboBox";
			this.alertTypeComboBox.Size = new System.Drawing.Size(210, 21);
			this.alertTypeComboBox.TabIndex = 3;
			// 
			// emailToSendToLabel
			// 
			this.emailToSendToLabel.Location = new System.Drawing.Point(4, 35);
			this.emailToSendToLabel.Name = "emailToSendToLabel";
			this.emailToSendToLabel.Size = new System.Drawing.Size(179, 16);
			this.emailToSendToLabel.TabIndex = 9;
			this.emailToSendToLabel.Text = "Emails to send to (One each line): ";
			this.toolTip.SetToolTip(this.emailToSendToLabel, "The emails that will receive alert messages (Separated by commas)");
			// 
			// recieverEmailTextBox
			// 
			this.recieverEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.recieverEmailTextBox.Location = new System.Drawing.Point(6, 54);
			this.recieverEmailTextBox.Multiline = true;
			this.recieverEmailTextBox.Name = "recieverEmailTextBox";
			this.recieverEmailTextBox.Size = new System.Drawing.Size(484, 140);
			this.recieverEmailTextBox.TabIndex = 1;
			// 
			// propertyInfoButton
			// 
			this.propertyInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyInfoButton.Location = new System.Drawing.Point(248, 298);
			this.propertyInfoButton.Margin = new System.Windows.Forms.Padding(2);
			this.propertyInfoButton.Name = "propertyInfoButton";
			this.propertyInfoButton.Size = new System.Drawing.Size(121, 21);
			this.propertyInfoButton.TabIndex = 4;
			this.propertyInfoButton.Text = "Property Info";
			this.propertyInfoButton.UseVisualStyleBackColor = true;
			this.propertyInfoButton.Click += new System.EventHandler(this.propertyInfoButton_Click);
			// 
			// helpLabel
			// 
			this.helpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.helpLabel.AutoSize = true;
			this.helpLabel.Location = new System.Drawing.Point(8, 302);
			this.helpLabel.Name = "helpLabel";
			this.helpLabel.Size = new System.Drawing.Size(183, 13);
			this.helpLabel.TabIndex = 11;
			this.helpLabel.Text = "Alert will only send if all triggers match";
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 100;
			this.toolTip.UseAnimation = false;
			this.toolTip.UseFading = false;
			// 
			// ignoreFollowingCheckbox
			// 
			this.ignoreFollowingCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ignoreFollowingCheckbox.AutoSize = true;
			this.ignoreFollowingCheckbox.Location = new System.Drawing.Point(342, 41);
			this.ignoreFollowingCheckbox.Name = "ignoreFollowingCheckbox";
			this.ignoreFollowingCheckbox.Size = new System.Drawing.Size(151, 17);
			this.ignoreFollowingCheckbox.TabIndex = 6;
			this.ignoreFollowingCheckbox.Text = "Ignore following conditions";
			this.ignoreFollowingCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ignoreFollowingCheckbox.UseVisualStyleBackColor = true;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.triggersTabPage);
			this.tabControl.Controls.Add(this.emailTabPage);
			this.tabControl.Controls.Add(this.twitterTabPage);
			this.tabControl.Location = new System.Drawing.Point(0, 67);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(505, 226);
			this.tabControl.TabIndex = 12;
			// 
			// triggersTabPage
			// 
			this.triggersTabPage.Controls.Add(this.triggerDataGridView);
			this.triggersTabPage.Location = new System.Drawing.Point(4, 22);
			this.triggersTabPage.Name = "triggersTabPage";
			this.triggersTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.triggersTabPage.Size = new System.Drawing.Size(497, 200);
			this.triggersTabPage.TabIndex = 0;
			this.triggersTabPage.Text = "Triggers";
			this.triggersTabPage.UseVisualStyleBackColor = true;
			// 
			// emailTabPage
			// 
			this.emailTabPage.Controls.Add(this.emailCheckBox);
			this.emailTabPage.Controls.Add(this.recieverEmailTextBox);
			this.emailTabPage.Controls.Add(this.emailToSendToLabel);
			this.emailTabPage.Controls.Add(this.emailPropertyComboBox);
			this.emailTabPage.Controls.Add(this.emailPropertyLabel);
			this.emailTabPage.Location = new System.Drawing.Point(4, 22);
			this.emailTabPage.Name = "emailTabPage";
			this.emailTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.emailTabPage.Size = new System.Drawing.Size(497, 200);
			this.emailTabPage.TabIndex = 1;
			this.emailTabPage.Text = "Email";
			this.emailTabPage.UseVisualStyleBackColor = true;
			// 
			// emailCheckBox
			// 
			this.emailCheckBox.AutoSize = true;
			this.emailCheckBox.Location = new System.Drawing.Point(7, 8);
			this.emailCheckBox.Name = "emailCheckBox";
			this.emailCheckBox.Size = new System.Drawing.Size(122, 17);
			this.emailCheckBox.TabIndex = 10;
			this.emailCheckBox.Text = "Email Alerts Enabled";
			this.emailCheckBox.UseVisualStyleBackColor = true;
			this.emailCheckBox.CheckedChanged += new System.EventHandler(this.emailCheckBox_CheckedChanged);
			// 
			// twitterTabPage
			// 
			this.twitterTabPage.Controls.Add(this.twitterLinkLabel);
			this.twitterTabPage.Controls.Add(this.tweetLinkComboBox);
			this.twitterTabPage.Controls.Add(this.label3);
			this.twitterTabPage.Controls.Add(this.twitterContentTextBox);
			this.twitterTabPage.Controls.Add(this.tweetContentLabel);
			this.twitterTabPage.Controls.Add(this.twitterAccountComboBox);
			this.twitterTabPage.Controls.Add(this.twitterAccountLabel);
			this.twitterTabPage.Controls.Add(this.twitterCheckBox);
			this.twitterTabPage.Location = new System.Drawing.Point(4, 22);
			this.twitterTabPage.Name = "twitterTabPage";
			this.twitterTabPage.Size = new System.Drawing.Size(497, 200);
			this.twitterTabPage.TabIndex = 2;
			this.twitterTabPage.Text = "Twitter";
			this.twitterTabPage.UseVisualStyleBackColor = true;
			// 
			// twitterLinkLabel
			// 
			this.twitterLinkLabel.AutoSize = true;
			this.twitterLinkLabel.Location = new System.Drawing.Point(255, 31);
			this.twitterLinkLabel.Name = "twitterLinkLabel";
			this.twitterLinkLabel.Size = new System.Drawing.Size(76, 13);
			this.twitterLinkLabel.TabIndex = 20;
			this.twitterLinkLabel.Text = "Attached Link:";
			// 
			// tweetLinkComboBox
			// 
			this.tweetLinkComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tweetLinkComboBox.FormattingEnabled = true;
			this.tweetLinkComboBox.Location = new System.Drawing.Point(337, 28);
			this.tweetLinkComboBox.Name = "tweetLinkComboBox";
			this.tweetLinkComboBox.Size = new System.Drawing.Size(152, 21);
			this.tweetLinkComboBox.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 157);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(483, 43);
			this.label3.TabIndex = 18;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// twitterContentTextBox
			// 
			this.twitterContentTextBox.Location = new System.Drawing.Point(6, 77);
			this.twitterContentTextBox.Name = "twitterContentTextBox";
			this.twitterContentTextBox.Size = new System.Drawing.Size(484, 76);
			this.twitterContentTextBox.TabIndex = 17;
			this.twitterContentTextBox.Text = "";
			// 
			// tweetContentLabel
			// 
			this.tweetContentLabel.AutoSize = true;
			this.tweetContentLabel.Location = new System.Drawing.Point(4, 60);
			this.tweetContentLabel.Name = "tweetContentLabel";
			this.tweetContentLabel.Size = new System.Drawing.Size(80, 13);
			this.tweetContentLabel.TabIndex = 16;
			this.tweetContentLabel.Text = "Tweet Content:";
			// 
			// twitterAccountComboBox
			// 
			this.twitterAccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.twitterAccountComboBox.FormattingEnabled = true;
			this.twitterAccountComboBox.Location = new System.Drawing.Point(337, 55);
			this.twitterAccountComboBox.Name = "twitterAccountComboBox";
			this.twitterAccountComboBox.Size = new System.Drawing.Size(152, 21);
			this.twitterAccountComboBox.TabIndex = 14;
			// 
			// twitterAccountLabel
			// 
			this.twitterAccountLabel.AutoSize = true;
			this.twitterAccountLabel.Location = new System.Drawing.Point(281, 58);
			this.twitterAccountLabel.Name = "twitterAccountLabel";
			this.twitterAccountLabel.Size = new System.Drawing.Size(50, 13);
			this.twitterAccountLabel.TabIndex = 13;
			this.twitterAccountLabel.Text = "Account:";
			// 
			// twitterCheckBox
			// 
			this.twitterCheckBox.AutoSize = true;
			this.twitterCheckBox.Location = new System.Drawing.Point(7, 8);
			this.twitterCheckBox.Name = "twitterCheckBox";
			this.twitterCheckBox.Size = new System.Drawing.Size(129, 17);
			this.twitterCheckBox.TabIndex = 11;
			this.twitterCheckBox.Text = "Twitter Alerts Enabled";
			this.twitterCheckBox.UseVisualStyleBackColor = true;
			this.twitterCheckBox.CheckedChanged += new System.EventHandler(this.twitterCheckBox_CheckedChanged);
			// 
			// Condition_Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(505, 330);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.ignoreFollowingCheckbox);
			this.Controls.Add(this.helpLabel);
			this.Controls.Add(this.propertyInfoButton);
			this.Controls.Add(this.alertTypeComboBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.alertTypeLabel);
			this.Controls.Add(this.conditionNameLabel);
			this.Controls.Add(this.conditionNameTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(521, 10000);
			this.MinimumSize = new System.Drawing.Size(521, 369);
			this.Name = "Condition_Editor";
			this.Text = "PlaneAlerter Condition Editor";
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.triggersTabPage.ResumeLayout(false);
			this.emailTabPage.ResumeLayout(false);
			this.emailTabPage.PerformLayout();
			this.twitterTabPage.ResumeLayout(false);
			this.twitterTabPage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button saveButton;

		#endregion

		private System.Windows.Forms.TextBox conditionNameTextBox;
		private System.Windows.Forms.Label conditionNameLabel;
		private System.Windows.Forms.Label emailPropertyLabel;
		private System.Windows.Forms.ComboBox emailPropertyComboBox;
		private System.Windows.Forms.DataGridView triggerDataGridView;
		private System.Windows.Forms.Label alertTypeLabel;
		private System.Windows.Forms.DataGridViewComboBoxColumn propertyColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn comparisonTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
		private System.Windows.Forms.ComboBox alertTypeComboBox;
		private System.Windows.Forms.Label emailToSendToLabel;
		private System.Windows.Forms.TextBox recieverEmailTextBox;
		private System.Windows.Forms.Button propertyInfoButton;
		private System.Windows.Forms.Label helpLabel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox ignoreFollowingCheckbox;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage triggersTabPage;
		private System.Windows.Forms.TabPage emailTabPage;
		private System.Windows.Forms.TabPage twitterTabPage;
		private System.Windows.Forms.CheckBox emailCheckBox;
		private System.Windows.Forms.CheckBox twitterCheckBox;
		private System.Windows.Forms.RichTextBox twitterContentTextBox;
		private System.Windows.Forms.Label tweetContentLabel;
		private System.Windows.Forms.ComboBox twitterAccountComboBox;
		private System.Windows.Forms.Label twitterAccountLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label twitterLinkLabel;
		private System.Windows.Forms.ComboBox tweetLinkComboBox;
	}
}