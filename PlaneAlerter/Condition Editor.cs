using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PlaneAlerter {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	public partial class ConditionEditor :Form {
		public ConditionEditor() {
			//Initialise form elements
			InitializeComponent();
			
			//Load conditions
			if (File.Exists("conditions.json")) {
				EditorConditionsList.Conditions.Clear();

				//Parse json
				var conditionsJsonText = File.ReadAllText("conditions.json");
				var conditionJson = (JObject?)JsonConvert.DeserializeObject(conditionsJsonText);

				if (conditionJson != null) {
					//Iterate parsed conditions
					for (var conditionId = 0; conditionId < conditionJson.Count; conditionId++) {
						var condition = conditionJson[conditionId.ToString()];

						//Create condition from parsed json
						var newCondition = new Core.Condition {
							conditionName = condition["conditionName"].ToString(),
							alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString()),
							ignoreFollowing = (bool)condition["ignoreFollowing"],
							emailEnabled = (bool)(condition["emailEnabled"] ?? true),
							emailFirstFormat = (condition["emailFirstFormat"] ?? "").ToString(),
							emailLastFormat = (condition["emailLastFormat"] ?? "").ToString(),
							twitterEnabled = (bool)(condition["twitterEnabled"] ?? false),
							twitterAccount = (condition["twitterAccount"] ?? "").ToString(),
							tweetFirstFormat = (condition["tweetFirstFormat"] ?? "").ToString(),
							tweetLastFormat = (condition["tweetLastFormat"] ?? "").ToString(),
							tweetMap = (bool)(condition["tweetMap"] ?? true),
							tweetLink = (Core.TweetLink)Enum.Parse(typeof(Core.TweetLink), (condition["tweetLink"] ?? Core.TweetLink.None.ToString()).ToString())
						};

						if (condition["emailProperty"] != null && !string.IsNullOrEmpty(condition["emailProperty"].ToString())) {
							var emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), (condition["emailProperty"] ?? Core.vrsProperty.Registration.ToString()).ToString());
							newCondition.emailFirstFormat = "First Contact Alert! [ConditionName]: [" + Core.vrsPropertyData[emailProperty][2] + "]";
							newCondition.emailLastFormat = "Last Contact Alert! [ConditionName]: [" + Core.vrsPropertyData[emailProperty][2] + "]";
						}

						var emailsArray = new List<string>();
						foreach (var email in condition["recieverEmails"])
							emailsArray.Add(email.ToString());
						newCondition.recieverEmails = emailsArray;

						foreach (var trigger in condition["triggers"].Values())
							newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger((Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));

						//Add condition to list
						EditorConditionsList.Conditions.Add(conditionId, newCondition);
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

			foreach (var conditionId in EditorConditionsList.Conditions.Keys) {
				var condition = EditorConditionsList.Conditions[conditionId];

				var conditionNode = conditionEditorTreeView.Nodes.Add(conditionId + ": " + condition.conditionName);
				conditionNode.Tag = conditionId;
				conditionNode.Nodes.Add("Id: " + conditionId);
				conditionNode.Nodes.Add("Alert Type: " + condition.alertType);
				conditionNode.Nodes.Add("Email Enabled: " + condition.emailEnabled);
				conditionNode.Nodes.Add("Twitter Enabled: " + condition.twitterEnabled);
				conditionNode.Nodes.Add("Twitter Account: " + condition.twitterAccount);

				var triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (var trigger in condition.triggers.Values)
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
			var conditionsJson = JsonConvert.SerializeObject(EditorConditionsList.Conditions, Formatting.Indented);
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
			EditorConditionsList.Conditions.Remove(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			//Sort conditions
			var sortedConditions = new SortedDictionary<int, Core.Condition>();
			var id = 0;
			foreach (var c in EditorConditionsList.Conditions.Values) {
				sortedConditions.Add(id,c);
				id++;
			}
			EditorConditionsList.Conditions = sortedConditions;
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
			var c1 = EditorConditionsList.Conditions[conditionId];
			var c2 = EditorConditionsList.Conditions[conditionId - 1];
			EditorConditionsList.Conditions.Remove(conditionId - 1);
			EditorConditionsList.Conditions.Remove(conditionId);
			EditorConditionsList.Conditions.Add(conditionId - 1, c1);
			EditorConditionsList.Conditions.Add(conditionId, c2);
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
			if (conditionId == EditorConditionsList.Conditions.Count - 1) return;
			var c1 = EditorConditionsList.Conditions[conditionId];
			var c2 = EditorConditionsList.Conditions[conditionId + 1];
			EditorConditionsList.Conditions.Remove(conditionId + 1);
			EditorConditionsList.Conditions.Remove(conditionId);
			EditorConditionsList.Conditions.Add(conditionId + 1, c1);
			EditorConditionsList.Conditions.Add(conditionId, c2);
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

	/// <summary>
	/// Editor conditions list
	/// </summary>
	public class EditorConditionsList {
		/// <summary>
		/// List of conditions
		/// </summary>
		public static SortedDictionary<int, Core.Condition> Conditions = new SortedDictionary<int, Core.Condition>();
	}
}
