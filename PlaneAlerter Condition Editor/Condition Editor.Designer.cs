namespace PlaneAlerter_Condition_Editor {
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
			this.conditionNameTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.emailPropertyComboBox = new System.Windows.Forms.ComboBox();
			this.triggerDataGridView = new System.Windows.Forms.DataGridView();
			this.propertyColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.comparisonTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label3 = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// conditionNameTextBox
			// 
			this.conditionNameTextBox.Location = new System.Drawing.Point(137, 15);
			this.conditionNameTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.conditionNameTextBox.Name = "conditionNameTextBox";
			this.conditionNameTextBox.Size = new System.Drawing.Size(365, 22);
			this.conditionNameTextBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 18);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Condition Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 50);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Email Property:";
			// 
			// emailPropertyComboBox
			// 
			this.emailPropertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.emailPropertyComboBox.FormattingEnabled = true;
			this.emailPropertyComboBox.Location = new System.Drawing.Point(137, 47);
			this.emailPropertyComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.emailPropertyComboBox.Name = "emailPropertyComboBox";
			this.emailPropertyComboBox.Size = new System.Drawing.Size(365, 24);
			this.emailPropertyComboBox.TabIndex = 3;
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
			this.triggerDataGridView.Location = new System.Drawing.Point(0, 118);
			this.triggerDataGridView.Margin = new System.Windows.Forms.Padding(4);
			this.triggerDataGridView.MultiSelect = false;
			this.triggerDataGridView.Name = "triggerDataGridView";
			this.triggerDataGridView.RowHeadersVisible = false;
			this.triggerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.triggerDataGridView.Size = new System.Drawing.Size(520, 220);
			this.triggerDataGridView.TabIndex = 4;
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
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 84);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 17);
			this.label3.TabIndex = 5;
			this.label3.Text = "Triggers:";
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(137, 78);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(365, 33);
			this.saveButton.TabIndex = 6;
			this.saveButton.Text = "Save Condition";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			// 
			// Condition_Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(520, 338);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.triggerDataGridView);
			this.Controls.Add(this.emailPropertyComboBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.conditionNameTextBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Condition_Editor";
			this.Text = "Condition Editor";
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button saveButton;

		#endregion

		private System.Windows.Forms.TextBox conditionNameTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox emailPropertyComboBox;
		private System.Windows.Forms.DataGridView triggerDataGridView;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridViewComboBoxColumn propertyColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn comparisonTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
	}
}