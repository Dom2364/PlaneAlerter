using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using Match = PlaneAlerter.Models.Match;

namespace PlaneAlerter.Services {
	internal interface ICheckerService
	{	
		void Start();
		void Stop();
	}

	/// <summary>
	/// Class for checking operations
	/// </summary>
	internal class CheckerService : ICheckerService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly IVrsService _vrsService;
		private readonly IEmailService _emailService;
		private readonly ITwitterService _twitterService;
		private readonly IUrlBuilderService _urlBuilderService;
		private readonly IStatsService _statsService;
		private readonly IStringFormatterService _stringFormatterService;

		/// <summary>
		/// Time of next check
		/// </summary>
		private DateTime _nextCheck;

		/// <summary>
		/// Stop the loop when it's suitable to do so
		/// </summary>
		private bool _stopping = false;

		/// <summary>
		/// Constructor
		/// </summary>
		public CheckerService(ISettingsManagerService settingsManagerService, IConditionManagerService conditionManagerService, IVrsService vrsService,
			IEmailService emailService, ITwitterService twitterService, IUrlBuilderService urlBuilderService, IStatsService statsService, IStringFormatterService stringFormatterService)
		{
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_vrsService = vrsService;
			_emailService = emailService;
			_twitterService = twitterService;
			_urlBuilderService = urlBuilderService;
			_statsService = statsService;
			_stringFormatterService = stringFormatterService;
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

			while (!_stopping) {
				//Set next check time
				_nextCheck = DateTime.Now.AddSeconds(_settingsManagerService.Settings.RefreshRate);
				//Notify user that aircraft info is being downloaded
				Core.Ui.UpdateStatusLabel("Downloading Aircraft Info...");
				receiverName = "";
				//Get latest aircraft information
				_vrsService.GetAircraft(false, true);
				if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS) _vrsService.GetAircraft(true, false);

				//Check if there are aircraft to check
				if (_vrsService.AircraftList.Count != 0) {
					//Aircraft number to be shown on UI
					var aircraftCount = 1;
					//Current condition being checked
					Condition condition;
					//Ignore following conditions for an aircraft
					var ignoreFollowing = false;
					//Updated trails are available, this check contains the first match in a while
					var updatedTrailsAvailable = false;
					foreach (var aircraft in _vrsService.AircraftList.ToList()) {
						//Update UI with aircraft being checked
						Core.Ui.UpdateStatusLabel("Checking conditions for aircraft " + aircraftCount + " of " +
						                          _vrsService.AircraftList.Count());
						aircraftCount++;

						if (Core.ActiveMatches.ContainsKey(aircraft.Icao) && Core.ActiveMatches[aircraft.Icao].IgnoreFollowing) continue;
						if (aircraft.GetProperty("Reg") == null && aircraft.GetProperty("Type") == null && (aircraft.GetProperty("FlightsCount") == null || Convert.ToInt32(aircraft.GetProperty("FlightsCount")) == 0)) 
							continue;

						//Iterate conditions
						foreach (var conditionId in _conditionManagerService.Conditions.Keys.ToList()) {
							condition = _conditionManagerService.Conditions[conditionId];
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
								if (_vrsService.Receivers.ContainsKey(aircraft.GetProperty("Rcvr")))
									receiverName = _vrsService.Receivers[aircraft.GetProperty("Rcvr")];

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
									_vrsService.GetAircraft(false, true, true);
									if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS) _vrsService.GetAircraft(true, false, true);
									updatedTrailsAvailable = true;
								}

								//If updated trails are available, update trail and position
								if (updatedTrailsAvailable) {
									foreach (var updatedAircraft in _vrsService.AircraftList.ToList())
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
						if (_stopping)
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
									if (_vrsService.Receivers.ContainsKey(aircraft.GetProperty("Rcvr")))
										receiverName = _vrsService.Receivers[aircraft.GetProperty("Rcvr")];

									//Send Alert
									SendAlert(condition, aircraft, receiverName, false);
								}
								break;
							}
							//Check if signal has been lost/returned
							var stillActive = false;
							foreach (var aircraft in _vrsService.AircraftList)
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
				if (_stopping) return;
				//Set thread status to waiting
				Core.Ui.UpdateStatusLabel("Waiting for next check...");
				//Wait until the next check time
				while (DateTime.Compare(DateTime.Now, _nextCheck) < 0) {
					//Cancel if thread is supposed to stop
					if (_stopping) return;
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
				content = _stringFormatterService.Format(content, aircraft, condition);
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
	}
}
