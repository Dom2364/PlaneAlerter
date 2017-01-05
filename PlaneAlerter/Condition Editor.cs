using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PlaneAlerter {
	public partial class Form1 :Form {
		
		
		public Form1() {
			InitializeComponent();
			
			if (File.Exists("conditions.json")) {
				EditorConditionsList.conditions.Clear();
				string conditionsJsonText = File.ReadAllText("conditions.json");
				JObject conditionJson = (JObject)JsonConvert.DeserializeObject(conditionsJsonText);
				for (int conditionid = 0;conditionid<conditionJson.Count;conditionid++) {
					JToken condition = conditionJson[conditionid.ToString()];
					Core.Condition newCondition = new Core.Condition();
					newCondition.conditionName = condition["conditionName"].ToString();
					newCondition.emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition["emailProperty"].ToString());
					newCondition.alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString());
					newCondition.ignoreFollowing = (bool)condition["ignoreFollowing"];
					List<string> emailsArray = new List<string>();
					foreach(JToken email in condition["recieverEmails"])
						emailsArray.Add(email.ToString());
					newCondition.recieverEmails = emailsArray;
					foreach(JToken trigger in condition["triggers"].Values())
						newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger((Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));
					EditorConditionsList.conditions.Add(conditionid, newCondition);
				}
			}
			updateConditionList();
		}

		public void updateConditionList() {
			conditionEditorTreeView.Nodes.Clear();
			foreach (int conditionid in EditorConditionsList.conditions.Keys) {
				Core.Condition condition = EditorConditionsList.conditions[conditionid];
				TreeNode conditionNode = conditionEditorTreeView.Nodes.Add(conditionid + ": " + condition.conditionName);
				conditionNode.Tag = conditionid;
				conditionNode.Nodes.Add("Id: " + conditionid);
				conditionNode.Nodes.Add("Email Parameter: " + condition.emailProperty.ToString());
				conditionNode.Nodes.Add("Reciever Emails: " + string.Join(", ", condition.recieverEmails.ToArray()));
				conditionNode.Nodes.Add("Alert Type: " + condition.alertType);
				TreeNode triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (Core.Trigger trigger in condition.triggers.Values)
					triggersNode.Nodes.Add(trigger.Property.ToString() + " " + trigger.ComparisonType + " " + trigger.Value);
			}
		}

		private void addConditionButton_Click(object sender, EventArgs e) {
			Condition_Editor editor = new Condition_Editor();
			editor.ShowDialog();
			updateConditionList();
		}
		
		void ConditionTreeViewNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag == null || e.Node.Tag.ToString() == "")
				return;
			Condition_Editor editor = new Condition_Editor(Convert.ToInt32(e.Node.Tag));
			editor.ShowDialog();
			updateConditionList();
		}
		
		void ExitButtonClick(object sender, EventArgs e)
		{
			string conditionsJson = JsonConvert.SerializeObject(EditorConditionsList.conditions, Formatting.Indented);
			File.WriteAllText("conditions.json", conditionsJson);
			Close();
		}
		
		void RemoveConditionButtonClick(object sender, EventArgs e)
		{
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			EditorConditionsList.conditions.Remove(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			SortedDictionary<int, Core.Condition> sortedConditions = new SortedDictionary<int, Core.Condition>();
			int id = 0;
			foreach (Core.Condition c in EditorConditionsList.conditions.Values) {
				sortedConditions.Add(id,c);
				id++;
			}
			EditorConditionsList.conditions = sortedConditions;
			updateConditionList();
		}

		private void moveUpButton_Click(object sender, EventArgs e) {
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
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

		private void moveDownButton_Click(object sender, EventArgs e) {
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
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
	}

	public class EditorConditionsList {
		public static SortedDictionary<int, Core.Condition> conditions = new SortedDictionary<int, Core.Condition>();
	}
}
