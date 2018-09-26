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
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// conditionNameTextBox
			// 
			this.conditionNameTextBox.Location = new System.Drawing.Point(103, 12);
			this.conditionNameTextBox.Name = "conditionNameTextBox";
			this.conditionNameTextBox.Size = new System.Drawing.Size(390, 20);
			this.conditionNameTextBox.TabIndex = 0;
			// 
			// conditionNameLabel
			// 
			this.conditionNameLabel.AutoSize = true;
			this.conditionNameLabel.Location = new System.Drawing.Point(12, 15);
			this.conditionNameLabel.Name = "conditionNameLabel";
			this.conditionNameLabel.Size = new System.Drawing.Size(85, 13);
			this.conditionNameLabel.TabIndex = 1;
			this.conditionNameLabel.Text = "Condition Name:";
			this.toolTip.SetToolTip(this.conditionNameLabel, "The name for the condition");
			// 
			// emailPropertyLabel
			// 
			this.emailPropertyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.emailPropertyLabel.AutoSize = true;
			this.emailPropertyLabel.Location = new System.Drawing.Point(12, 101);
			this.emailPropertyLabel.Name = "emailPropertyLabel";
			this.emailPropertyLabel.Size = new System.Drawing.Size(77, 13);
			this.emailPropertyLabel.TabIndex = 2;
			this.emailPropertyLabel.Text = "Email Property:";
			this.toolTip.SetToolTip(this.emailPropertyLabel, "The property that will show in the email subject");
			// 
			// emailPropertyComboBox
			// 
			this.emailPropertyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.emailPropertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.emailPropertyComboBox.FormattingEnabled = true;
			this.emailPropertyComboBox.Location = new System.Drawing.Point(103, 98);
			this.emailPropertyComboBox.Name = "emailPropertyComboBox";
			this.emailPropertyComboBox.Size = new System.Drawing.Size(390, 21);
			this.emailPropertyComboBox.TabIndex = 2;
			// 
			// triggerDataGridView
			// 
			this.triggerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.triggerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.triggerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyColumn,
            this.comparisonTypeColumn,
            this.valueColumn});
			this.triggerDataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.triggerDataGridView.Location = new System.Drawing.Point(0, 174);
			this.triggerDataGridView.MultiSelect = false;
			this.triggerDataGridView.Name = "triggerDataGridView";
			this.triggerDataGridView.RowHeadersVisible = false;
			this.triggerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.triggerDataGridView.Size = new System.Drawing.Size(505, 156);
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
			this.alertTypeLabel.Location = new System.Drawing.Point(12, 128);
			this.alertTypeLabel.Name = "alertTypeLabel";
			this.alertTypeLabel.Size = new System.Drawing.Size(58, 13);
			this.alertTypeLabel.TabIndex = 5;
			this.alertTypeLabel.Text = "Alert Type:";
			this.toolTip.SetToolTip(this.alertTypeLabel, "Disabled: Never alert\r\nFirst: Alert on first contact\r\nLast: Alert on last contact" +
        "\r\nBoth: Alert on first and last contact");
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.saveButton.Location = new System.Drawing.Point(373, 124);
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
			this.alertTypeComboBox.Location = new System.Drawing.Point(103, 124);
			this.alertTypeComboBox.Name = "alertTypeComboBox";
			this.alertTypeComboBox.Size = new System.Drawing.Size(140, 21);
			this.alertTypeComboBox.TabIndex = 3;
			// 
			// emailToSendToLabel
			// 
			this.emailToSendToLabel.Location = new System.Drawing.Point(12, 41);
			this.emailToSendToLabel.Name = "emailToSendToLabel";
			this.emailToSendToLabel.Size = new System.Drawing.Size(95, 51);
			this.emailToSendToLabel.TabIndex = 9;
			this.emailToSendToLabel.Text = "Emails to send to: (One each line)";
			this.toolTip.SetToolTip(this.emailToSendToLabel, "The emails that will receive alert messages (Separated by commas)");
			// 
			// recieverEmailTextBox
			// 
			this.recieverEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.recieverEmailTextBox.Location = new System.Drawing.Point(103, 38);
			this.recieverEmailTextBox.Multiline = true;
			this.recieverEmailTextBox.Name = "recieverEmailTextBox";
			this.recieverEmailTextBox.Size = new System.Drawing.Size(390, 54);
			this.recieverEmailTextBox.TabIndex = 1;
			// 
			// propertyInfoButton
			// 
			this.propertyInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.propertyInfoButton.Location = new System.Drawing.Point(248, 124);
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
			this.helpLabel.Location = new System.Drawing.Point(12, 153);
			this.helpLabel.Name = "helpLabel";
			this.helpLabel.Size = new System.Drawing.Size(190, 13);
			this.helpLabel.TabIndex = 11;
			this.helpLabel.Text = "Alert will only send if all triggers are true";
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
			this.ignoreFollowingCheckbox.Location = new System.Drawing.Point(342, 152);
			this.ignoreFollowingCheckbox.Name = "ignoreFollowingCheckbox";
			this.ignoreFollowingCheckbox.Size = new System.Drawing.Size(151, 17);
			this.ignoreFollowingCheckbox.TabIndex = 6;
			this.ignoreFollowingCheckbox.Text = "Ignore following conditions";
			this.ignoreFollowingCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ignoreFollowingCheckbox.UseVisualStyleBackColor = true;
			// 
			// Condition_Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(505, 330);
			this.Controls.Add(this.ignoreFollowingCheckbox);
			this.Controls.Add(this.recieverEmailTextBox);
			this.Controls.Add(this.helpLabel);
			this.Controls.Add(this.propertyInfoButton);
			this.Controls.Add(this.emailToSendToLabel);
			this.Controls.Add(this.alertTypeComboBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.alertTypeLabel);
			this.Controls.Add(this.triggerDataGridView);
			this.Controls.Add(this.emailPropertyComboBox);
			this.Controls.Add(this.emailPropertyLabel);
			this.Controls.Add(this.conditionNameLabel);
			this.Controls.Add(this.conditionNameTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(521, 10000);
			this.MinimumSize = new System.Drawing.Size(521, 369);
			this.Name = "Condition_Editor";
			this.Text = "PlaneAlerter Condition Editor";
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).EndInit();
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
	}
}