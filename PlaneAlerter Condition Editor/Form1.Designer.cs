namespace PlaneAlerter_Condition_Editor {
	partial class Form1 {
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
			this.conditionTreeView = new System.Windows.Forms.TreeView();
			this.addConditionButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.removeConditionButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// conditionTreeView
			// 
			this.conditionTreeView.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.conditionTreeView.Location = new System.Drawing.Point(0, 51);
			this.conditionTreeView.Margin = new System.Windows.Forms.Padding(4);
			this.conditionTreeView.Name = "conditionTreeView";
			treeNode1.Name = "conditionsNode";
			treeNode1.Text = "Conditions";
			this.conditionTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
									treeNode1});
			this.conditionTreeView.Size = new System.Drawing.Size(525, 306);
			this.conditionTreeView.TabIndex = 0;
			this.conditionTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ConditionTreeViewNodeMouseDoubleClick);
			// 
			// addConditionButton
			// 
			this.addConditionButton.Location = new System.Drawing.Point(16, 15);
			this.addConditionButton.Margin = new System.Windows.Forms.Padding(4);
			this.addConditionButton.Name = "addConditionButton";
			this.addConditionButton.Size = new System.Drawing.Size(202, 28);
			this.addConditionButton.TabIndex = 1;
			this.addConditionButton.Text = "Add Condition";
			this.addConditionButton.UseVisualStyleBackColor = true;
			this.addConditionButton.Click += new System.EventHandler(this.addConditionButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.Location = new System.Drawing.Point(363, 15);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(150, 28);
			this.exitButton.TabIndex = 2;
			this.exitButton.Text = "Save and Exit";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
			// 
			// removeConditionButton
			// 
			this.removeConditionButton.Location = new System.Drawing.Point(225, 15);
			this.removeConditionButton.Name = "removeConditionButton";
			this.removeConditionButton.Size = new System.Drawing.Size(132, 28);
			this.removeConditionButton.TabIndex = 3;
			this.removeConditionButton.Text = "Remove Condition";
			this.removeConditionButton.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(525, 357);
			this.Controls.Add(this.removeConditionButton);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.addConditionButton);
			this.Controls.Add(this.conditionTreeView);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "PlaneAlerter Condition Editor";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button removeConditionButton;
		private System.Windows.Forms.Button exitButton;

		#endregion

		private System.Windows.Forms.TreeView conditionTreeView;
		private System.Windows.Forms.Button addConditionButton;
	}
}

