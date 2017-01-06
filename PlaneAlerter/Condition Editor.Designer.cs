namespace PlaneAlerter {
	partial class ConditionEditor {
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Conditions");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionEditor));
			this.conditionEditorTreeView = new System.Windows.Forms.TreeView();
			this.addConditionButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.removeConditionButton = new System.Windows.Forms.Button();
			this.moveUpButton = new System.Windows.Forms.Button();
			this.moveDownButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// conditionEditorTreeView
			// 
			this.conditionEditorTreeView.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.conditionEditorTreeView.Location = new System.Drawing.Point(0, 43);
			this.conditionEditorTreeView.Name = "conditionEditorTreeView";
			treeNode1.Name = "conditionsNode";
			treeNode1.Text = "Conditions";
			this.conditionEditorTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
			this.conditionEditorTreeView.Size = new System.Drawing.Size(451, 249);
			this.conditionEditorTreeView.TabIndex = 0;
			this.conditionEditorTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ConditionTreeViewNodeMouseDoubleClick);
			// 
			// addConditionButton
			// 
			this.addConditionButton.Location = new System.Drawing.Point(13, 11);
			this.addConditionButton.Name = "addConditionButton";
			this.addConditionButton.Size = new System.Drawing.Size(82, 23);
			this.addConditionButton.TabIndex = 1;
			this.addConditionButton.Text = "Add Condition";
			this.addConditionButton.UseVisualStyleBackColor = true;
			this.addConditionButton.Click += new System.EventHandler(this.addConditionButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.Location = new System.Drawing.Point(353, 11);
			this.exitButton.Margin = new System.Windows.Forms.Padding(2);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(85, 23);
			this.exitButton.TabIndex = 2;
			this.exitButton.Text = "Save and Exit";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
			// 
			// removeConditionButton
			// 
			this.removeConditionButton.Location = new System.Drawing.Point(100, 11);
			this.removeConditionButton.Margin = new System.Windows.Forms.Padding(2);
			this.removeConditionButton.Name = "removeConditionButton";
			this.removeConditionButton.Size = new System.Drawing.Size(104, 23);
			this.removeConditionButton.TabIndex = 3;
			this.removeConditionButton.Text = "Remove Condition";
			this.removeConditionButton.UseVisualStyleBackColor = true;
			this.removeConditionButton.Click += new System.EventHandler(this.RemoveConditionButtonClick);
			// 
			// moveUpButton
			// 
			this.moveUpButton.Location = new System.Drawing.Point(208, 11);
			this.moveUpButton.Margin = new System.Windows.Forms.Padding(2);
			this.moveUpButton.Name = "moveUpButton";
			this.moveUpButton.Size = new System.Drawing.Size(60, 23);
			this.moveUpButton.TabIndex = 4;
			this.moveUpButton.Text = "Move Up";
			this.moveUpButton.UseVisualStyleBackColor = true;
			this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
			// 
			// moveDownButton
			// 
			this.moveDownButton.Location = new System.Drawing.Point(272, 11);
			this.moveDownButton.Margin = new System.Windows.Forms.Padding(2);
			this.moveDownButton.Name = "moveDownButton";
			this.moveDownButton.Size = new System.Drawing.Size(77, 23);
			this.moveDownButton.TabIndex = 5;
			this.moveDownButton.Text = "Move Down";
			this.moveDownButton.UseVisualStyleBackColor = true;
			this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(451, 292);
			this.Controls.Add(this.moveDownButton);
			this.Controls.Add(this.moveUpButton);
			this.Controls.Add(this.removeConditionButton);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.addConditionButton);
			this.Controls.Add(this.conditionEditorTreeView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "PlaneAlerter Condition Editor";
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button removeConditionButton;
		private System.Windows.Forms.Button exitButton;

		#endregion

		private System.Windows.Forms.TreeView conditionEditorTreeView;
		private System.Windows.Forms.Button addConditionButton;
		private System.Windows.Forms.Button moveUpButton;
		private System.Windows.Forms.Button moveDownButton;
	}
}

