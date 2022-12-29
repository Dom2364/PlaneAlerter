using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	internal partial class ConditionEditor :Form {
		/// <summary>
		/// List of conditions
		/// </summary>
		public static SortedDictionary<int, Condition> Conditions = new SortedDictionary<int, Condition>();

		public ConditionEditor() {
			//Initialise form elements
			InitializeComponent();
			
			//Load conditions
			if (File.Exists("conditions.json")) {
				Conditions.Clear();

				//Parse json
				var conditionsJsonText = File.ReadAllText("conditions.json");
				var conditionJson = (JObject?)JsonConvert.DeserializeObject(conditionsJsonText);

				if (conditionJson != null) {
					//Iterate parsed conditions
					for (var conditionId = 0; conditionId < conditionJson.Count; conditionId++) {
						var condition = conditionJson[conditionId.ToString()];

						//Create condition from parsed json
						var newCondition = new Condition {
							Name = condition["conditionName"].ToString(),
							AlertType = (AlertType)Enum.Parse(typeof(AlertType), condition["alertType"].ToString()),
							IgnoreFollowing = (bool)condition["ignoreFollowing"],
							EmailEnabled = (bool)(condition["emailEnabled"] ?? true),
							EmailFirstFormat = (condition["emailFirstFormat"] ?? "").ToString(),
							EmailLastFormat = (condition["emailLastFormat"] ?? "").ToString(),
							TwitterEnabled = (bool)(condition["twitterEnabled"] ?? false),
							TwitterAccount = (condition["twitterAccount"] ?? "").ToString(),
							TweetFirstFormat = (condition["tweetFirstFormat"] ?? "").ToString(),
							TweetLastFormat = (condition["tweetLastFormat"] ?? "").ToString(),
							TweetMap = (bool)(condition["tweetMap"] ?? true),
							TweetLink = (TweetLink)Enum.Parse(typeof(TweetLink), (condition["tweetLink"] ?? TweetLink.None.ToString()).ToString())
						};

						if (condition["emailProperty"] != null && !string.IsNullOrEmpty(condition["emailProperty"].ToString())) {
							var emailProperty = (VrsProperty)Enum.Parse(typeof(VrsProperty), (condition["emailProperty"] ?? VrsProperty.Registration.ToString()).ToString());
							newCondition.EmailFirstFormat = "First Contact Alert! [ConditionName]: [" + Core.VrsPropertyData[emailProperty][2] + "]";
							newCondition.EmailLastFormat = "Last Contact Alert! [ConditionName]: [" + Core.VrsPropertyData[emailProperty][2] + "]";
						}

						var emailsArray = new List<string>();
						foreach (var email in condition["recieverEmails"])
							emailsArray.Add(email.ToString());
						newCondition.ReceiverEmails = emailsArray;

						foreach (var trigger in condition["triggers"].Values())
							newCondition.Triggers.Add(newCondition.Triggers.Count, new Trigger((VrsProperty)Enum.Parse(typeof(VrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));

						//Add condition to list
						Conditions.Add(conditionId, newCondition);
					}
				}
			}
			UpdateConditionList();
		}

		/// <summary>
		/// Update condition list
		/// </summary>
		public void UpdateConditionList() {
			conditionEditorTreeView.Nodes.Clear();

			foreach (var conditionId in Conditions.Keys) {
				var condition = Conditions[conditionId];

				var conditionNode = conditionEditorTreeView.Nodes.Add(conditionId + ": " + condition.Name);
				conditionNode.Tag = conditionId;
				conditionNode.Nodes.Add("Id: " + conditionId);
				conditionNode.Nodes.Add("Alert Type: " + condition.AlertType);
				conditionNode.Nodes.Add("Email Enabled: " + condition.EmailEnabled);
				conditionNode.Nodes.Add("Twitter Enabled: " + condition.TwitterEnabled);
				conditionNode.Nodes.Add("Twitter Account: " + condition.TwitterAccount);

				var triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (var trigger in condition.Triggers.Values)
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
			}
			
			if (conditionEditorTreeView.Nodes.Count != 0)
				conditionEditorTreeView.SelectedNode = conditionEditorTreeView.Nodes[0];
			
			updateUIState();
		}

		/// <summary>
		/// Add a condition
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void addConditionButton_Click(object sender, EventArgs e) {
			//Show editor dialog then update condition list
			var editor = new ConditionEditorDialog();
			editor.ShowDialog();
			UpdateConditionList();
		}
		
		/// <summary>
		/// Exit button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void ExitButtonClick(object sender, EventArgs e) {
			//Save conditions to file then close
			var conditionsJson = JsonConvert.SerializeObject(Conditions, Formatting.Indented);
			File.WriteAllText("conditions.json", conditionsJson);
			Close();
		}
		
		/// <summary>
		/// Remove condition button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event viewer</param>
		private void RemoveConditionButtonClick(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Remove condition from condition list
			Conditions.Remove(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			//Sort conditions
			var sortedConditions = new SortedDictionary<int, Condition>();
			var id = 0;
			foreach (var c in Conditions.Values) {
				sortedConditions.Add(id,c);
				id++;
			}
			Conditions = sortedConditions;
			//Update condition list
			UpdateConditionList();
		}

		/// <summary>
		/// Move up button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void moveUpButton_Click(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Swap conditions then update condition list
			var conditionId = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionId == 0) return;
			var c1 = Conditions[conditionId];
			var c2 = Conditions[conditionId - 1];
			Conditions.Remove(conditionId - 1);
			Conditions.Remove(conditionId);
			Conditions.Add(conditionId - 1, c1);
			Conditions.Add(conditionId, c2);
			UpdateConditionList();
		}

		/// <summary>
		/// Move down button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void moveDownButton_Click(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Swap conditions then update condition list
			var conditionId = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionId == Conditions.Count - 1) return;
			var c1 = Conditions[conditionId];
			var c2 = Conditions[conditionId + 1];
			Conditions.Remove(conditionId + 1);
			Conditions.Remove(conditionId);
			Conditions.Add(conditionId + 1, c1);
			Conditions.Add(conditionId, c2);
			UpdateConditionList();
		}

		/// <summary>
		/// Edit button click
		/// </summary>
		private void editButton_Click(object sender, EventArgs e) {
			var node = conditionEditorTreeView.SelectedNode;
			
			//Check if node is valid
			if (node?.Tag == null || node.Tag.ToString() == "")
				return;
			
			//Open editor, update list once closed
			var editor = new ConditionEditorDialog(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			editor.ShowDialog();
			UpdateConditionList();
		}

		/// <summary>
		/// Update buttons enabled state
		/// </summary>
		private void updateUIState() {
			//If node is valid, enable buttons
			var node = conditionEditorTreeView.SelectedNode;
			var conditionSelected = node != null && node.Tag != null && node.Tag.ToString() != "";
			removeConditionButton.Enabled = conditionSelected;
			editButton.Enabled = conditionSelected;
			moveUpButton.Enabled = conditionSelected;
			moveDownButton.Enabled = conditionSelected;
		}

		private void conditionEditorTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
			updateUIState();
		}
	}
}
