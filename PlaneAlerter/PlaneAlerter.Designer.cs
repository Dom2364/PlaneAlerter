namespace PlaneAlerter {
	partial class PlaneAlerter {
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Conditions");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Email");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Radar");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Program");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Settings", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0");
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node1");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node2");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Statistics", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaneAlerter));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.console = new System.Windows.Forms.RichTextBox();
			this.conditionTreeView = new System.Windows.Forms.TreeView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startConditionEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.emailContentConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.activeAlertsLabel = new System.Windows.Forms.Label();
			this.activeMatchesDataGridView = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.activeMatchesDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// console
			// 
			this.console.Dock = System.Windows.Forms.DockStyle.Fill;
			this.console.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.console.Location = new System.Drawing.Point(0, 0);
			this.console.Name = "console";
			this.console.ReadOnly = true;
			this.console.Size = new System.Drawing.Size(634, 368);
			this.console.TabIndex = 0;
			this.console.Text = "";
			this.console.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ConsoleLinkClicked);
			// 
			// conditionTreeView
			// 
			this.conditionTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conditionTreeView.Location = new System.Drawing.Point(0, 0);
			this.conditionTreeView.Name = "conditionTreeView";
			treeNode1.Name = "conditionsNode";
			treeNode1.Text = "Conditions";
			treeNode2.Name = "emailSettingsNode";
			treeNode2.Text = "Email";
			treeNode3.Name = "radarSettingsNode";
			treeNode3.Text = "Radar";
			treeNode4.Name = "programSettingsNode";
			treeNode4.Text = "Program";
			treeNode5.Name = "settingsNode";
			treeNode5.Text = "Settings";
			treeNode6.Name = "Node0";
			treeNode6.Text = "Node0";
			treeNode7.Name = "Node1";
			treeNode7.Text = "Node1";
			treeNode8.Name = "Node2";
			treeNode8.Text = "Node2";
			treeNode9.Name = "statsNode";
			treeNode9.Text = "Statistics";
			this.conditionTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5,
            treeNode9});
			this.conditionTreeView.Size = new System.Drawing.Size(260, 163);
			this.conditionTreeView.TabIndex = 1;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.startToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.clearConsoleToolStripMenuItem,
            this.propertyInfoToolStripMenuItem,
            this.donateToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(898, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startConditionEditorToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.twitterToolStripMenuItem,
            this.emailContentConfigToolStripMenuItem,
            this.openLogFileToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// startConditionEditorToolStripMenuItem
			// 
			this.startConditionEditorToolStripMenuItem.Name = "startConditionEditorToolStripMenuItem";
			this.startConditionEditorToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.startConditionEditorToolStripMenuItem.Text = "Open Condition Editor";
			this.startConditionEditorToolStripMenuItem.Click += new System.EventHandler(this.startConditionEditorToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.settingsToolStripMenuItem.Text = "Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// twitterToolStripMenuItem
			// 
			this.twitterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAccountToolStripMenuItem,
            this.removeAccountToolStripMenuItem});
			this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
			this.twitterToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.twitterToolStripMenuItem.Text = "Twitter";
			// 
			// addAccountToolStripMenuItem
			// 
			this.addAccountToolStripMenuItem.Name = "addAccountToolStripMenuItem";
			this.addAccountToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.addAccountToolStripMenuItem.Text = "Add Account";
			this.addAccountToolStripMenuItem.Click += new System.EventHandler(this.addAccountToolStripMenuItem_Click);
			// 
			// removeAccountToolStripMenuItem
			// 
			this.removeAccountToolStripMenuItem.Name = "removeAccountToolStripMenuItem";
			this.removeAccountToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.removeAccountToolStripMenuItem.Text = "Remove Account";
			// 
			// emailContentConfigToolStripMenuItem
			// 
			this.emailContentConfigToolStripMenuItem.Name = "emailContentConfigToolStripMenuItem";
			this.emailContentConfigToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.emailContentConfigToolStripMenuItem.Text = "Email Content Config";
			this.emailContentConfigToolStripMenuItem.Click += new System.EventHandler(this.emailContentConfigToolStripMenuItem_Click);
			// 
			// openLogFileToolStripMenuItem
			// 
			this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
			this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.openLogFileToolStripMenuItem.Text = "Open log file";
			this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.openLogFileToolStripMenuItem_Click);
			// 
			// startToolStripMenuItem
			// 
			this.startToolStripMenuItem.Name = "startToolStripMenuItem";
			this.startToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.startToolStripMenuItem.Text = "Start";
			this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
			// 
			// restartToolStripMenuItem
			// 
			this.restartToolStripMenuItem.Enabled = false;
			this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
			this.restartToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.restartToolStripMenuItem.Text = "Restart";
			this.restartToolStripMenuItem.Click += new System.EventHandler(this.RestartToolStripMenuItemClick);
			// 
			// clearConsoleToolStripMenuItem
			// 
			this.clearConsoleToolStripMenuItem.Name = "clearConsoleToolStripMenuItem";
			this.clearConsoleToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
			this.clearConsoleToolStripMenuItem.Text = "Clear Console";
			this.clearConsoleToolStripMenuItem.Click += new System.EventHandler(this.clearConsoleToolStripMenuItem_Click);
			// 
			// propertyInfoToolStripMenuItem
			// 
			this.propertyInfoToolStripMenuItem.Name = "propertyInfoToolStripMenuItem";
			this.propertyInfoToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
			this.propertyInfoToolStripMenuItem.Text = "Property Info";
			this.propertyInfoToolStripMenuItem.Click += new System.EventHandler(this.propertyInfoToolStripMenuItem_Click);
			// 
			// donateToolStripMenuItem
			// 
			this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
			this.donateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.donateToolStripMenuItem.Text = "Donate";
			this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 392);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(898, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(64, 17);
			this.statusLabel.Text = "Status: Idle";
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "PlaneAlerter";
			this.notifyIcon.Visible = true;
			this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.console);
			this.splitContainer1.Size = new System.Drawing.Size(898, 368);
			this.splitContainer1.SplitterDistance = 260;
			this.splitContainer1.TabIndex = 5;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.conditionTreeView);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.activeAlertsLabel);
			this.splitContainer2.Panel2.Controls.Add(this.activeMatchesDataGridView);
			this.splitContainer2.Size = new System.Drawing.Size(260, 368);
			this.splitContainer2.SplitterDistance = 163;
			this.splitContainer2.TabIndex = 0;
			// 
			// activeAlertsLabel
			// 
			this.activeAlertsLabel.AutoSize = true;
			this.activeAlertsLabel.Location = new System.Drawing.Point(3, 0);
			this.activeAlertsLabel.Name = "activeAlertsLabel";
			this.activeAlertsLabel.Size = new System.Drawing.Size(68, 13);
			this.activeAlertsLabel.TabIndex = 5;
			this.activeAlertsLabel.Text = "Active alerts:";
			// 
			// activeMatchesDataGridView
			// 
			this.activeMatchesDataGridView.AllowUserToAddRows = false;
			this.activeMatchesDataGridView.AllowUserToDeleteRows = false;
			this.activeMatchesDataGridView.AllowUserToResizeRows = false;
			this.activeMatchesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.activeMatchesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.activeMatchesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.activeMatchesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.activeMatchesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.activeMatchesDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
			this.activeMatchesDataGridView.Location = new System.Drawing.Point(0, 16);
			this.activeMatchesDataGridView.Name = "activeMatchesDataGridView";
			this.activeMatchesDataGridView.ReadOnly = true;
			this.activeMatchesDataGridView.RowHeadersVisible = false;
			this.activeMatchesDataGridView.Size = new System.Drawing.Size(260, 185);
			this.activeMatchesDataGridView.TabIndex = 1;
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.Column1.HeaderText = "ICAO";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.Width = 5;
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.Column2.HeaderText = "Reg";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Width = 5;
			// 
			// Column3
			// 
			this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.Column3.HeaderText = "Type";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.Width = 5;
			// 
			// Column4
			// 
			this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.Column4.HeaderText = "Callsign";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			this.Column4.Width = 5;
			// 
			// Column5
			// 
			this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column5.HeaderText = "Condition";
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			// 
			// PlaneAlerter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(898, 414);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "PlaneAlerter";
			this.Text = "PlaneAlerter";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlaneAlerterFormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.activeMatchesDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.RichTextBox console;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startConditionEditorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openLogFileToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripMenuItem clearConsoleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertyInfoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem emailContentConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
		public System.Windows.Forms.TreeView conditionTreeView;
		public System.Windows.Forms.ToolStripStatusLabel statusLabel;
		public System.Windows.Forms.NotifyIcon notifyIcon;
		public System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeAccountToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addAccountToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		public System.Windows.Forms.DataGridView activeMatchesDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		public System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
		public System.Windows.Forms.Label activeAlertsLabel;
	}
}
