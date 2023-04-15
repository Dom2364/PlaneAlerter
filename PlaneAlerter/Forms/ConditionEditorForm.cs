using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	internal partial class ConditionEditorForm :Form {
		private readonly IConditionManagerService _conditionManagerService;
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ITwitterService _twitterService;

		/// <summary>
		/// Id of condition to update
		/// </summary>
		private int _conditionToUpdate;

		/// <summary>
		/// Is this updating the condition?
		/// </summary>
		private bool _isUpdating;
		
		/// <summary>
		/// Initialise form with no existing condition
		/// </summary>
		public ConditionEditorForm(IConditionManagerService conditionManagerService, ISettingsManagerService settingsManagerService, ITwitterService twitterService) {
			_conditionManagerService = conditionManagerService;
			_settingsManagerService = settingsManagerService;
			_twitterService = twitterService;
			
			//Initialise form elements
			InitializeComponent();

			//Add vrs properties to triggers table
			var rows = triggerDataGridView.Rows.Cast<DataGridViewRow>();
			foreach (var row in rows)
			{
				var comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
				foreach (VrsProperty property in Enum.GetValues(typeof(VrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));
			}

			//Add alert types to combobox
			foreach (AlertType property in Enum.GetValues(typeof(AlertType)))
				alertTypeComboBox.Items.Add(property.ToString().Replace('_', ' '));

			//Add tweet link types
			foreach (TweetLink linkType in Enum.GetValues(typeof(TweetLink)))
				tweetLinkComboBox.Items.Add(linkType.ToString().Replace('_', ' '));

			//Add twitter accounts to combobox
			twitterAccountComboBox.Items.Add("Add Account");
			twitterAccountComboBox.Items.AddRange(_settingsManagerService.Settings.TwitterUsers.Keys.ToArray());

			//Set Defaults
			conditionNameTextBox.Text = "New Condition";
			tweetFirstFormatTextBox.Text = "";
			emailCheckBox.Checked = false;
			emailFirstFormatTextBox.Text = "First Contact Alert! [conditionname]: [reg]";
			emailLastFormatTextBox.Text = "Last Contact Alert! [conditionname]: [reg]";
			twitterCheckBox.Checked = false;
			twitterAccountComboBox.Text = "";
			tweetLinkComboBox.SelectedIndex = 0;
			tweetFirstFormatTextBox.Text = "Alert! Registration: [Reg], Callsign: [Call]";
			tweetLastFormatTextBox.Text = "Aircraft out of range, Registration: [Reg], Callsign: [Call]";
			alertTypeComboBox.SelectedIndex = 1;
		}

		public void LoadCondition(int conditionId)
		{
			_isUpdating = true;
			_conditionToUpdate = conditionId;

			//Set form element values from condition info
			var c = _conditionManagerService.EditorConditions[_conditionToUpdate];
			conditionNameTextBox.Text = c.Name;
			ignoreFollowingCheckbox.Checked = c.IgnoreFollowing;
			emailCheckBox.Checked = c.EmailEnabled;
			emailFirstFormatTextBox.Text = c.EmailFirstFormat;
			emailLastFormatTextBox.Text = c.EmailLastFormat;
			receiverEmailTextBox.Text = string.Join(Environment.NewLine, c.RecieverEmails.ToArray());
			twitterCheckBox.Checked = c.TwitterEnabled;
			twitterAccountComboBox.Text = c.TwitterAccount;
			tweetFirstFormatTextBox.Text = c.TweetFirstFormat;
			tweetLastFormatTextBox.Text = c.TweetLastFormat;
			tweetMapCheckBox.Checked = c.TweetMap;
			tweetLinkComboBox.Text = c.TweetLink.ToString().Replace('_', ' ');
			alertTypeComboBox.Text = c.AlertType.ToString().Replace('_', ' ');

			foreach (var trigger in c.Triggers.Values)
			{
				triggerDataGridView.Rows.Add();
				var newRow = triggerDataGridView.Rows[triggerDataGridView.Rows.Count - 2];

				var comboBoxCell = (DataGridViewComboBoxCell)(newRow.Cells[0]);
				foreach (VrsProperty property in Enum.GetValues(typeof(VrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));

				newRow.Cells[0].Value = trigger.Property.ToString().Replace('_', ' ');
				newRow.Cells[1].Value = trigger.ComparisonType;
				newRow.Cells[2].Value = trigger.Value;
			}
		}
		
		/// <summary>
		/// Trigger table cell value changed
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void triggerDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (triggerDataGridView.Rows.Count == 1)
				return;

			var selectedPropertyKey = triggerDataGridView.Rows[e.RowIndex].Cells[0].Value?.ToString();
			var selectedProperty = !string.IsNullOrEmpty(selectedPropertyKey)
				? (VrsProperty?)Enum.Parse(typeof(VrsProperty), selectedPropertyKey.Replace(' ', '_'))
				: null;

			//Check if cell changed is in the value column and value isn't empty
			if (e.ColumnIndex == 2 &&
			    selectedProperty != null)
			{
				//Clear value if property is number and value is not a number
				if (VrsProperties.VrsPropertyData[selectedProperty.Value][0] == "Number" &&
				    !string.IsNullOrEmpty(triggerDataGridView.Rows[e.RowIndex].Cells[2].Value?.ToString()) &&
				    !double.TryParse(triggerDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out _))
				{
					triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
				}
			}

			//Change comparison type combo box based on property selected
			if (e.ColumnIndex == 0 && selectedProperty != null) {
				//Clear combobox
				var comparisonTypeComboBox = (DataGridViewComboBoxCell)(triggerDataGridView.Rows[e.RowIndex].Cells[1]);
				comparisonTypeComboBox.Items.Clear();
				comparisonTypeComboBox.Value = "";

				//Get comparison types supported by property
				var supportedComparisonTypes = VrsProperties.VrsPropertyData[selectedProperty.Value][1];

				//Add comparison types to combobox from supported comparison types
				if (supportedComparisonTypes.Contains("A")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["A"])
						comparisonTypeComboBox.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("B")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["B"])
						comparisonTypeComboBox.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("C")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["C"])
						comparisonTypeComboBox.Items.Add(comparisonType);

					triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "True";
					triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
				}
				else {
					triggerDataGridView.Rows[e.RowIndex].Cells[2].Value = "";
					triggerDataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = false;
				}

				if (supportedComparisonTypes.Contains("D")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["D"])
						comparisonTypeComboBox.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("E")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["E"])
						comparisonTypeComboBox.Items.Add(comparisonType);
				}

				comparisonTypeComboBox.Value = comparisonTypeComboBox.Items[0].ToString();
			}
		}
		
		/// <summary>
		/// Trigger table row added
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void TriggerDataGridViewUserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			//Iterate rows in table
			foreach (DataGridViewRow row in triggerDataGridView.Rows) {
				var comboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);

				//Add vrs properties to combobox
				comboBoxCell.Items.Clear();
				foreach (VrsProperty property in Enum.GetValues(typeof(VrsProperty)))
					comboBoxCell.Items.Add(property.ToString().Replace('_', ' '));
			}
		}
		
		/// <summary>
		/// Save button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void SaveButtonClick(object sender, EventArgs e) {
			var cancelSave = false;

			//Check if values are empty/invalid
			if (conditionNameTextBox.Text == "") {
				conditionNameLabel.ForeColor = Color.Red;
				cancelSave = true;
			}
			else {
				conditionNameLabel.ForeColor = SystemColors.ControlText;
			}

			if (emailCheckBox.Checked) {
				if (emailFirstFormatTextBox.Text == "") {
					emailFirstFormatTextBox.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					emailFirstFormatTextBox.ForeColor = SystemColors.ControlText;
				}

				if (emailLastFormatTextBox.Text == "") {
					emailLastFormatTextBox.ForeColor = Color.Red;
					cancelSave = true;
				}
				else {
					emailLastFormatTextBox.ForeColor = SystemColors.ControlText;
				}

				if (receiverEmailTextBox.Text == "") {
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
			if (alertTypeComboBox.Text == "") {
				alertTypeLabel.ForeColor = Color.Red;
				cancelSave = true;
			}
			else {
				alertTypeLabel.ForeColor = SystemColors.ControlText;
			}
			if (triggerDataGridView.Rows.Count == 1) {
				triggerDataGridView.BackgroundColor = Color.Red;
				cancelSave = true;
			}
			else {
				triggerDataGridView.BackgroundColor = SystemColors.AppWorkspace;
			}
			//Trim empty lines from email textbox
			receiverEmailTextBox.Text = receiverEmailTextBox.Text.TrimEnd('\r', '\n');
			//Check if emails are valid
			foreach (var line in receiverEmailTextBox.Lines) {
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
			if (_isUpdating)
				_conditionManagerService.EditorConditions.Remove(_conditionToUpdate);

			//Sort conditions
			var list = _conditionManagerService.EditorConditions.Keys.ToList();
			var sortedConditions = new SortedDictionary<int, Condition>();
			list.Sort();
			foreach(var key in list)
				sortedConditions.Add(key, _conditionManagerService.EditorConditions[key]);
			_conditionManagerService.EditorConditions = sortedConditions;

			//Create new condition
			var newCondition = new Condition(
				conditionNameTextBox.Text,
				(AlertType)Enum.Parse(typeof(AlertType), alertTypeComboBox.Text.Replace(' ', '_')),
				ignoreFollowingCheckbox.Checked,
				emailCheckBox.Checked,
				emailFirstFormatTextBox.Text,
				emailLastFormatTextBox.Text,
				receiverEmailTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList(),
				twitterCheckBox.Checked,
				twitterAccountComboBox.Text,
				tweetFirstFormatTextBox.Text,
				tweetLastFormatTextBox.Text,
				tweetMapCheckBox.Checked,
				(TweetLink)Enum.Parse(typeof(TweetLink), tweetLinkComboBox.Text.Replace(' ', '_')));

			if (triggerDataGridView.Rows.Count != 0)
			{
				foreach (DataGridViewRow row in triggerDataGridView.Rows)
				{
					if (row.Index == triggerDataGridView.Rows.Count - 1)
						continue;

					var selectedPropertyKey = row.Cells[0].Value?.ToString();
					var selectedProperty = !string.IsNullOrEmpty(selectedPropertyKey)
						? (VrsProperty?)Enum.Parse(typeof(VrsProperty), selectedPropertyKey.Replace(' ', '_'))
						: null;

					var comparisonType = row.Cells[1].Value.ToString();

					if (selectedProperty == null || string.IsNullOrEmpty(comparisonType))
						continue;
					
					newCondition.Triggers.Add(newCondition.Triggers.Count,
						new Trigger(
							selectedProperty.Value,
							row.Cells[2].Value.ToString() ?? "",
							comparisonType));
				}
			}

			//Add condition to condition list
			_conditionManagerService.EditorConditions.Add(_isUpdating ?
				_conditionToUpdate :
				_conditionManagerService.EditorConditions.Count, newCondition);

			//Close form
			Close();
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		private void propertyInfoButton_Click(object sender, EventArgs e) {
			//Show property info form
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show();
		}

		/// <summary>
		/// Update controls when email checkbox is toggled
		/// </summary>
		private void emailCheckBox_CheckedChanged(object sender, EventArgs e) {
			UpdateUiState();
		}

		/// <summary>
		/// Update controls when twitter checkbox is toggled
		/// </summary>
		private void twitterCheckBox_CheckedChanged(object sender, EventArgs e) {
			UpdateUiState();
		}
		
		//Show add account dialog if add account is selected
		private void twitterAccountComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (twitterAccountComboBox.Text != "Add Account")
				return;

			//Show add account dialog
			_twitterService.AddAccount();
				
			//Update accounts
			twitterAccountComboBox.Items.Clear();
			twitterAccountComboBox.Items.Add("Add Account");
			twitterAccountComboBox.Items.AddRange(_settingsManagerService.Settings.TwitterUsers.Keys.ToArray());

			//Clear selection
			twitterAccountComboBox.SelectedIndex = -1;
			twitterAccountComboBox.Text = "";
		}

		private void triggerDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			//Ignore if clicking column header
			if (e.RowIndex == -1)
				return;

			//Edit combobox on click
			var column = triggerDataGridView.Columns[e.ColumnIndex];
			if (column is DataGridViewComboBoxColumn) {
				var cell = (DataGridViewComboBoxCell)triggerDataGridView[e.ColumnIndex, e.RowIndex];
				triggerDataGridView.CurrentCell = cell;
				triggerDataGridView.BeginEdit(true);

				var editingControl = (DataGridViewComboBoxEditingControl)triggerDataGridView.EditingControl;
				editingControl.DroppedDown = true;
			}
		}

		/// <summary>
		/// Update enabled state and styling of UI based on settings
		/// </summary>
		private void UpdateUiState() {
			var alertType = AlertType.First_and_Last_Contact;

			if (!string.IsNullOrEmpty(alertTypeComboBox.Text)) {
				alertType = (AlertType)Enum.Parse(typeof(AlertType), alertTypeComboBox.Text.Replace(' ', '_'));
			}
			
			emailFirstFormatTextBox.Enabled = emailCheckBox.Checked && (alertType != AlertType.Last_Contact);
			emailLastFormatTextBox.Enabled = emailCheckBox.Checked && (alertType != AlertType.First_Contact);
			receiverEmailTextBox.Enabled = emailCheckBox.Checked;

			twitterAccountComboBox.Enabled = twitterCheckBox.Checked;
			tweetFirstFormatTextBox.Enabled = twitterCheckBox.Checked && (alertType != AlertType.Last_Contact);
			tweetLastFormatTextBox.Enabled = twitterCheckBox.Checked && (alertType != AlertType.First_Contact);
		}

		private void alertTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateUiState();
		}
	}
}
