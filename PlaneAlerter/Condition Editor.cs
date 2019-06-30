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
				EditorConditionsList.conditions.Clear();
				//Parse json
				string conditionsJsonText = File.ReadAllText("conditions.json");
				JObject conditionJson = (JObject)JsonConvert.DeserializeObject(conditionsJsonText);
				if (conditionJson != null) {
					//Iterate parsed conditions
					for (int conditionid = 0; conditionid < conditionJson.Count; conditionid++) {
						JToken condition = conditionJson[conditionid.ToString()];
						//Create condition from parsed json
						Core.Condition newCondition = new Core.Condition {
							conditionName = condition["conditionName"].ToString(),
							alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString()),
							ignoreFollowing = (bool)condition["ignoreFollowing"],
							emailEnabled = (bool)condition["emailEnabled"],
							twitterEnabled = (bool)condition["twitterEnabled"],
							twitterAccount = condition["twitterAccount"].ToString(),
							tweetFormat = condition["tweetFormat"].ToString(),
							emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition["emailProperty"].ToString())
						};
						List<string> emailsArray = new List<string>();
						foreach (JToken email in condition["recieverEmails"])
							emailsArray.Add(email.ToString());
						newCondition.recieverEmails = emailsArray;
						foreach (JToken trigger in condition["triggers"].Values())
							newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger((Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));
						//Add condition to list
						EditorConditionsList.conditions.Add(conditionid, newCondition);
					}
				}
			}
			updateConditionList();
		}

		/// <summary>
		/// Update condition list
		/// </summary>
		public void updateConditionList() {
			conditionEditorTreeView.Nodes.Clear();
			foreach (int conditionid in EditorConditionsList.conditions.Keys) {
				Core.Condition condition = EditorConditionsList.conditions[conditionid];
				TreeNode conditionNode = conditionEditorTreeView.Nodes.Add(conditionid + ": " + condition.conditionName);
				conditionNode.Tag = conditionid;
				conditionNode.Nodes.Add("Id: " + conditionid);
				conditionNode.Nodes.Add("Alert Type: " + condition.alertType);
				conditionNode.Nodes.Add("Email Enabled: " + condition.emailEnabled);
				conditionNode.Nodes.Add("Twitter Enabled: " + condition.twitterEnabled);
				conditionNode.Nodes.Add("Reciever Emails: " + string.Join(", ", condition.recieverEmails.ToArray()));
				conditionNode.Nodes.Add("Twitter Account: " + condition.twitterAccount);
				TreeNode triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (Core.Trigger trigger in condition.triggers.Values)
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
			}
		}

		/// <summary>
		/// Add a condition
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void addConditionButton_Click(object sender, EventArgs e) {
			//Show editor dialog then update condition list
			Condition_Editor editor = new Condition_Editor();
			editor.ShowDialog();
			updateConditionList();
		}
		
		/// <summary>
		/// Exit button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		void ExitButtonClick(object sender, EventArgs e)
		{
			//Save conditions to file then close
			string conditionsJson = JsonConvert.SerializeObject(EditorConditionsList.conditions, Formatting.Indented);
			File.WriteAllText("conditions.json", conditionsJson);
			Close();
		}
		
		/// <summary>
		/// Remove condition button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event viewer</param>
		void RemoveConditionButtonClick(object sender, EventArgs e)
		{
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Remove condition from condition list
			EditorConditionsList.conditions.Remove(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			//Sort conditions
			SortedDictionary<int, Core.Condition> sortedConditions = new SortedDictionary<int, Core.Condition>();
			int id = 0;
			foreach (Core.Condition c in EditorConditionsList.conditions.Values) {
				sortedConditions.Add(id,c);
				id++;
			}
			EditorConditionsList.conditions = sortedConditions;
			//Update condition list
			updateConditionList();
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
			int conditionid = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionid == 0) return;
			Core.Condition c1 = EditorConditionsList.conditions[conditionid];
			Core.Condition c2 = EditorConditionsList.conditions[conditionid - 1];
			EditorConditionsList.conditions.Remove(conditionid - 1);
			EditorConditionsList.conditions.Remove(conditionid);
			EditorConditionsList.conditions.Add(conditionid - 1, c1);
			EditorConditionsList.conditions.Add(conditionid, c2);
			updateConditionList();
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
			int conditionid = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionid == EditorConditionsList.conditions.Count - 1) return;
			Core.Condition c1 = EditorConditionsList.conditions[conditionid];
			Core.Condition c2 = EditorConditionsList.conditions[conditionid + 1];
			EditorConditionsList.conditions.Remove(conditionid + 1);
			EditorConditionsList.conditions.Remove(conditionid);
			EditorConditionsList.conditions.Add(conditionid + 1, c1);
			EditorConditionsList.conditions.Add(conditionid, c2);
			updateConditionList();
		}

		/// <summary>
		/// Edit button click
		/// </summary>
		private void editButton_Click(object sender, EventArgs e) {
			TreeNode node = conditionEditorTreeView.SelectedNode;
			//Check if node is valid
			if (node != null && node.Tag != null && node.Tag.ToString() != "") {
				//Open editor, update list once closed
				Condition_Editor editor = new Condition_Editor(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
				editor.ShowDialog();
				updateConditionList();
			}
		}

		/// <summary>
		/// Node mouse click
		/// </summary>
		private void conditionEditorTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			//If node is valid, enable edit button
			TreeNode node = e.Node;
			editButton.Enabled = (node != null && node.Tag != null && node.Tag.ToString() != "");
		}
	}

	/// <summary>
	/// Editor conditions list
	/// </summary>
	public class EditorConditionsList {
		/// <summary>
		/// List of conditions
		/// </summary>
		public static SortedDictionary<int, Core.Condition> conditions = new SortedDictionary<int, Core.Condition>();
	}
}
