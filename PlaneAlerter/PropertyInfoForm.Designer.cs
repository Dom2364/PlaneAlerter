namespace PlaneAlerter {
	partial class PropertyInfoForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyInfoForm));
			this.propertyDataGridView = new System.Windows.Forms.DataGridView();
			this.propertyNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.propertyValueTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.propertyInternalNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.propertyInfoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.propertyDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// propertyDataGridView
			// 
			this.propertyDataGridView.AllowUserToAddRows = false;
			this.propertyDataGridView.AllowUserToDeleteRows = false;
			this.propertyDataGridView.AllowUserToOrderColumns = true;
			this.propertyDataGridView.AllowUserToResizeRows = false;
			this.propertyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.propertyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.propertyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyNameColumn,
            this.propertyValueTypeColumn,
            this.propertyInternalNameColumn,
            this.propertyInfoColumn});
			this.propertyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyDataGridView.Location = new System.Drawing.Point(0, 0);
			this.propertyDataGridView.Name = "propertyDataGridView";
			this.propertyDataGridView.ReadOnly = true;
			this.propertyDataGridView.RowHeadersVisible = false;
			this.propertyDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.propertyDataGridView.Size = new System.Drawing.Size(919, 474);
			this.propertyDataGridView.TabIndex = 0;
			// 
			// propertyNameColumn
			// 
			this.propertyNameColumn.HeaderText = "Name";
			this.propertyNameColumn.Name = "propertyNameColumn";
			this.propertyNameColumn.ReadOnly = true;
			// 
			// propertyValueTypeColumn
			// 
			this.propertyValueTypeColumn.HeaderText = "Value Type";
			this.propertyValueTypeColumn.Name = "propertyValueTypeColumn";
			this.propertyValueTypeColumn.ReadOnly = true;
			// 
			// propertyInternalNameColumn
			// 
			this.propertyInternalNameColumn.HeaderText = "Internal Name";
			this.propertyInternalNameColumn.Name = "propertyInternalNameColumn";
			this.propertyInternalNameColumn.ReadOnly = true;
			// 
			// propertyInfoColumn
			// 
			this.propertyInfoColumn.FillWeight = 500F;
			this.propertyInfoColumn.HeaderText = "Detailed Info";
			this.propertyInfoColumn.Name = "propertyInfoColumn";
			this.propertyInfoColumn.ReadOnly = true;
			// 
			// PropertyInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(919, 474);
			this.Controls.Add(this.propertyDataGridView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PropertyInfoForm";
			this.Text = "Property Info";
			((System.ComponentModel.ISupportInitialize)(this.propertyDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView propertyDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn propertyNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn propertyValueTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn propertyInternalNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn propertyInfoColumn;
	}
}