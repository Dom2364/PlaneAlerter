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
using System.Threading;
using System.Windows.Forms;

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
		/// Constructor
		/// </summary>
		public static void Start() {
			//Name of receiver that provided the aircraft information
			string recieverName;
			//Number of triggers matching for condition
			int triggersMatching;
			//VRS name of property
			string propertyInternalName;
			//Index of match in waiting matches
			int wmIcaoIndex;
			//Alert message to send
			MailMessage message;
			//Text to display email property value
			string emailPropertyInfo;

			while (ThreadManager.threadStatus != ThreadManager.CheckerStatus.Stopping) {
				ThreadManager.threadStatus = ThreadManager.CheckerStatus.Running;
				//Set next check time
				nextCheck = DateTime.Now.AddSeconds(Settings.refreshRate);
				//Notify user that aircraft info is being downloaded
				Core.UI.updateStatusLabel("Downloading Aircraft Info...");
				recieverName = "";
				//Get latest aircraft information
				GetAircraft();

				//Check if there are aircraft to check
				if (Core.aircraftlist.Count != 0) {
					//Aircraft number to be shown on UI
					int aircraftCount = 1;
					//Current condition being checked
					Core.Condition condition;
					foreach (Core.Aircraft aircraft in Core.aircraftlist.ToList()) {
						//Update UI with aircraft being checked
						Core.UI.updateStatusLabel("Checking conditions for aircraft " + aircraftCount + " of " + Core.aircraftlist.Count());
						aircraftCount++;

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

								//If aircraft properties do not contain property, skip
								if (aircraft.GetProperty(propertyInternalName) == null)
									continue;
								//Check property against value
								if (trigger.ComparisonType == "Equals" && aircraft.GetProperty(propertyInternalName) == trigger.Value)
									triggersMatching++;
								if (trigger.ComparisonType == "Not Equals" && aircraft.GetProperty(propertyInternalName) != trigger.Value)
									triggersMatching++;
								if (trigger.ComparisonType == "Contains" && aircraft.GetProperty(propertyInternalName).Contains(trigger.Value))
									triggersMatching++;
								if (trigger.ComparisonType == "Higher Than" && Convert.ToDouble(aircraft.GetProperty(propertyInternalName)) > Convert.ToDouble(trigger.Value))
									triggersMatching++;
								if (trigger.ComparisonType == "Lower Than" && Convert.ToDouble(aircraft.GetProperty(propertyInternalName)) < Convert.ToDouble(trigger.Value))
									triggersMatching++;
								if (trigger.ComparisonType == "Starts With" && (aircraft.GetProperty(propertyInternalName).Length > trigger.Value.Length && aircraft.GetProperty(propertyInternalName).Substring(0, trigger.Value.Length) == trigger.Value))
									triggersMatching++;
								if (trigger.ComparisonType == "Ends With" && (aircraft.GetProperty(propertyInternalName).ToString().Length > trigger.Value.Length && aircraft.GetProperty(propertyInternalName).Substring(aircraft.GetProperty(propertyInternalName).Length - trigger.Value.Length) == trigger.Value))
									triggersMatching++;
							}

							//Get position in waiting matches
							wmIcaoIndex = -1;
							for (int i = 0; i < Core.waitingMatches.Count; i++) {
								if (Core.waitingMatches[i][0] == aircraft.ICAO && Core.waitingMatches[i][1] == conditionid.ToString()) {
									wmIcaoIndex = i;
									break;
								}
							}
							//If condition doesn't exist in waiting matches and all triggers match, add to waiting matches
							if (wmIcaoIndex == -1 && triggersMatching == condition.triggers.Count) {
								Core.waitingMatches.Add(new string[] { aircraft.ICAO, conditionid.ToString() });
								//If ignore following is true, skip the following conditions
								if (condition.ignoreFollowing)
									break;
								else
									continue;
							}
							//If condition exists in waiting matches with this condition id, remove from waiting matches and send alert
							if (wmIcaoIndex != -1 && Core.waitingMatches[wmIcaoIndex][1] == conditionid.ToString()) {
								Core.waitingMatches.RemoveAt(wmIcaoIndex);

								//Get receiver name
								foreach (Core.Reciever reciever in Core.receivers)
									if (reciever.Id == aircraft.GetProperty("Rcvr"))
										recieverName = reciever.Name;

								//Create email message
								message = new MailMessage();
								//Set subject and email property info
								if (aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString()) == null) {
									message.Subject = "First Contact Alert! " + condition.conditionName;
									emailPropertyInfo = condition.emailProperty.ToString() + ": No Value";
								}
								else {
									message.Subject = "First Contact Alert! " + condition.conditionName + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
									emailPropertyInfo = condition.emailProperty.ToString() + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
								}
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

								//Update stats and log to console
								Stats.updateStats();
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | ADDED      | " + aircraft.ICAO + " | Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightGreen);

								//Send alert to emails in condition
								if (condition.alertType == Core.AlertType.Both || condition.alertType == Core.AlertType.First)
									foreach (string email in condition.recieverEmails)
										Email.SendEmail(email, message, condition, aircraft, recieverName, emailPropertyInfo, true);
							}
						}
						//If active matches contains this aircraft, update aircraft info
						if (Core.activeMatches.ContainsKey(aircraft.ICAO))
							foreach (Core.MatchedCondition c in Core.activeMatches[aircraft.ICAO].Conditions)
								c.AircraftInfo = aircraft;
					}
					//Check if aircraft have lost signal and remove aircraft that have timed out
					Core.UI.updateStatusLabel("Checking aircraft are still on radar...");
					//Iterate active matches
					foreach (Core.Match match in Core.activeMatches.Values.ToList()) {
						//Iterate match conditions
						foreach (Core.MatchedCondition c in match.Conditions) {
							//Check if signal has been lost for more than the removal timeout
							if (match.SignalLostTime != DateTime.MinValue && DateTime.Compare(match.SignalLostTime, DateTime.Now.AddSeconds((Settings.removalTimeout - (Settings.removalTimeout * 2)))) < 0) {
								//Log to UI
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | REMOVING   | " + match.Icao + " | " + c.DisplayName, Color.Orange);
								//Remove from active matches
								Core.activeMatches.Remove(match.Icao);
								//Update aircraft info
								Core.Aircraft aircraft = c.AircraftInfo;
								//Alert if alert type is both or last
								if (c.Match.alertType == Core.AlertType.Both || c.Match.alertType == Core.AlertType.Last) {
									condition = c.Match;

									//Create new email message
									message = new MailMessage();

									//Set email subject and email property info
									if (aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString()) == null) {
										message.Subject = "Last Contact Alert!  " + condition.conditionName;
										emailPropertyInfo = condition.emailProperty.ToString() + ": No Value";
									}
									else {
										message.Subject = "Last Contact Alert!  " + condition.conditionName + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
										emailPropertyInfo = condition.emailProperty.ToString() + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
									}
									//Make a ding noise or something
									SystemSounds.Exclamation.Play();
									//Show notification
									Core.UI.notifyIcon.ShowBalloonTip(5000, "Plane Alert!", "Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")\nRegistration: " + aircraft.GetProperty("Reg"), ToolTipIcon.Info);
									//Send alerts to all the emails
									foreach (string email in condition.recieverEmails)
										Email.SendEmail(email, message, condition, aircraft, recieverName, emailPropertyInfo, false);
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
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | LOST SGNL  | " + match.Icao + " | " + match.DisplayName, Color.LightGoldenrodYellow);
							}
							if (stillActive && match.SignalLost == true) {
								Core.activeMatches[match.Icao].SignalLost = false;
								Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | RETND SGNL | " + match.Icao + " | " + match.DisplayName, Color.LightGoldenrodYellow);
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

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		public static void GetAircraft() {
			JObject responseJson;
			try {
				//Generate aircraftlist.json url
				string url = "";
				if (!Settings.acListUrl.Contains("?"))
					url = Settings.acListUrl + "?trFmt=f&refreshTrails=1&lat=" + Settings.Lat + "&lng=" + Settings.Long;
				else
					url = Settings.acListUrl + "&trFmt=f&refreshTrails=1&lat=" + Settings.Lat + "&lng=" + Settings.Long;
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
								responseJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
				//Check if we actually got aircraft data
				if (responseJson["acList"] == null) {
					Core.UI.writeToConsole("ERROR: Invalid response recieved from server", Color.Red);
					Thread.Sleep(5000);
					return;
				}
				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();
				//Parse aircraft data
				Core.aircraftlist.Clear();
				foreach (JObject a in responseJson["acList"].ToList()) {
					//Ignore if no icao is provided
					if (a["Icao"] == null) continue;
					//Create new aircraft
					Core.Aircraft aircraft = new Core.Aircraft(a["Icao"].ToString());
					//Parse aircraft trail
					if (a["Cot"] != null)
						aircraft.Trail = new double[a["Cot"].Count()];
					for (int i = 0;i < aircraft.Trail.Length - 1;i++)
						if (a["Cot"][i].Value<string>() != null)
							aircraft.Trail[i] = Convert.ToDouble(a["Cot"][i].Value<string>());
						else
							aircraft.Trail[i] = 0;
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
					Core.receivers.Add(new Core.Reciever(f["id"].ToString(), f["name"].ToString()));
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
						emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition["emailProperty"].ToString()),
						alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString()),
						ignoreFollowing = (bool)condition["ignoreFollowing"]
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
				//Update condition list
				Core.UI.Invoke((MethodInvoker)(() => {
					Core.UI.updateConditionList();
				}));
				Core.UI.conditionTreeView.Nodes[0].Expand();
				//Log to UI
				Core.UI.writeToConsole("Conditions Loaded", Color.White);
				//Restart threads if they are not stopping/stopped
				if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Waiting || ThreadManager.threadStatus == ThreadManager.CheckerStatus.Running)
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
