﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IConditionManagerService
	{
		/// <summary>
		/// List of current conditions
		/// </summary>
		Dictionary<int, Condition> Conditions { get; set; }

		/// <summary>
		/// Have conditions loaded?
		/// </summary>
		bool ConditionsLoaded { get; set; }

		/// <summary>
		/// Load conditions
		/// </summary>
		void LoadConditions();
	}

	internal class ConditionManagerService : IConditionManagerService
	{
		/// <summary>
		/// List of current conditions
		/// </summary>
		public Dictionary<int, Condition> Conditions { get; set; } = new Dictionary<int, Condition>();

		/// <summary>
		/// Have conditions loaded?
		/// </summary>
		public bool ConditionsLoaded { get; set; } = false;

		/// <summary>
		/// Load conditions
		/// </summary>
		public void LoadConditions()
		{
			try
			{
				//Clear conditions and active matches
				Conditions.Clear();
				Core.ActiveMatches.Clear();

				//Create conditions file if one doesnt exist
				if (!File.Exists("conditions.json"))
				{
					Core.Ui.WriteToConsole("No conditions file! Creating one...", Color.White);
					File.WriteAllText("conditions.json", "{\n}");
				}

				//Parse conditions file
				JObject? conditionJson;
				using (var fileStream = new FileStream("conditions.json", FileMode.Open))
				using (var reader = new StreamReader(fileStream))
				using (var jsonReader = new JsonTextReader(reader))
					conditionJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);

				if (conditionJson == null)
					return;

				//Iterate parsed conditions
				for (var conditionId = 0; conditionId < conditionJson.Count; conditionId++)
				{
					var condition = conditionJson[conditionId.ToString()];

					//Create condition and copy values
					var newCondition = new Condition
					{
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

					if (condition["emailProperty"] != null && !string.IsNullOrEmpty(condition["emailProperty"].ToString()))
					{
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

				ConditionsLoaded = true;

				//Try to clean up json parsing
				conditionJson.RemoveAll();

				//Save to file again in case some defaults were set
				var conditionsJson = JsonConvert.SerializeObject(Conditions, Formatting.Indented);
				File.WriteAllText("conditions.json", conditionsJson);

				//Update condition list
				Core.Ui.Invoke((MethodInvoker)(() => {
					Core.Ui.UpdateConditionList();
				}));
				Core.Ui.conditionTreeView.Nodes[0].Expand();

				//Log to UI
				Core.Ui.WriteToConsole("Conditions Loaded", Color.White);

				//Notify user if no conditions are found
				if (Conditions.Count == 0)
				{
					MessageBox.Show("No Conditions! Click Options then Open Condition Editor to add conditions.",
						"No Conditions!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}
	}
}