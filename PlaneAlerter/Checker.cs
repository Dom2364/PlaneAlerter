using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Media;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PlaneAlerter {
	/// <summary>
	/// Class for checking operations
	/// </summary>
	static class Checker {
		/// <summary>
		/// Client for sending aircraftlist.json requests
		/// </summary>
		static HttpWebRequest request;

		/// <summary>
		/// Time of next check
		/// </summary>
		static DateTime nextCheck;

		/// <summary>
		/// Have conditions loaded?
		/// </summary>
		public static bool ConditionsLoaded = false;

		/// <summary>
		/// How many checks ago were the trails requested
		/// </summary>
		private static int trailsAge = 1;

		/// <summary>
		/// Constructor
		/// </summary>
		public static void Start() {
			//Name of receiver that provided the aircraft information
			string recieverName;
			//Number of triggers matching for condition
			int triggersMatching;
			//VRS name of property
			string propertyInternalName;

			//Set culture to invariant
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			while (ThreadManager.threadStatus != ThreadManager.CheckerStatus.Stopping) {
				ThreadManager.threadStatus = ThreadManager.CheckerStatus.Running;
				//Set next check time
				nextCheck = DateTime.Now.AddSeconds(Settings.refreshRate);
				//Notify user that aircraft info is being downloaded
				Core.UI.updateStatusLabel("Downloading Aircraft Info...");
				recieverName = "";
				//Get latest aircraft information
				GetAircraft(false, true);
				if (Settings.filterDistance && !Settings.ignoreModeS) GetAircraft(true, false);

				//Check if there are aircraft to check
				if (Core.aircraftlist.Count != 0) {
					//Aircraft number to be shown on UI
					int aircraftCount = 1;
					//Current condition being checked
					Core.Condition condition;
					//Ignore following conditions for an aircraft
					bool ignorefollowing = false;
					//Updated trails are available, this check contains the first match in a while
					bool updatedtrailsavailable = false;
					foreach (Core.Aircraft aircraft in Core.aircraftlist.ToList()) {
						//Update UI with aircraft being checked
						Core.UI.updateStatusLabel("Checking conditions for aircraft " + aircraftCount + " of " + Core.aircraftlist.Count());
						aircraftCount++;

						if (Core.activeMatches.ContainsKey(aircraft.ICAO) && Core.activeMatches[aircraft.ICAO].IgnoreFollowing) continue;
						if (aircraft.GetProperty("Reg") == null && aircraft.GetProperty("Type") == null && (aircraft.GetProperty("FlightsCount") == null || Convert.ToInt32(aircraft.GetProperty("FlightsCount")) == 0)) 
							continue;

						//Iterate conditions
						foreach (int conditionid in Core.conditions.Keys.ToList()) {
							condition = Core.conditions[conditionid];
							//Skip if condition is disabled or condition is already matched
							if (condition.alertType == Core.AlertType.Disabled || (Core.activeMatches.ContainsKey(aircraft.ICAO) && Core.activeMatches[aircraft.ICAO].Conditions.Exists(x => x.ConditionID == conditionid)))
								continue;

							triggersMatching = 0;
							//Iterate triggers for condition
							foreach (Core.Trigger trigger in condition.triggers.Values) {
								//LEGEND
								//A = Equals/Not Equals
								//B = Higher Than + Lower Than
								//C = True/False Boolean
								//D = Starts With + Ends With
								//E = Contains

								//Get internal name for property to compare
								propertyInternalName = Core.vrsPropertyData[trigger.Property][2].ToString();
								string propertyValue = aircraft.GetProperty(propertyInternalName);

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
							if (triggersMatching == condition.triggers.Count) {
								//Get receiver name
								if (Core.receivers.ContainsKey(aircraft.GetProperty("Rcvr"))) recieverName = Core.receivers[aircraft.GetProperty("Rcvr")];

								//If active matches contains aircraft, add condition to the match
								if (Core.activeMatches.ContainsKey(aircraft.ICAO)) {
									Core.activeMatches[aircraft.ICAO].AddCondition(conditionid, condition, aircraft);
								}
								//Else add to active matches
								else {
									Core.Match m = new Core.Match(aircraft.ICAO);
									m.AddCondition(conditionid, condition, aircraft);
									Core.activeMatches.Add(aircraft.ICAO, m);
								}

								//Cancel checking for conditions for this aircraft
								ignorefollowing = condition.ignoreFollowing;

								//Get trails if they haven't been requested due to no matches
								if (Core.activeMatches.Count == 1 && Settings.trailsUpdateFrequency != 0) {
									GetAircraft(false, true, true);
									if (Settings.filterDistance && !Settings.ignoreModeS) GetAircraft(true, false, true);
									updatedtrailsavailable = true;
								}

								//If updated trails are available, update trail and position
								if (updatedtrailsavailable) {
									foreach (Core.Aircraft updatedac in Core.aircraftlist.ToList()) {
										if (updatedac.ICAO == aircraft.ICAO) {
											aircraft.Trail = updatedac.Trail;
											aircraft.SetProperty("Lat", updatedac.GetProperty("Lat"));
											aircraft.SetProperty("Long", updatedac.GetProperty("Long"));
											break;
										}
									}
								}

								//Update stats and log to console
								Stats.updateStats();
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | ADDED      | " + aircraft.ICAO + " | " + condition.conditionName, Color.LightGreen);

								//Send Alert
								if (condition.alertType == Core.AlertType.First_and_Last_Contact || condition.alertType == Core.AlertType.First_Contact)
									SendAlert(condition, aircraft, recieverName, true);
							}
							if (ignorefollowing) break;
						}
						//If active matches contains this aircraft, update aircraft info
						if (Core.activeMatches.ContainsKey(aircraft.ICAO))
							foreach (Core.MatchedCondition c in Core.activeMatches[aircraft.ICAO].Conditions)
								c.AircraftInfo = aircraft;

						//Cancel if thread is supposed to stop
						if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping)
							return;
					}
					//Check if aircraft have lost signal and remove aircraft that have timed out
					Core.UI.updateStatusLabel("Checking aircraft are still on radar...");
					//Iterate active matches
					foreach (Core.Match match in Core.activeMatches.Values.ToList()) {
						//Iterate match conditions
						foreach (Core.MatchedCondition c in match.Conditions) {
							//Check if signal has been lost for more than the removal timeout
							if (match.SignalLost && DateTime.Compare(match.SignalLostTime, DateTime.Now.AddSeconds((Settings.removalTimeout - (Settings.removalTimeout * 2)))) < 0) {
								//Remove from active matches
								Core.activeMatches.Remove(match.Icao);
								//Update stats and log to console
								Stats.updateStats();
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | REMOVING   | " + match.Icao + " | " + c.Condition.conditionName, Color.Orange);
								//Update aircraft info
								Core.Aircraft aircraft = c.AircraftInfo;

								//Alert if alert type is both or last
								if (c.Condition.alertType == Core.AlertType.First_and_Last_Contact || c.Condition.alertType == Core.AlertType.Last_Contact) {
									condition = c.Condition;

									//Get receiver name
									if (Core.receivers.ContainsKey(aircraft.GetProperty("Rcvr"))) recieverName = Core.receivers[aircraft.GetProperty("Rcvr")];

									//Send Alert
									SendAlert(condition, aircraft, recieverName, false);
								}
								break;
							}
							//Check if signal has been lost/returned
							bool stillActive = false;
							foreach (Core.Aircraft aircraft in Core.aircraftlist)
								if (aircraft.ICAO == match.Icao)
									stillActive = true;
							if (!stillActive && match.SignalLost == false) {
								Core.activeMatches[match.Icao].SignalLostTime = DateTime.Now;
								Core.activeMatches[match.Icao].SignalLost = true;
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | LOST SGNL  | " + match.Icao + " | " + match.Conditions[0].Condition.conditionName, Color.LightGoldenrodYellow);
							}
							if (stillActive && match.SignalLost == true) {
								Core.activeMatches[match.Icao].SignalLost = false;
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | RETND SGNL | " + match.Icao + " | " + match.Conditions[0].Condition.conditionName, Color.LightGoldenrodYellow);
							}
						}
					}
				}
				//Cancel if thread is supposed to stop
				if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping) return;
				//Set thread status to waiting
				Core.UI.updateStatusLabel("Waiting for next check...");
				ThreadManager.threadStatus = ThreadManager.CheckerStatus.Waiting;
				//Wait until the next check time
				while (DateTime.Compare(DateTime.Now, nextCheck) < 0) {
					//Cancel if thread is supposed to stop
					if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping) return;
					Thread.Sleep(1000);
				}
			}
		}

		public static void SendAlert(Core.Condition condition, Core.Aircraft aircraft, string receiver, bool isFirst) {
			//Show notification
			if (Settings.showNotifications) Core.UI.notifyIcon.ShowBalloonTip(5000, "Plane Alert!", $"Condition: {condition.conditionName}\nAircraft: {aircraft.GetProperty("Icao")} | {aircraft.GetProperty("Reg")} | {aircraft.GetProperty("Type")} | {aircraft.GetProperty("Call")}", ToolTipIcon.Info);

			//Make a ding noise
			if (Settings.soundAlerts) SystemSounds.Exclamation.Play();

			//Log
			Core.LogAlert(condition, aircraft, receiver, isFirst);

			if (condition.emailEnabled) {
				//Create email message
				MailMessage message = new MailMessage();
				string firstlast = isFirst ? "First" : "Last";

				//Set subject and email property info
				string emailPropertyInfo;
				if (aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString()) == null) {
					message.Subject = firstlast + " Contact Alert! " + condition.conditionName;
					emailPropertyInfo = condition.emailProperty.ToString() + ": No Value";
				}
				else {
					message.Subject = firstlast + " Contact Alert! " + condition.conditionName + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
					emailPropertyInfo = condition.emailProperty.ToString() + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
				}

				//Send emails
				foreach (string email in condition.recieverEmails) {
					Email.SendEmail(email, message, condition, aircraft, receiver, emailPropertyInfo, isFirst);
				}
			}
			
			if (condition.twitterEnabled) {
				string content = isFirst?condition.tweetFirstFormat:condition.tweetLastFormat;

				//Check if selected account is valid
				if (string.IsNullOrWhiteSpace(condition.twitterAccount)) {
					Core.UI.writeToConsole("ERROR: Please select Twitter account in condition editor", Color.Red);
					return;
				}
				if (!Settings.TwitterUsers.ContainsKey(condition.twitterAccount)) {
					Core.UI.writeToConsole("ERROR: Selected Twitter account (" + condition.twitterAccount + ") has not been authenticated", Color.Red);
					return;
				}
				if (string.IsNullOrEmpty(content)) {
					Core.UI.writeToConsole("ERROR: Tweet content can't be empty. Tweet content can be configured in the condition editor.", Color.Red);
					return;
				}

				//Get credentials
				string[] creds = Settings.TwitterUsers[condition.twitterAccount];

				//Replace keywords in content
				foreach(string[] info in Core.vrsPropertyData.Values) {
					//Check if content contains keyword
					if (content.ToLower().Contains(@"[" + info[2].ToLower() + @"]")) {
						//Replace keyword with value
						string value = aircraft.GetProperty(info[2]);
						if (string.IsNullOrEmpty(value)) value = "Unknown";
						content = Regex.Replace(content, @"\[" + info[2] + @"\]", value, RegexOptions.IgnoreCase);
					}
				}
				if (content.Length > 250) {
					int charsover = content.Length - 250;
					content = content.Substring(0, 250) + "...";
					Core.UI.writeToConsole("WARNING: Tweet content is " + charsover + " characters over the limit of 280, removing end of message", Color.Orange);
				}
					
				//Add link to tweet
				switch (condition.tweetLink) {
					case Core.TweetLink.None:
						break;
					case Core.TweetLink.Radar_link:
						content += " " + Settings.radarUrl;
						break;
					case Core.TweetLink.Radar_link_with_aircraft_selected:
						if (isFirst) content += " " + Settings.radarUrl + "?icao=" + aircraft.ICAO;
						break;
					case Core.TweetLink.Report_link:
						content += " " + Core.GenerateReportURL(aircraft.ICAO, true);
						break;
				}

				//Get map URL if enabled
				string mapURL = condition.tweetMap?Core.GenerateMapURL(aircraft) :"";

				//Send tweet
				bool success = Twitter.Tweet(creds[0], creds[1], content, mapURL).Result;
				if (success) {
					Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | TWEET      | " + aircraft.ICAO + " | " + condition.conditionName, Color.LightBlue);
				}
			}

			//Increase sent alerts for condition and update stats
			condition.increaseSentAlerts();
			Stats.updateStats();
		}

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		public static void GetAircraft(bool modeSOnly, bool clearExisting, bool forceRequestTrails = false) {
			bool requestTrails = Settings.trailsUpdateFrequency==1;

			//Force request trails
			if (forceRequestTrails) {
				requestTrails = true;
			}
			//No matches so we don't need trails
			else if (Core.activeMatches.Count == 0) {
				requestTrails = false;
			}
			//Threshold enabled
			else if (Settings.trailsUpdateFrequency >= 2) {
				if (trailsAge >= Settings.trailsUpdateFrequency) {
					requestTrails = true;
					trailsAge = 0;
				}
				trailsAge++;
			}

			//Generate aircraftlist.json url
			string url = Settings.acListUrl;
			url += Settings.acListUrl.Contains("?") ? "&" : "?";
			url += "lat=" + Settings.Lat.ToString() + "&lng=" + Settings.Long.ToString();
			if (Settings.filterDistance && !modeSOnly) url += "&fDstU=" + Settings.ignoreDistance.ToString("#.##");
			if (Settings.filterAltitude) url += "&fAltU=" + Settings.ignoreAltitude.ToString();
			if (Settings.filterReceiver) url += "&feed=" + Settings.filterReceiverId.ToString();
			if (modeSOnly) url += "&fNoPosQN=1";
			if (requestTrails) url += "&trFmt=fa&refreshTrails=1";

			try {
				JObject responseJson;
				try {
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e) {
					Core.UI.writeToConsole("ERROR: " + e.GetType().ToString() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null) {
					Core.UI.writeToConsole("ERROR: Invalid response recieved from server", Color.Red);
					return;
				}

				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Save old trails if not requesting new ones
				Dictionary<string, double[]> oldTrails = null;
				if (!requestTrails) oldTrails = Core.aircraftlist.ToDictionary(x => x.ICAO, x => x.Trail);

				//Parse aircraft data
				if (clearExisting) Core.aircraftlist.Clear();
				foreach (JObject a in responseJson["acList"].ToList()) {
					//Ignore if no icao is provided
					if (a["Icao"] == null) continue;
					//Create new aircraft
					Core.Aircraft aircraft = new Core.Aircraft(a["Icao"].ToString());

					//Parse aircraft trail
					if (requestTrails) {
						if (a["Cot"] != null)
							aircraft.Trail = new double[a["Cot"].Count()];
						for (int i = 0; i < aircraft.Trail.Length - 1; i++)
							if (a["Cot"][i].Value<string>() != null)
								aircraft.Trail[i] = double.Parse(a["Cot"][i].Value<string>(), CultureInfo.InvariantCulture);
							else
								aircraft.Trail[i] = 0;
					}
					else {
						if (oldTrails != null && oldTrails.ContainsKey(aircraft.ICAO)) aircraft.Trail = oldTrails[aircraft.ICAO];
					}

					//Parse aircraft properties
					List<JProperty> properties = a.Properties().ToList();
					for (int i = 0;i < properties.Count();i++)
						aircraft.AddProperty(properties[i].Name, properties[i].Value.ToString());
					properties = null;
					//Add aircraft to list
					Core.aircraftlist.Add(aircraft);
				}

				//Get list of receivers
				Core.receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Core.receivers.Add(f["id"].ToString(), f["name"].ToString());

				//Try to clean up json parsing
				responseJson.RemoveAll();
				responseJson = null;
				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException) {
				Core.UI.writeToConsole("ERROR: AircraftList.json url invalid (" + Settings.acListUrl + ")", Color.Red);
				return;
			}
			catch (InvalidDataException) {
				Core.UI.writeToConsole("ERROR: Data returned from " + Settings.acListUrl + " was not gzip compressed", Color.Red);
				return;
			}
			catch (WebException e) {
				Core.UI.writeToConsole("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return;
			}
			catch (JsonReaderException e) {
				Core.UI.writeToConsole("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return;
			}
		}

		private static JObject RequestAircraftList(string url) {
			//Create request
			request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			request.Timeout = Settings.timeout * 1000;
			//Add credentials if they are provided
			if (Settings.VRSAuthenticate) {
				string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Settings.VRSUsr + ":" + Settings.VRSPwd));
				request.Headers.Add("Authorization", "Basic " + encoded);
			}
			//Send request and parse json response
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream responsestream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(responsestream))
			using (JsonTextReader jsonreader = new JsonTextReader(reader))
				return JsonSerializer.Create().Deserialize<JObject>(jsonreader);
		}

		public static Dictionary<string, string> GetReceivers() {
			//Generate aircraftlist.json url
			string url = Settings.acListUrl;
			url += Settings.acListUrl.Contains("?") ? "&" : "?";
			url += "fUtQ=abc";

			try {
				JObject responseJson;
				try {
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e) {
					Core.UI.writeToConsole("ERROR: " + e.GetType().ToString() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return null;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null) {
					Core.UI.writeToConsole("ERROR: Invalid response recieved from server", Color.Red);
					return null;
				}
				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Get list of receivers
				Core.receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Core.receivers.Add(f["id"].ToString(), f["name"].ToString());

				//Try to clean up json parsing
				responseJson.RemoveAll();
				responseJson = null;
				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException) {
				Core.UI.writeToConsole("ERROR: AircraftList.json url invalid (" + Settings.acListUrl + ")", Color.Red);
				return null;
			}
			catch (InvalidDataException) {
				Core.UI.writeToConsole("ERROR: Data returned from " + Settings.acListUrl + " was not gzip compressed", Color.Red);
				return null;
			}
			catch (WebException e) {
				Core.UI.writeToConsole("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return null;
			}
			catch (JsonReaderException e) {
				Core.UI.writeToConsole("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return null;
			}

			return Core.receivers;
		}

		/// <summary>
		/// Load conditions
		/// </summary>
		public static void LoadConditions() {
			try {
				//Clear conditions and active matches
				Core.conditions.Clear();
				Core.activeMatches.Clear();
				//Parse conditions file
				JObject conditionJson;
				using (FileStream filestream = new FileStream("conditions.json", FileMode.Open))
					using (StreamReader reader = new StreamReader(filestream))
						using (JsonTextReader jsonreader = new JsonTextReader(reader))
							conditionJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
				if (conditionJson == null) return;
				//Iterate parsed conditions
				for (int conditionid = 0;conditionid < conditionJson.Count;conditionid++) {
					JToken condition = conditionJson[conditionid.ToString()];
					//Create condition and copy values
					Core.Condition newCondition = new Core.Condition {
						conditionName = condition["conditionName"].ToString(),
						alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString()),
						ignoreFollowing = (bool)condition["ignoreFollowing"],
						emailEnabled = (bool)(condition["emailEnabled"]??true),
						twitterEnabled = (bool)(condition["twitterEnabled"]??false),
						twitterAccount = (condition["twitterAccount"]??"").ToString(),
						tweetFirstFormat = (condition["tweetFirstFormat"]??"").ToString(),
						tweetLastFormat = (condition["tweetLastFormat"] ?? "").ToString(),
						tweetMap = (bool)(condition["tweetMap"] ?? true),
						tweetLink = (Core.TweetLink)Enum.Parse(typeof(Core.TweetLink), (condition["tweetLink"]??Core.TweetLink.None.ToString()).ToString()),
						emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), (condition["emailProperty"]??Core.vrsProperty.Registration.ToString()).ToString())
					};
					List<string> emailsArray = new List<string>();
					foreach (JToken email in condition["recieverEmails"])
						emailsArray.Add(email.ToString());
					newCondition.recieverEmails = emailsArray;
					foreach (JToken trigger in condition["triggers"].Values())
						newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger((Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));
					//Add condition to list
					Core.conditions.Add(conditionid, newCondition);
				}
				//Try to clean up json parsing
				conditionJson.RemoveAll();
				conditionJson = null;
				//Save to file again in case some defaults were set
				string conditionsJson = JsonConvert.SerializeObject(Core.conditions, Formatting.Indented);
				File.WriteAllText("conditions.json", conditionsJson);
				//Update condition list
				Core.UI.Invoke((MethodInvoker)(() => {
					Core.UI.updateConditionList();
				}));
				Core.UI.conditionTreeView.Nodes[0].Expand();
				//Log to UI
				Core.UI.writeToConsole("Conditions Loaded", Color.White);
				//Restart threads
				if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.WaitingForLoad) 
					ThreadManager.Restart();

			}
			catch (Exception e) {
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		static Checker() {
			//Create conditions file if one doesnt exist
			if (!File.Exists("conditions.json")) {
				Core.UI.writeToConsole("No conditions file! Creating one...", Color.White);
				File.WriteAllText("conditions.json", "{\n}");
			}
			//Load conditions
			LoadConditions();
			//Notify user if no conditions are found
			if (Core.conditions.Count == 0) {
				MessageBox.Show("No Conditions! Click Options then Open Condition Editor to add conditions.", "No Conditions!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			ConditionsLoaded = true;
		}
	}
}
