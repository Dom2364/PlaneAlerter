using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services {
	internal interface ICheckerService
	{
		/// <summary>
		/// Have conditions loaded?
		/// </summary>
		bool ConditionsLoaded { get; set; }
		
		void Start();
		void Stop();

		Dictionary<string, string>? GetReceivers();

		/// <summary>
		/// Load conditions
		/// </summary>
		void LoadConditions();
	}

	/// <summary>
	/// Class for checking operations
	/// </summary>
	internal class CheckerService : ICheckerService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IEmailService _emailService;
		private readonly ITwitterService _twitterService;
		private readonly IUrlBuilderService _urlBuilderService;
		private readonly IStatsService _statsService;
		private readonly IThreadManagerService _threadManagerService;

		/// <summary>
		/// Have conditions loaded?
		/// </summary>
		public bool ConditionsLoaded { get; set; } = false;

		/// <summary>
		/// Client for sending aircraftlist.json requests
		/// </summary>
		private HttpWebRequest _request;

		/// <summary>
		/// Time of next check
		/// </summary>
		private DateTime _nextCheck;

		/// <summary>
		/// How many checks ago were the trails requested
		/// </summary>
		private int _trailsAge = 1;

		/// <summary>
		/// Stop the loop when it's suitable to do so
		/// </summary>
		private bool _stopping = false;

		/// <summary>
		/// Constructor
		/// </summary>
		public CheckerService(ISettingsManagerService settingsManagerService, IEmailService emailService,
			ITwitterService twitterService, IUrlBuilderService urlBuilderService, IStatsService statsService,
			IThreadManagerService threadManagerService)
		{
			_settingsManagerService = settingsManagerService;
			_emailService = emailService;
			_twitterService = twitterService;
			_urlBuilderService = urlBuilderService;
			_statsService = statsService;
			_threadManagerService = threadManagerService;

			//Create conditions file if one doesnt exist
			if (!File.Exists("conditions.json"))
			{
				Core.Ui.WriteToConsole("No conditions file! Creating one...", Color.White);
				File.WriteAllText("conditions.json", "{\n}");
			}

			//Load conditions
			LoadConditions();
			//Notify user if no conditions are found
			if (Core.Conditions.Count == 0)
			{
				MessageBox.Show("No Conditions! Click Options then Open Condition Editor to add conditions.",
					"No Conditions!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			ConditionsLoaded = true;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public void Start() {
			//Name of receiver that provided the aircraft information
			string receiverName;
			//Number of triggers matching for condition
			int triggersMatching;
			//VRS name of property
			string propertyInternalName;

			//Set culture to invariant
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			while (_threadManagerService.ThreadStatus != CheckerStatus.Stopping) {
				//Set next check time
				_nextCheck = DateTime.Now.AddSeconds(_settingsManagerService.Settings.RefreshRate);
				//Notify user that aircraft info is being downloaded
				Core.Ui.UpdateStatusLabel("Downloading Aircraft Info...");
				receiverName = "";
				//Get latest aircraft information
				GetAircraft(false, true);
				if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS) GetAircraft(true, false);

				//Check if there are aircraft to check
				if (Core.AircraftList.Count != 0) {
					//Aircraft number to be shown on UI
					var aircraftCount = 1;
					//Current condition being checked
					Condition condition;
					//Ignore following conditions for an aircraft
					var ignoreFollowing = false;
					//Updated trails are available, this check contains the first match in a while
					var updatedTrailsAvailable = false;
					foreach (var aircraft in Core.AircraftList.ToList()) {
						//Update UI with aircraft being checked
						Core.Ui.UpdateStatusLabel("Checking conditions for aircraft " + aircraftCount + " of " + Core.AircraftList.Count());
						aircraftCount++;

						if (Core.ActiveMatches.ContainsKey(aircraft.Icao) && Core.ActiveMatches[aircraft.Icao].IgnoreFollowing) continue;
						if (aircraft.GetProperty("Reg") == null && aircraft.GetProperty("Type") == null && (aircraft.GetProperty("FlightsCount") == null || Convert.ToInt32(aircraft.GetProperty("FlightsCount")) == 0)) 
							continue;

						//Iterate conditions
						foreach (var conditionId in Core.Conditions.Keys.ToList()) {
							condition = Core.Conditions[conditionId];
							//Skip if condition is disabled or condition is already matched
							if (condition.AlertType == AlertType.Disabled || (Core.ActiveMatches.ContainsKey(aircraft.Icao) && Core.ActiveMatches[aircraft.Icao].Conditions.Exists(x => x.ConditionId == conditionId)))
								continue;

							triggersMatching = 0;
							//Iterate triggers for condition
							foreach (var trigger in condition.Triggers.Values) {
								//LEGEND
								//A = Equals/Not Equals
								//B = Higher Than + Lower Than
								//C = True/False Boolean
								//D = Starts With + Ends With
								//E = Contains

								//Get internal name for property to compare
								propertyInternalName = Core.VrsPropertyData[trigger.Property][2].ToString();
								var propertyValue = aircraft.GetProperty(propertyInternalName);

								//Check property against value
								if (trigger.ComparisonType == "Equals") {
									if (propertyValue == null) {
										if (trigger.Value == "") triggersMatching++;
									}
									else {
										if (propertyValue == trigger.Value) triggersMatching++;
									}
								}	
								else if (trigger.ComparisonType == "Not Equals") {
									if (propertyValue == null) {
										if (trigger.Value != "") triggersMatching++;
									}
									else {
										if (propertyValue != trigger.Value) triggersMatching++;
									}
								}
								//These comparisons must have a non-empty value to compare with
								else if (!string.IsNullOrEmpty(propertyValue)) {
									if (trigger.ComparisonType == "Contains" && propertyValue.Contains(trigger.Value))
										triggersMatching++;
									else if (trigger.ComparisonType == "Higher Than" && Convert.ToDouble(propertyValue) > Convert.ToDouble(trigger.Value))
										triggersMatching++;
									else if (trigger.ComparisonType == "Lower Than" && Convert.ToDouble(propertyValue) < Convert.ToDouble(trigger.Value))
										triggersMatching++;
									else if (trigger.ComparisonType == "Starts With" && (propertyValue.Length > trigger.Value.Length && propertyValue.Substring(0, trigger.Value.Length) == trigger.Value))
										triggersMatching++;
									else if (trigger.ComparisonType == "Ends With" && (propertyValue.Length > trigger.Value.Length && propertyValue.Substring(propertyValue.Length - trigger.Value.Length) == trigger.Value))
										triggersMatching++;
								}
							}

							//Check if condition still matches
							if (triggersMatching == condition.Triggers.Count) {
								//Get receiver name
								if (Core.Receivers.ContainsKey(aircraft.GetProperty("Rcvr"))) receiverName = Core.Receivers[aircraft.GetProperty("Rcvr")];

								//If active matches contains aircraft, add condition to the match
								if (Core.ActiveMatches.ContainsKey(aircraft.Icao)) {
									Core.ActiveMatches[aircraft.Icao].AddCondition(conditionId, condition, aircraft);
								}
								//Else add to active matches
								else {
									var m = new Match(aircraft.Icao);
									m.AddCondition(conditionId, condition, aircraft);
									Core.ActiveMatches.Add(aircraft.Icao, m);
								}

								//Cancel checking for conditions for this aircraft
								ignoreFollowing = condition.IgnoreFollowing;

								//Get trails if they haven't been requested due to no matches
								if (Core.ActiveMatches.Count == 1 && _settingsManagerService.Settings.TrailsUpdateFrequency != 0) {
									GetAircraft(false, true, true);
									if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS) GetAircraft(true, false, true);
									updatedTrailsAvailable = true;
								}

								//If updated trails are available, update trail and position
								if (updatedTrailsAvailable) {
									foreach (var updatedAircraft in Core.AircraftList.ToList())
									{
										if (updatedAircraft.Icao != aircraft.Icao)
											continue;

										aircraft.Trail = updatedAircraft.Trail;
										aircraft.SetProperty("Lat", updatedAircraft.GetProperty("Lat"));
										aircraft.SetProperty("Long", updatedAircraft.GetProperty("Long"));
										break;
									}
								}

								//Update stats and log to console
								_statsService.UpdateStats();
								Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | ADDED      | " + aircraft.Icao + " | " + condition.Name, Color.LightGreen);

								//Send Alert
								if (condition.AlertType == AlertType.First_and_Last_Contact || condition.AlertType == AlertType.First_Contact)
									SendAlert(condition, aircraft, receiverName, true);
							}
							if (ignoreFollowing) break;
						}
						//If active matches contains this aircraft, update aircraft info
						if (Core.ActiveMatches.ContainsKey(aircraft.Icao))
							foreach (var c in Core.ActiveMatches[aircraft.Icao].Conditions)
								c.AircraftInfo = aircraft;

						//Cancel if thread is supposed to stop
						if (_threadManagerService.ThreadStatus == CheckerStatus.Stopping)
							return;
					}
					//Check if aircraft have lost signal and remove aircraft that have timed out
					Core.Ui.UpdateStatusLabel("Checking aircraft are still on radar...");
					//Iterate active matches
					foreach (var match in Core.ActiveMatches.Values.ToList()) {
						//Iterate match conditions
						foreach (var c in match.Conditions) {
							//Check if signal has been lost for more than the removal timeout
							if (match.SignalLost && DateTime.Compare(match.SignalLostTime, DateTime.Now.AddSeconds((
								    _settingsManagerService.Settings.RemovalTimeout - (_settingsManagerService.Settings.RemovalTimeout * 2)))) < 0) {
								//Remove from active matches
								Core.ActiveMatches.Remove(match.Icao);
								//Update stats and log to console
								_statsService.UpdateStats();
								Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | REMOVING   | " + match.Icao + " | " + c.Condition.Name, Color.Orange);
								//Update aircraft info
								var aircraft = c.AircraftInfo;

								//Alert if alert type is both or last
								if (c.Condition.AlertType == AlertType.First_and_Last_Contact || c.Condition.AlertType ==
								    AlertType.Last_Contact) {
									condition = c.Condition;

									//Get receiver name
									if (Core.Receivers.ContainsKey(aircraft.GetProperty("Rcvr"))) receiverName = Core.Receivers[aircraft.GetProperty("Rcvr")];

									//Send Alert
									SendAlert(condition, aircraft, receiverName, false);
								}
								break;
							}
							//Check if signal has been lost/returned
							var stillActive = false;
							foreach (var aircraft in Core.AircraftList)
								if (aircraft.Icao == match.Icao)
									stillActive = true;
							if (!stillActive && match.SignalLost == false) {
								Core.ActiveMatches[match.Icao].SignalLostTime = DateTime.Now;
								Core.ActiveMatches[match.Icao].SignalLost = true;
								Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | LOST SGNL  | " + match.Icao + " | " + match.Conditions[0].Condition.Name, Color.LightGoldenrodYellow);
							}
							if (stillActive && match.SignalLost) {
								Core.ActiveMatches[match.Icao].SignalLost = false;
								Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | RETND SGNL | " + match.Icao + " | " + match.Conditions[0].Condition.Name, Color.LightGoldenrodYellow);
							}
						}
					}
				}
				//Cancel if thread is supposed to stop
				if (_threadManagerService.ThreadStatus == CheckerStatus.Stopping) return;
				//Set thread status to waiting
				Core.Ui.UpdateStatusLabel("Waiting for next check...");
				//Wait until the next check time
				while (DateTime.Compare(DateTime.Now, _nextCheck) < 0) {
					//Cancel if thread is supposed to stop
					if (_threadManagerService.ThreadStatus == CheckerStatus.Stopping) return;
					Thread.Sleep(1000);
				}
			}
		}

		public void Stop()
		{
			_stopping = true;
		}

		private void SendAlert(Condition condition, Aircraft aircraft, string receiver, bool isFirst) {
			//Show notification
			if (_settingsManagerService.Settings.ShowNotifications) Core.Ui.notifyIcon.ShowBalloonTip(5000, "Plane Alert!", $"Condition: {condition.Name}\nAircraft: {aircraft.GetProperty("Icao")} | {aircraft.GetProperty("Reg")} | {aircraft.GetProperty("Type")} | {aircraft.GetProperty("Call")}", ToolTipIcon.Info);

			//Make a ding noise
			if (_settingsManagerService.Settings.SoundAlerts) SystemSounds.Exclamation.Play();

			//Log
			Core.LogAlert(condition, aircraft, receiver, isFirst);

			if (condition.EmailEnabled) {
				//Send emails
				foreach (var email in condition.ReceiverEmails) {
					_emailService.SendEmail(email, condition, aircraft, receiver, isFirst);
				}
			}
			
			if (condition.TwitterEnabled) {
				var content = isFirst?condition.TweetFirstFormat:condition.TweetLastFormat;

				//Check if selected account is valid
				if (string.IsNullOrWhiteSpace(condition.TwitterAccount)) {
					Core.Ui.WriteToConsole("ERROR: Please select Twitter account in condition editor", Color.Red);
					return;
				}
				if (!_settingsManagerService.Settings.TwitterUsers.ContainsKey(condition.TwitterAccount)) {
					Core.Ui.WriteToConsole("ERROR: Selected Twitter account (" + condition.TwitterAccount + ") has not been authenticated", Color.Red);
					return;
				}
				if (string.IsNullOrEmpty(content)) {
					Core.Ui.WriteToConsole("ERROR: Tweet content can't be empty. Tweet content can be configured in the condition editor.", Color.Red);
					return;
				}

				//Get credentials
				var credentials = _settingsManagerService.Settings.TwitterUsers[condition.TwitterAccount];

				//Replace keywords in content
				content = Core.ParseCustomFormatString(content, aircraft, condition);
				if (content.Length > 250) {
					var charsOver = content.Length - 250;
					content = content.Substring(0, 250) + "...";
					Core.Ui.WriteToConsole("WARNING: Tweet content is " + charsOver + " characters over the limit of 280, removing end of message", Color.Orange);
				}
					
				//Add link to tweet
				switch (condition.TweetLink) {
					case TweetLink.None:
						break;
					case TweetLink.Radar_link:
						content += " " + _settingsManagerService.Settings.RadarUrl;
						break;
					case TweetLink.Radar_link_with_aircraft_selected:
						if (isFirst) content += " " + _settingsManagerService.Settings.RadarUrl + "?icao=" + aircraft.Icao;
						break;
					case TweetLink.Report_link:
						content += " " + _urlBuilderService.GenerateReportUrl(aircraft.Icao, true);
						break;
				}

				//Get map URL if enabled
				var mapUrl = condition.TweetMap ? _urlBuilderService.GenerateMapUrl(aircraft) : "";

				//Send tweet
				var success = _twitterService.Tweet(credentials[0], credentials[1], content, mapUrl).Result;
				if (success) {
					Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | TWEET      | " + aircraft.Icao + " | " + condition.Name, Color.LightBlue);
				}
			}

			//Increase sent alerts for condition and update stats
			condition.AlertsThisSession++;
			_statsService.TotalAlertsSent++;
			_statsService.UpdateStats();
		}

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		private void GetAircraft(bool modeSOnly, bool clearExisting, bool forceRequestTrails = false) {
			var requestTrails = _settingsManagerService.Settings.TrailsUpdateFrequency==1;

			//Force request trails
			if (forceRequestTrails) {
				requestTrails = true;
			}
			//No matches so we don't need trails
			else if (Core.ActiveMatches.Count == 0) {
				requestTrails = false;
			}
			//Threshold enabled
			else if (_settingsManagerService.Settings.TrailsUpdateFrequency >= 2) {
				if (_trailsAge >= _settingsManagerService.Settings.TrailsUpdateFrequency) {
					requestTrails = true;
					_trailsAge = 0;
				}
				_trailsAge++;
			}

			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains("?") ? "&" : "?";
			url += "lat=" + _settingsManagerService.Settings.Lat + "&lng=" + _settingsManagerService.Settings.Long;
			if (_settingsManagerService.Settings.FilterDistance && !modeSOnly) url += "&fDstU=" + _settingsManagerService.Settings.IgnoreDistance.ToString("#.##");
			if (_settingsManagerService.Settings.FilterAltitude) url += "&fAltU=" + _settingsManagerService.Settings.IgnoreAltitude;
			if (_settingsManagerService.Settings.FilterReceiver) url += "&feed=" + _settingsManagerService.Settings.FilterReceiverId;
			if (modeSOnly) url += "&fNoPosQN=1";
			if (requestTrails) url += "&trFmt=fa&refreshTrails=1";

			try {
				JObject responseJson;
				try {
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e) {
					Core.Ui.WriteToConsole("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null) {
					Core.Ui.WriteToConsole("ERROR: Invalid response received from server", Color.Red);
					return;
				}

				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Save old trails if not requesting new ones
				Dictionary<string, double[]>? oldTrails = null;
				if (!requestTrails) oldTrails = Core.AircraftList.ToDictionary(x => x.Icao, x => x.Trail);

				//Parse aircraft data
				if (clearExisting) Core.AircraftList.Clear();
				foreach (JObject a in responseJson["acList"].ToList()) {
					//Ignore if no icao is provided
					if (a["Icao"] == null) continue;
					//Create new aircraft
					var aircraft = new Aircraft(a["Icao"].ToString());

					//Parse aircraft trail
					if (requestTrails) {
						if (a["Cot"] != null)
							aircraft.Trail = new double[a["Cot"].Count()];
						for (var i = 0; i < aircraft.Trail.Length - 1; i++)
							if (a["Cot"][i].Value<string>() != null)
								aircraft.Trail[i] = double.Parse(a["Cot"][i].Value<string>(), CultureInfo.InvariantCulture);
							else
								aircraft.Trail[i] = 0;
					}
					else {
						if (oldTrails != null && oldTrails.ContainsKey(aircraft.Icao)) aircraft.Trail = oldTrails[aircraft.Icao];
					}

					//Parse aircraft properties
					var properties = a.Properties().ToList();
					for (var i = 0; i < properties.Count; i++)
						aircraft.AddProperty(properties[i].Name, properties[i].Value.ToString());
					
					//Add aircraft to list
					Core.AircraftList.Add(aircraft);
				}

				//Get list of receivers
				Core.Receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Core.Receivers.Add(f["id"].ToString(), f["name"].ToString());

				//Try to clean up json parsing
				responseJson.RemoveAll();
				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException) {
				Core.Ui.WriteToConsole("ERROR: AircraftList.json url invalid (" + _settingsManagerService.Settings.AircraftListUrl + ")", Color.Red);
				return;
			}
			catch (InvalidDataException) {
				Core.Ui.WriteToConsole("ERROR: Data returned from " + _settingsManagerService.Settings.AircraftListUrl + " was not gzip compressed", Color.Red);
				return;
			}
			catch (WebException e) {
				Core.Ui.WriteToConsole("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return;
			}
			catch (JsonReaderException e) {
				Core.Ui.WriteToConsole("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return;
			}
		}

		private JObject? RequestAircraftList(string url) {
			//Create request
			_request = (HttpWebRequest)WebRequest.Create(url);
			_request.Method = "GET";
			_request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			_request.Timeout = _settingsManagerService.Settings.Timeout * 1000;
			//Add credentials if they are provided
			if (_settingsManagerService.Settings.VRSAuthenticate) {
				var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(
					_settingsManagerService.Settings.VRSUser + ":" + _settingsManagerService.Settings.VRSPassword));
				_request.Headers.Add("Authorization", "Basic " + encoded);
			}
			//Send request and parse json response
			using var response = (HttpWebResponse)_request.GetResponse();
			using var responseStream = response.GetResponseStream();
			using var reader = new StreamReader(responseStream);
			using var jsonReader = new JsonTextReader(reader);

			return JsonSerializer.Create().Deserialize<JObject>(jsonReader);
		}

		public Dictionary<string, string>? GetReceivers() {
			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains("?") ? "&" : "?";
			url += "fUtQ=abc";

			try {
				JObject responseJson;
				try {
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e) {
					Core.Ui.WriteToConsole("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return null;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null) {
					Core.Ui.WriteToConsole("ERROR: Invalid response recieved from server", Color.Red);
					return null;
				}
				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Get list of receivers
				Core.Receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Core.Receivers.Add(f["id"].ToString(), f["name"].ToString());

				//Try to clean up json parsing
				responseJson.RemoveAll();

				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException) {
				Core.Ui.WriteToConsole("ERROR: AircraftList.json url invalid (" + _settingsManagerService.Settings.AircraftListUrl + ")", Color.Red);
				return null;
			}
			catch (InvalidDataException) {
				Core.Ui.WriteToConsole("ERROR: Data returned from " + _settingsManagerService.Settings.AircraftListUrl + " was not gzip compressed", Color.Red);
				return null;
			}
			catch (WebException e) {
				Core.Ui.WriteToConsole("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return null;
			}
			catch (JsonReaderException e) {
				Core.Ui.WriteToConsole("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return null;
			}

			return Core.Receivers;
		}

		/// <summary>
		/// Load conditions
		/// </summary>
		public void LoadConditions() {
			try {
				//Clear conditions and active matches
				Core.Conditions.Clear();
				Core.ActiveMatches.Clear();

				//Parse conditions file
				JObject? conditionJson;
				using (var fileStream = new FileStream("conditions.json", FileMode.Open))
					using (var reader = new StreamReader(fileStream))
						using (var jsonReader = new JsonTextReader(reader))
							conditionJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);

				if (conditionJson == null)
					return;

				//Iterate parsed conditions
				for (var conditionId = 0; conditionId < conditionJson.Count; conditionId++) {
					var condition = conditionJson[conditionId.ToString()];

					//Create condition and copy values
					var newCondition = new Condition {
						Name = condition["conditionName"].ToString(),
						AlertType = (AlertType)Enum.Parse(typeof(AlertType), condition["alertType"].ToString()),
						IgnoreFollowing = (bool)condition["ignoreFollowing"],
						EmailEnabled = (bool)(condition["emailEnabled"]??true),
						EmailFirstFormat = (condition["emailFirstFormat"] ?? "").ToString(),
						EmailLastFormat = (condition["emailLastFormat"] ?? "").ToString(),
						TwitterEnabled = (bool)(condition["twitterEnabled"]??false),
						TwitterAccount = (condition["twitterAccount"]??"").ToString(),
						TweetFirstFormat = (condition["tweetFirstFormat"]??"").ToString(),
						TweetLastFormat = (condition["tweetLastFormat"] ?? "").ToString(),
						TweetMap = (bool)(condition["tweetMap"] ?? true),
						TweetLink = (TweetLink)Enum.Parse(typeof(TweetLink), (condition["tweetLink"]??TweetLink.None.ToString()).ToString())
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
					Core.Conditions.Add(conditionId, newCondition);
				}
				//Try to clean up json parsing
				conditionJson.RemoveAll();
				
				//Save to file again in case some defaults were set
				var conditionsJson = JsonConvert.SerializeObject(Core.Conditions, Formatting.Indented);
				File.WriteAllText("conditions.json", conditionsJson);

				//Update condition list
				Core.Ui.Invoke((MethodInvoker)(() => {
					Core.Ui.UpdateConditionList();
				}));
				Core.Ui.conditionTreeView.Nodes[0].Expand();

				//Log to UI
				Core.Ui.WriteToConsole("Conditions Loaded", Color.White);

				//Restart threads
				if (_threadManagerService.ThreadStatus == CheckerStatus.WaitingForLoad)
					_threadManagerService.Restart();

			}
			catch (Exception e) {
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}
	}
}
