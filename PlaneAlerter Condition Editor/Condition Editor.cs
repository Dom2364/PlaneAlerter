using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneAlerter_Condition_Editor {
	public partial class Condition_Editor :Form {
		public Condition_Editor(Condition conditionToUpdate) {
			initialise();
			conditionNameTextBox.Text = conditionToUpdate.conditionName;
			emailPropertyComboBox.Text = conditionToUpdate.emailProperty.ToString();
			foreach (object[] trigger in conditionToUpdate.triggers.Values) {
				triggerDataGridView.Rows.Add();
				DataGridViewRow newRow = triggerDataGridView.Rows[triggerDataGridView.Rows.Count - 2];
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(newRow.Cells[0]);
				foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty))) {
					comboBoxCell.Items.Add(property.ToString());
				}
				newRow.Cells[0].Value = trigger[0].ToString();
				newRow.Cells[1].Value = trigger[1];
				newRow.Cells[2].Value = trigger[2];
			}
		}
		
		public Condition_Editor() {
			initialise();
		}
		
		public void initialise() {
			InitializeComponent();
			
			foreach (DataGridViewRow row in triggerDataGridView.Rows) {
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
				foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty))) {
					comboBoxCell.Items.Add(property.ToString());
				}
			}
			
			foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty))) {
				emailPropertyComboBox.Items.Add(property.ToString());
			}
		}
		
		private void triggerDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			
			//Check if value fits the format of the property
			if (e.ColumnIndex == 2 && triggerDataGridView.Rows.Count != 1) {
				//If there is no property selected, clear the textbox
				if (triggerDataGridView.Rows[e.RowIndex].Cells[0].Value != null && triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() != "") {
					if (triggerDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() != "" && Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString())][0] == "Number") {
						try {
							Convert.ToInt32(triggerDataGridView.Rows[e.RowIndex].Cells[2].Value);
						}
						catch(Exception) {
							triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
						}

					}
					//TODO CHANGE TO PROPER NAMES
					if (triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() == "Sqk" && triggerDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString().Length != 4) {
						triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
					}
				}
			}
			if (e.ColumnIndex == 0 && triggerDataGridView.Rows.Count != 1) {
				DataGridViewComboBoxCell comparisonTypeComboBox = (DataGridViewComboBoxCell)(triggerDataGridView.Rows[e.RowIndex].Cells[1]);
				comparisonTypeComboBox.Items.Clear();
				comparisonTypeComboBox.Value = "";
				string supportedComparisonTypes = "";
				try {
					supportedComparisonTypes = Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString())][1];
				}
				catch (Exception) {
					return;
				}
				if (supportedComparisonTypes.Contains("A")) {
					foreach (string comparisonType in Core.comparisonTypes["A"]) {
						comparisonTypeComboBox.Items.Add(comparisonType);
					}
				}
				if(supportedComparisonTypes.Contains("B")) {
					foreach(string comparisonType in Core.comparisonTypes["B"]) {
						comparisonTypeComboBox.Items.Add(comparisonType);
					}
				}
				if(supportedComparisonTypes.Contains("C")) {
					foreach(string comparisonType in Core.comparisonTypes["C"]) {
						comparisonTypeComboBox.Items.Add(comparisonType);
					}
					triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "True";
					triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
				}
				else {
					triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
					triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = false;
				}
				if(supportedComparisonTypes.Contains("D")) {
					foreach(string comparisonType in Core.comparisonTypes["D"]) {
						comparisonTypeComboBox.Items.Add(comparisonType);
					}
				}
				if(supportedComparisonTypes.Contains("E")) {
					foreach(string comparisonType in Core.comparisonTypes["E"]) {
						comparisonTypeComboBox.Items.Add(comparisonType);
					}
				}
			}
		}
		
		void TriggerDataGridViewUserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			foreach (DataGridViewRow row in triggerDataGridView.Rows) {
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
				comboBoxCell.Items.Clear();
				foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty))) {
					comboBoxCell.Items.Add(property.ToString());
				}
			}
		}
		
		void SaveButtonClick(object sender, EventArgs e)
		{
			//TODO CHECK IF EVERYTHING IS FILLED OUT
			Condition newCondition = new Condition();
			newCondition.conditionName = conditionNameTextBox.Text;
			newCondition.emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), emailPropertyComboBox.Text);
			newCondition.id = Core.conditions.Count;
			if (triggerDataGridView.Rows.Count != 0) {
				foreach (DataGridViewRow row in triggerDataGridView.Rows) {
					if (row.Index != triggerDataGridView.Rows.Count - 1) {
						newCondition.triggers.Add(newCondition.triggers.Count, new object[] {(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), row.Cells[0].Value.ToString()), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString()});
					}
				}
			}
			Core.conditions.Add(Core.conditions.Count, newCondition);
			this.Close();
		}
	}
}
