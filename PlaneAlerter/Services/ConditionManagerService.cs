using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PlaneAlerter.Enums;
using PlaneAlerter.Extensions;
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
		/// List of conditions being edited
		/// </summary>
		SortedDictionary<int, Condition> EditorConditions { get; set; }

		/// <summary>
		/// Save the conditions in the editor
		/// </summary>
		void SaveEditorConditions();

		/// <summary>
		/// Load conditions
		/// </summary>
		void LoadConditions();
	}

	internal class ConditionManagerService : IConditionManagerService
	{
		private readonly ILoggerWithQueue _logger;

		public ConditionManagerService(ILoggerWithQueue logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// List of current conditions
		/// </summary>
		public Dictionary<int, Condition> Conditions { get; set; } = new();

		/// <summary>
		/// List of conditions being edited
		/// </summary>
		public SortedDictionary<int, Condition> EditorConditions { get; set; } = new();

		/// <summary>
		/// Save the conditions in the editor
		/// </summary>
		public void SaveEditorConditions()
		{
			//Save conditions to file then close
			var conditionsJson =
				JsonConvert.SerializeObject(EditorConditions, new JsonSerializerSettings
				{
					Formatting = Formatting.Indented
				});
			File.WriteAllText("conditions.json", conditionsJson);
			EditorConditions.Clear();
		}

		/// <summary>
		/// Load conditions
		/// </summary>
		public void LoadConditions()
		{
			try
			{
				//Clear conditions and active matches
				Conditions.Clear();

				//Create conditions file if one doesn't exist
				if (!File.Exists("conditions.json"))
				{
					_logger.Log("No conditions file! Creating one...", Color.White);
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

					if (condition == null)
						throw new Exception($"Condition with id {conditionId} not found in conditions.json.\nConditions should have sequential ids.");

					//Create condition and copy values
					var newCondition = new Condition
					{
						Name = condition.RequiredValue<string>("conditionName", "Name"),
						AlertType = condition.RequiredValueStruct<AlertType>("alertType", "AlertType"),
						IgnoreFollowing = condition.RequiredValueStruct<bool>("ignoreFollowing", "IgnoreFollowing"),
						EmailEnabled = condition.OptionalValueStruct<bool>("emailEnabled", "EmailEnabled") ?? true,
						EmailFirstFormat = condition.OptionalValue<string>("emailFirstFormat", "EmailFirstFormat") ?? string.Empty,
						EmailLastFormat = condition.OptionalValue<string>("emailLastFormat", "EmailLastFormat") ?? string.Empty,
						TwitterEnabled = condition.OptionalValueStruct<bool>("twitterEnabled", "TwitterEnabled") ?? true,
						TwitterAccount = condition.OptionalValue<string>("twitterAccount", "TwitterAccount") ?? string.Empty,
						TweetFirstFormat = condition.OptionalValue<string>("tweetFirstFormat", "TweetFirstFormat") ?? string.Empty,
						TweetLastFormat = condition.OptionalValue<string>("tweetLastFormat", "TweetLastFormat") ?? string.Empty,
						TweetMap = condition.OptionalValueStruct<bool>("tweetMap", "TweetMap") ?? true,
						TweetLink = condition.OptionalValueStruct<TweetLink>("tweetLink", "TweetLink") ?? TweetLink.None,
						RecieverEmails = condition.RequiredValue<List<string>>("recieverEmails", "RecieverEmails"),
						Triggers = condition.RequiredValue<Dictionary<int, Trigger>>("triggers", "Triggers")
					};

					//Convert conditions using the email property system to use email formats
					var oldEmailProperty = condition.OptionalValueStruct<VrsProperty>("emailProperty");
					if (oldEmailProperty != null)
					{
						newCondition.EmailFirstFormat = "First Contact Alert! [ConditionName]: [" + VrsProperties.VrsPropertyData[oldEmailProperty.Value][2] + "]";
						newCondition.EmailLastFormat = "Last Contact Alert! [ConditionName]: [" + VrsProperties.VrsPropertyData[oldEmailProperty.Value][2] + "]";
					}

					//Add condition to list
					Conditions.Add(conditionId, newCondition);
				}

				//Save to file again in case some defaults were set
				var conditionsJson = JsonConvert.SerializeObject(Conditions, Formatting.Indented);
				File.WriteAllText("conditions.json", conditionsJson);

				//Log to UI
				_logger.Log("Conditions Loaded", Color.White);
			}
			catch (Exception e)
			{
				MessageBox.Show("Error loading conditions:\n\n" + e.Message + "\n\n" + e.StackTrace);
			}
		}
	}
}
