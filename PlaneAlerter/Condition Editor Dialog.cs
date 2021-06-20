using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PlaneAlerter {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	public partial class Condition_Editor :Form {
		/// <summary>
		/// Id of condition to update
		/// </summary>
		int conditionToUpdate;

		/// <summary>
		/// Is this updating the condition?
		/// </summary>
		bool isUpdating = false;

		/// <summary>
		/// Initialise form with a condition to update
		/// </summary>
		/// <param name="_conditionToUpdate"></param>
		public Condition_Editor(int _conditionToUpdate) {
			isUpdating = true;
			conditionToUpdate = _conditionToUpdate;
			//Initialise form options
			Initialise();
			//Set form element values from condition info
			Core.Condition c = EditorConditionsList.conditions[conditionToUpdate];
			conditionNameTextBox.Text = c.conditionName;
			ignoreFollowingCheckbox.Checked = c.ignoreFollowing;
			emailCheckBox.Checked = c.emailEnabled;
			emailPropertyComboBox.Text = c.emailProperty.ToString().Replace('_', ' ');
			recieverEmailTextBox.Text = string.Join(Environment.NewLine, c.recieverEmails.ToArray());
			twitterCheckBox.Checked = c.twitterEnabled;
			twitterAccountComboBox.Text = c.twitterAccount;
			tweetFirstFormatTextBox.Text = c.tweetFirstFormat;
			tweetLastFormatTextBox.Text = c.tweetLastFormat;
			tweetMapCheckBox.Checked = c.tweetMap;
			tweetLinkComboBox.Text = c.tweetLink.ToString().Replace('_', ' ');
			alertTypeComboBox.Text = c.alertType.ToString().Replace('_', ' ');
			foreach (Core.Trigger trigger in c.triggers.Values) {
				triggerDataGridView.Rows.Add();
				DataGridViewRow newRow = triggerDataGridView.Rows[triggerDataGridView.Rows.Count - 2];
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(newRow.Cells[0]);
				foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));
				newRow.Cells[0].Value = trigger.Property.ToString().Replace('_', ' ');
				newRow.Cells[1].Value = trigger.ComparisonType;
				newRow.Cells[2].Value = trigger.Value;
			}

			emailPropertyComboBox.Enabled = emailCheckBox.Checked;
			recieverEmailTextBox.Enabled = emailCheckBox.Checked;
			twitterAccountComboBox.Enabled = twitterCheckBox.Checked;
			tweetFirstFormatTextBox.Enabled = twitterCheckBox.Checked;
		}
		
		/// <summary>
		/// Initialise form with no existing condition
		/// </summary>
		public Condition_Editor() {
			//Initialise form options
			Initialise();
			//Set Defaults
			conditionNameTextBox.Text = "New Condition";
			tweetFirstFormatTextBox.Text = "";
			emailCheckBox.Checked = false;
			emailPropertyComboBox.Text = "Registration";
			twitterCheckBox.Checked = false;
			twitterAccountComboBox.Text = "";
			tweetLinkComboBox.SelectedIndex = 0;
			tweetFirstFormatTextBox.Text = "Alert! Registration: [Reg], Callsign: [Call]";
			tweetLastFormatTextBox.Text = "Aircraft out of range, Registration: [Reg], Callsign: [Call]";
			alertTypeComboBox.SelectedIndex = 1;
		}
		
		/// <summary>
		/// Initialise form options
		/// </summary>
		public void Initialise() {
			//Initialise form elements
			InitializeComponent();

			//Add vrs properties to triggers table
			IEnumerable<DataGridViewRow> rows = triggerDataGridView.Rows.Cast<DataGridViewRow>();
            foreach(DataGridViewRow row in rows) {
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
				foreach(Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));
			}

			//Add vrs properties to combobox
			foreach(Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty)))
				emailPropertyComboBox.Items.Add(property.ToString().Replace('_', ' '));
			//Add alert types to combobox
			foreach(Core.AlertType property in Enum.GetValues(typeof(Core.AlertType)))
				alertTypeComboBox.Items.Add(property.ToString().Replace('_', ' '));
			//Add tweet link types
			foreach (Core.TweetLink linktype in Enum.GetValues(typeof(Core.TweetLink)))
				tweetLinkComboBox.Items.Add(linktype.ToString().Replace('_', ' '));
			//Add twitter accounts to combobox
			twitterAccountComboBox.Items.Add("Add Account");
			twitterAccountComboBox.Items.AddRange(Settings.TwitterUsers.Keys.ToArray());
		}
		
		/// <summary>
		/// Trigger table cell value changed
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void triggerDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (triggerDataGridView.Rows.Count != 1) {
				//Check if cell changed is in the value column and value isnt empty
				if (e.ColumnIndex == 2 && triggerDataGridView.Rows[e.RowIndex].Cells[0].Value != null && triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() != "") {
					//Clear value if property is number and value is not a number
					if (triggerDataGridView.Rows[e.RowIndex].Cells[2].Value != null && triggerDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() != "" && Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Replace(' ', '_'))][0] == "Number") {
						try {
							Convert.ToInt32(triggerDataGridView.Rows[e.RowIndex].Cells[2].Value);
						}
						catch (Exception) {
							triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
						}
					}
				}
				//Change comparison type combo box based on property selected
				if (e.ColumnIndex == 0) {
					//Clear combobox
					DataGridViewComboBoxCell comparisonTypeComboBox = (DataGridViewComboBoxCell)(triggerDataGridView.Rows[e.RowIndex].Cells[1]);
					comparisonTypeComboBox.Items.Clear();
					comparisonTypeComboBox.Value = "";

					//Get comparison types supported by property
					string supportedComparisonTypes = "";
					try {
						supportedComparisonTypes = Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), triggerDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Replace(' ', '_'))][1];
					}
					catch (Exception) {
						return;
					}
					//Add comparison types to combobox from supported comparison types
					if (supportedComparisonTypes.Contains("A")) {
						foreach (string comparisonType in Core.comparisonTypes["A"]) comparisonTypeComboBox.Items.Add(comparisonType);
					}
					if (supportedComparisonTypes.Contains("B")) {
						foreach (string comparisonType in Core.comparisonTypes["B"]) comparisonTypeComboBox.Items.Add(comparisonType);
					}
					if (supportedComparisonTypes.Contains("C")) {
						foreach (string comparisonType in Core.comparisonTypes["C"]) comparisonTypeComboBox.Items.Add(comparisonType);
						triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "True";
						triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
					}
					else {
						triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
						triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = false;
					}
					if (supportedComparisonTypes.Contains("D")) {
						foreach (string comparisonType in Core.comparisonTypes["D"]) comparisonTypeComboBox.Items.Add(comparisonType);
					}
					if (supportedComparisonTypes.Contains("E")) {
						foreach (string comparisonType in Core.comparisonTypes["E"]) comparisonTypeComboBox.Items.Add(comparisonType);
					}

					comparisonTypeComboBox.Value = comparisonTypeComboBox.Items[0].ToString();
				}
			}
		}
		
		/// <summary>
		/// Trigger table row added
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void TriggerDataGridViewUserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			//Iterate rows in table
			foreach (DataGridViewRow row in triggerDataGridView.Rows) {
				DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
				//Add vrs properties to combobox
				comboBoxCell.Items.Clear();
				foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));
			}
		}
		
		/// <summary>
		/// Save button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void SaveButtonClick(object sender, EventArgs e) {
			bool cancelSave = false;

			//Check if values are empty/invalid
			if (conditionNameTextBox.Text == "") {
				conditionNameLabel.ForeColor = Color.Red;
				cancelSave = true;
			}
			else {
				conditionNameLabel.ForeColor = SystemColors.ControlText;
			}
			if (emailPropertyComboBox.Text == "") {
				emailPropertyLabel.ForeColor = Color.Red;
				cancelSave = true;
			}
			else {
				emailPropertyLabel.ForeColor = SystemColors.ControlText;
			}
			if (emailCheckBox.Checked) {
				if (recieverEmailTextBox.Text == "") {
					emailToSendToLabel.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					emailToSendToLabel.ForeColor = SystemColors.ControlText;
				}
			}
			if (twitterCheckBox.Checked) {
				if (twitterAccountComboBox.Text == "") {
					twitterAccountLabel.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					twitterAccountLabel.ForeColor = SystemColors.ControlText;
				}

				if (tweetFirstFormatTextBox.Text == "") {
					tweetFirstFormatLabel.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					if (tweetFirstFormatTextBox.Text.Contains("@")) {
						tweetFirstFormatLabel.ForeColor = Color.Red;
						cancelSave = true;
						MessageBox.Show("Mentions are not allowed in automated tweets as per Twitter rules", "Mentions not permitted");
					}
					else tweetFirstFormatLabel.ForeColor = SystemColors.ControlText;
				}

				if (tweetLastFormatTextBox.Text == "") {
					tweetLastFormatLabel.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					if (tweetLastFormatTextBox.Text.Contains("@")) {
						tweetLastFormatLabel.ForeColor = Color.Red;
						cancelSave = true;
						MessageBox.Show("Mentions are not allowed in automated tweets as per Twitter rules", "Mentions not permitted");
					}
					else tweetLastFormatLabel.ForeColor = SystemColors.ControlText;
				}
			}
			if(alertTypeComboBox.Text == "") {
				alertTypeLabel.ForeColor = Color.Red;
				cancelSave = true;
			}
			else {
				alertTypeLabel.ForeColor = SystemColors.ControlText;
			}
			if(triggerDataGridView.Rows.Count == 1) {
				triggerDataGridView.BackgroundColor = Color.Red;
				cancelSave = true;
			}
			else {
				triggerDataGridView.BackgroundColor = SystemColors.AppWorkspace;
			}
			//Trim empty lines from email textbox
			recieverEmailTextBox.Text = recieverEmailTextBox.Text.TrimEnd('\r', '\n');
			//Check if emails are valid
			foreach (string line in recieverEmailTextBox.Lines) {
				try {
					new System.Net.Mail.MailAddress(line);
				}
				catch (FormatException) {
					emailToSendToLabel.ForeColor = Color.Red;
					cancelSave = true;
					break;
				}
			}
			//Cancel if values are invalid
			if (cancelSave) {
				return;
			}

			//If condition is being updated, remove the old one
			if (isUpdating)
				EditorConditionsList.conditions.Remove(conditionToUpdate);

			//Sort conditions
			List<int> list = EditorConditionsList.conditions.Keys.ToList();
			SortedDictionary<int, Core.Condition> sortedConditions = new SortedDictionary<int, Core.Condition>();
			list.Sort();
			foreach(var key in list)
				sortedConditions.Add(key, EditorConditionsList.conditions[key]);
			EditorConditionsList.conditions = sortedConditions;

			//Create new condition
			Core.Condition newCondition = new Core.Condition {
				conditionName = conditionNameTextBox.Text,
				alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), alertTypeComboBox.Text.Replace(' ', '_')),
				ignoreFollowing = ignoreFollowingCheckbox.Checked,
				emailEnabled = emailCheckBox.Checked,
				emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), emailPropertyComboBox.Text.Replace(' ', '_')),
				recieverEmails = recieverEmailTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList(),
				twitterEnabled = twitterCheckBox.Checked,
				twitterAccount = twitterAccountComboBox.Text,
				tweetFirstFormat = tweetFirstFormatTextBox.Text,
				tweetLastFormat = tweetLastFormatTextBox.Text,
				tweetMap = tweetMapCheckBox.Checked,
				tweetLink = (Core.TweetLink)Enum.Parse(typeof(Core.TweetLink), tweetLinkComboBox.Text.Replace(' ', '_'))
			};
			if (triggerDataGridView.Rows.Count != 0)
				foreach (DataGridViewRow row in triggerDataGridView.Rows)
					if (row.Index != triggerDataGridView.Rows.Count - 1)
						foreach (Core.vrsProperty property in Enum.GetValues(typeof(Core.vrsProperty)))
							if (property.ToString() == row.Cells[0].Value.ToString().Replace(' ', '_')) {
								newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger(property, row.Cells[2].Value.ToString(), row.Cells[1].Value.ToString()));
								break;
							}
			//Add condition to condition list
			if (isUpdating)
				EditorConditionsList.conditions.Add(conditionToUpdate, newCondition);
			else
				EditorConditionsList.conditions.Add(EditorConditionsList.conditions.Count, newCondition);
			//Close form
			Close();
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		private void propertyInfoButton_Click(object sender, EventArgs e) {
			//Show property info form
			PropertyInfoForm propertyInfoForm = new PropertyInfoForm();
			propertyInfoForm.Show();
		}

		/// <summary>
		/// Update controls when email checkbox is toggled
		/// </summary>
		private void emailCheckBox_CheckedChanged(object sender, EventArgs e) {
			emailPropertyComboBox.Enabled = emailCheckBox.Checked;
			recieverEmailTextBox.Enabled = emailCheckBox.Checked;
		}

		/// <summary>
		/// Update controls when twitter checkbox is toggled
		/// </summary>
		private void twitterCheckBox_CheckedChanged(object sender, EventArgs e) {
			twitterAccountComboBox.Enabled = twitterCheckBox.Checked;
			tweetFirstFormatTextBox.Enabled = twitterCheckBox.Checked;
			tweetLastFormatTextBox.Enabled = twitterCheckBox.Checked;
		}
		
		//Show add account dialog if add account is selected
		private void twitterAccountComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (twitterAccountComboBox.Text == "Add Account") {
				//Show add account dialog
				Twitter.AddAccount();
				
				//Update accounts
				twitterAccountComboBox.Items.Clear();
				twitterAccountComboBox.Items.Add("Add Account");
				twitterAccountComboBox.Items.AddRange(Settings.TwitterUsers.Keys.ToArray());

				//Clear selection
				twitterAccountComboBox.SelectedIndex = -1;
				twitterAccountComboBox.Text = "";
			}
		}

		private void triggerDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			//Edit combobox on click
			DataGridViewColumn column = triggerDataGridView.Columns[e.ColumnIndex];
			if (column is DataGridViewComboBoxColumn) {
				DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)triggerDataGridView[e.ColumnIndex, e.RowIndex];
				triggerDataGridView.CurrentCell = cell;
				triggerDataGridView.BeginEdit(true);

				DataGridViewComboBoxEditingControl editingControl = (DataGridViewComboBoxEditingControl)triggerDataGridView.EditingControl;
				editingControl.DroppedDown = true;
			}
		}
	}
}
