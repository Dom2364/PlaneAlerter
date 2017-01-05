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
	static class Checker {
		static HttpWebRequest request;
		static DateTime nextCheck;
		public static bool ConditionsLoaded = false;

		public static void Start() {
			string recieverName;
			int triggersMatching;
			string propertyInternalName;
			int wmIcaoIndex;
			MailMessage message;
			string emailPropertyInfo;

			while (ThreadManager.threadStatus != ThreadManager.CheckerStatus.Stopping) {
				ThreadManager.threadStatus = ThreadManager.CheckerStatus.Running;
				nextCheck = DateTime.Now.AddSeconds(Settings.refreshRate);
				Core.UI.updateStatusLabel("Downloading Aircraft Info...");
				recieverName = "";
				GetAircraft();

				int aircraftCount = 1;

				Core.Condition condition;
				foreach (Core.Aircraft aircraft in Core.aircraftlist.ToList()) {
					Core.UI.updateStatusLabel("Checking conditions for aircraft " + aircraftCount + " of " + Core.aircraftlist.Count());
					aircraftCount++;
					foreach (int conditionid in Core.conditions.Keys.ToList()) {
						condition = Core.conditions[conditionid];
						if (condition.alertType == Core.AlertType.Disabled || (Core.activeMatches.ContainsKey(aircraft.ICAO) && Core.activeMatches[aircraft.ICAO].Conditions.Exists(x => x.ConditionID == conditionid))) {
							continue;
						}

						triggersMatching = 0;
						foreach (Core.Trigger trigger in condition.triggers.Values) {
							//LEGEND
							//A = Equals/Not Equals
							//B = Higher Than + Lower Than
							//C = True/False Boolean
							//D = Starts With + Ends With
							//E = Contains
							propertyInternalName = Core.vrsPropertyData[trigger.Property][2].ToString();

							if (aircraft.GetProperty(propertyInternalName) == null)
								continue;
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
						wmIcaoIndex = -1;
						for (int i = 0;i < Core.waitingMatches.Count;i++) {
							if (Core.waitingMatches[i][0] == aircraft.ICAO && Core.waitingMatches[i][1] == conditionid.ToString()) {
								wmIcaoIndex = i;
								break;
							}
						}
						if (wmIcaoIndex == -1 && triggersMatching == condition.triggers.Count) {
							Core.waitingMatches.Add(new string[] { aircraft.ICAO, conditionid.ToString() });
							if (condition.ignoreFollowing)
								break;
							else
								continue;
						}
						if (wmIcaoIndex != -1 && Core.waitingMatches[wmIcaoIndex][1] == conditionid.ToString()) {
							Core.waitingMatches.RemoveAt(wmIcaoIndex);

							foreach (Core.Reciever reciever in Core.receivers) {
								if (reciever.Id == aircraft.GetProperty("Rcvr")) {
									recieverName = reciever.Name;
								}
							}

							message = new MailMessage();
							if (aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString()) == null) {
								message.Subject = "First Contact Alert! " + condition.conditionName;
								emailPropertyInfo = condition.emailProperty.ToString() + ": No Value";
							}
							else {
								message.Subject = "First Contact Alert! " + condition.conditionName + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
								emailPropertyInfo = condition.emailProperty.ToString() + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
							}

							if (Core.activeMatches.ContainsKey(aircraft.ICAO)) {
								Core.activeMatches[aircraft.ICAO].AddCondition(conditionid, condition, aircraft);
							}
							else {
								Core.Match m = new Core.Match(aircraft.ICAO);
								m.AddCondition(conditionid, condition, aircraft);
								Core.activeMatches.Add(aircraft.ICAO, m);
							}

							Stats.updateStats();
							Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | ADDED      | " + aircraft.ICAO + " | Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightGreen);

							if (condition.alertType == Core.AlertType.Both || condition.alertType == Core.AlertType.First) {
								foreach (string email in condition.recieverEmails) {
									Email.sendEmail(email, message, condition, aircraft, recieverName, emailPropertyInfo, true);
								}
							}
						}
					}
					if (Core.activeMatches.ContainsKey(aircraft.ICAO)) {
						foreach (Core.MatchedCondition c in Core.activeMatches[aircraft.ICAO].Conditions) {
							c.AircraftInfo = aircraft;
						}
					}
				}
				Core.UI.updateStatusLabel("Checking aircraft are still on radar...");
				foreach (Core.Match match in Core.activeMatches.Values.ToList()) {
					foreach (Core.MatchedCondition c in match.Conditions) {
						if (match.SignalLostTime != DateTime.MinValue && DateTime.Compare(match.SignalLostTime, DateTime.Now.AddSeconds((Settings.timeoutLength - (Settings.timeoutLength * 2)))) < 0) {
							Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | REMOVING   | " + match.Icao + " | " + c.DisplayName, Color.Orange);
							Core.activeMatches.Remove(match.Icao);
							Core.Aircraft aircraft = c.AircraftInfo;
							if (c.Match.alertType == Core.AlertType.Both || c.Match.alertType == Core.AlertType.Last) {
								message = new MailMessage();
								condition = c.Match;
								if (aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString()) == null) {
									message.Subject = "Last Contact Alert!  " + condition.conditionName;
									emailPropertyInfo = condition.emailProperty.ToString() + ": No Value";
								}
								else {
									message.Subject = "Last Contact Alert!  " + condition.conditionName + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
									emailPropertyInfo = condition.emailProperty.ToString() + ": " + aircraft.GetProperty(Core.vrsPropertyData[(Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition.emailProperty.ToString())][2].ToString());
								}
								SystemSounds.Exclamation.Play();
								Core.UI.notifyIcon.ShowBalloonTip(5000, "Plane Alert!", "Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")\nRegistration: " + aircraft.GetProperty("Reg"), ToolTipIcon.Info);
								foreach (string email in condition.recieverEmails) {
									Email.sendEmail(email, message, condition, aircraft, recieverName, emailPropertyInfo, false);
								}
							}
							break;
						}
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
				if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping) return;
				Core.UI.updateStatusLabel("Waiting for next check...");
				ThreadManager.threadStatus = ThreadManager.CheckerStatus.Waiting;
				while (DateTime.Compare(DateTime.Now, nextCheck) < 0) {
					if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Stopping) return;
					Thread.Sleep(1000);
				}
			}
		}

		public static void GetAircraft() {
			JObject responseJson;
			try {
				string url = "";
				if (!Settings.acListUrl.Contains("?"))
					url = Settings.acListUrl + "?trFmt=f&refreshTrails=1&lat=" + Settings.Lat + "&lng=" + Settings.Long;
				else
					url = Settings.acListUrl + "&trFmt=f&refreshTrails=1&lat=" + Settings.Lat + "&lng=" + Settings.Long;
				request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				if (Settings.VRSAuthenticate) {
					string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Settings.VRSUsr + ":" + Settings.VRSPwd));
					request.Headers.Add("Authorization", "Basic " + encoded);
				}
				request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
				request.Timeout = 30000;
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					using (Stream responsestream = response.GetResponseStream())
						using (StreamReader reader = new StreamReader(responsestream))
							using (JsonTextReader jsonreader = new JsonTextReader(reader))
								responseJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
				if (responseJson["acList"] == null) {
					Core.UI.writeToConsole("ERROR: Invalid response recieved from server", Color.Red);
					Thread.Sleep(5000);
					return;
				}
				if (responseJson["stm"] == null)
					throw new JsonReaderException();
				Core.aircraftlist.Clear();
				foreach (JObject a in responseJson["acList"].ToList()) {
					if (a["Icao"] == null) continue;
					Core.Aircraft aircraft = new Core.Aircraft(a["Icao"].ToString());
					if (a["Cot"] != null)
						aircraft.Trail = new double[a["Cot"].Count()];
					for (int i = 0;i < aircraft.Trail.Length - 1;i++)
						if (a["Cot"][i].Value<string>() != null)
							aircraft.Trail[i] = Convert.ToDouble(a["Cot"][i].Value<string>());
						else
							aircraft.Trail[i] = 0;
					List<JProperty> properties = a.Properties().ToList();
					for (int i = 0;i < properties.Count() - 1;i++)
						aircraft.AddProperty(properties[i].Name, properties[i].Value.ToString());
					properties = null;
					Core.aircraftlist.Add(aircraft);
				}
				Core.receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Core.receivers.Add(new Core.Reciever(f["id"].ToString(), f["name"].ToString()));
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

		public static void LoadConditions() {
			try {
				Core.conditions.Clear();
				Core.activeMatches.Clear();
				JObject conditionJson;
				using (FileStream filestream = new FileStream("conditions.json", FileMode.Open))
					using (StreamReader reader = new StreamReader(filestream))
						using (JsonTextReader jsonreader = new JsonTextReader(reader))
							conditionJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
				for (int conditionid = 0;conditionid < conditionJson.Count;conditionid++) {
					JToken condition = conditionJson[conditionid.ToString()];
					Core.Condition newCondition = new Core.Condition();
					newCondition.conditionName = condition["conditionName"].ToString();
					newCondition.emailProperty = (Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), condition["emailProperty"].ToString());
					newCondition.alertType = (Core.AlertType)Enum.Parse(typeof(Core.AlertType), condition["alertType"].ToString());
					newCondition.ignoreFollowing = (bool)condition["ignoreFollowing"];
					List<string> emailsArray = new List<string>();
					foreach (JToken email in condition["recieverEmails"]) {
						emailsArray.Add(email.ToString());
					}
					newCondition.recieverEmails = emailsArray;
					foreach (JToken trigger in condition["triggers"].Values()) {
						newCondition.triggers.Add(newCondition.triggers.Count, new Core.Trigger((Core.vrsProperty)Enum.Parse(typeof(Core.vrsProperty), trigger["Property"].ToString()), trigger["Value"].ToString(), trigger["ComparisonType"].ToString()));
					}
					Core.conditions.Add(conditionid, newCondition);
				}
				conditionJson.RemoveAll();
				conditionJson = null;
				Core.UI.Invoke((MethodInvoker)(() => {
					Core.UI.updateConditionList();
				}));
				Core.UI.conditionTreeView.Nodes[0].Expand();
				Core.UI.writeToConsole("Conditions Loaded", Color.White);

				if (ThreadManager.threadStatus == ThreadManager.CheckerStatus.Waiting || ThreadManager.threadStatus == ThreadManager.CheckerStatus.Running)
					ThreadManager.Restart();
			}
			catch (Exception e) {
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}

		static Checker() {
			if (!File.Exists("conditions.json")) {
				Core.UI.writeToConsole("No conditions file! Creating one...", Color.White);
				File.WriteAllText("conditions.json", "{\n}");
			}
			LoadConditions();
			if (Core.conditions.Count == 0) {
				MessageBox.Show("No Conditions! Click Options then Open Condition Editor to add conditions.", "No Conditions!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			ConditionsLoaded = true;
		}
	}
}
