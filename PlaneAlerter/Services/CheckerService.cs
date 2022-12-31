using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using Match = PlaneAlerter.Models.Match;

namespace PlaneAlerter.Services {
	internal interface ICheckerService
	{
		Dictionary<string, Match> ActiveMatches { get; set; }
		delegate void AlertSentEventHandler(Condition condition, Aircraft aircraft, string receiver, bool isFirst);
		event AlertSentEventHandler SendingAlert;
		event EventHandler<string> StatusChanged;
		void Start();
		void Stop();
	}

	/// <summary>
	/// Class for checking operations
	/// </summary>
	internal class CheckerService : ICheckerService
	{

		/// <summary>
		/// List of current matches
		/// </summary>
		public Dictionary<string, Match> ActiveMatches { get; set; } = new Dictionary<string, Match>();

		public event ICheckerService.AlertSentEventHandler SendingAlert;

		public event EventHandler<string> StatusChanged;

		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly IVrsService _vrsService;
		private readonly IEmailService _emailService;
		private readonly ITwitterService _twitterService;
		private readonly IUrlBuilderService _urlBuilderService;
		private readonly IStatsService _statsService;
		private readonly IStringFormatterService _stringFormatterService;
		private readonly ILoggerWithQueue _logger;

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
			IEmailService emailService, ITwitterService twitterService, IUrlBuilderService urlBuilderService, IStatsService statsService,
			IStringFormatterService stringFormatterService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_vrsService = vrsService;
			_emailService = emailService;
			_twitterService = twitterService;
			_urlBuilderService = urlBuilderService;
			_statsService = statsService;
			_stringFormatterService = stringFormatterService;
			_logger = logger;
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
				StatusChanged?.Invoke(this, "Downloading Aircraft Info...");
				receiverName = "";
				//Get latest aircraft information
				_vrsService.GetAircraft(false, true, ActiveMatches.Count == 0);
				if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS)
					_vrsService.GetAircraft(true, false, ActiveMatches.Count == 0);

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
						StatusChanged?.Invoke(this, $"Checking conditions for aircraft {aircraftCount} of {_vrsService.AircraftList.Count}");
						aircraftCount++;

						if (ActiveMatches.ContainsKey(aircraft.Icao) && ActiveMatches[aircraft.Icao].IgnoreFollowing) continue;
						if (aircraft.GetProperty("Reg") == null && aircraft.GetProperty("Type") == null && (aircraft.GetProperty("FlightsCount") == null || Convert.ToInt32(aircraft.GetProperty("FlightsCount")) == 0)) 
							continue;

						//Iterate conditions
						foreach (var conditionId in _conditionManagerService.Conditions.Keys.ToList()) {
							condition = _conditionManagerService.Conditions[conditionId];
							//Skip if condition is disabled or condition is already matched
							if (condition.AlertType == AlertType.Disabled || (ActiveMatches.ContainsKey(aircraft.Icao) && ActiveMatches[aircraft.Icao].Conditions.Exists(x => x.ConditionId == conditionId)))
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
								if (ActiveMatches.ContainsKey(aircraft.Icao)) {
									ActiveMatches[aircraft.Icao].AddCondition(conditionId, condition, aircraft);
								}
								//Else add to active matches
								else {
									var m = new Match(aircraft.Icao);
									m.AddCondition(conditionId, condition, aircraft);
									ActiveMatches.Add(aircraft.Icao, m);
								}

								//Cancel checking for conditions for this aircraft
								ignoreFollowing = condition.IgnoreFollowing;

								//Get trails if they haven't been requested due to no matches
								if (ActiveMatches.Count == 1 && _settingsManagerService.Settings.TrailsUpdateFrequency != 0) {
									_vrsService.GetAircraft(false, true, false, true);
									if (_settingsManagerService.Settings.FilterDistance && !_settingsManagerService.Settings.IgnoreModeS)
										_vrsService.GetAircraft(true, false, false, true);
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

								//Log to console
								_logger.Log(DateTime.Now.ToLongTimeString() + " | ADDED      | " + aircraft.Icao + " | " + condition.Name, Color.LightGreen);

								//Send Alert
								if (condition.AlertType == AlertType.First_and_Last_Contact || condition.AlertType == AlertType.First_Contact)
									SendAlert(condition, aircraft, receiverName, true);
							}
							if (ignoreFollowing) break;
						}
						//If active matches contains this aircraft, update aircraft info
						if (ActiveMatches.ContainsKey(aircraft.Icao))
							foreach (var c in ActiveMatches[aircraft.Icao].Conditions)
								c.AircraftInfo = aircraft;

						//Cancel if thread is supposed to stop
						if (_stopping)
							return;
					}
					
					//Check if aircraft have lost signal and remove aircraft that have timed out
					StatusChanged?.Invoke(this, "Checking aircraft are still on radar...");

					//Iterate active matches
					foreach (var match in ActiveMatches.Values.ToList()) {
						//Iterate match conditions
						foreach (var c in match.Conditions) {
							//Check if signal has been lost for more than the removal timeout
							if (match.SignalLost && DateTime.Compare(match.SignalLostTime, DateTime.Now.AddSeconds((
								    _settingsManagerService.Settings.RemovalTimeout - (_settingsManagerService.Settings.RemovalTimeout * 2)))) < 0) {
								//Remove from active matches
								ActiveMatches.Remove(match.Icao);
								//Log to console
								_logger.Log(DateTime.Now.ToLongTimeString() + " | REMOVING   | " + match.Icao + " | " + c.Condition.Name, Color.Orange);
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
								ActiveMatches[match.Icao].SignalLostTime = DateTime.Now;
								ActiveMatches[match.Icao].SignalLost = true;
								_logger.Log(DateTime.Now.ToLongTimeString() + " | LOST SGNL  | " + match.Icao + " | " + match.Conditions[0].Condition.Name, Color.LightGoldenrodYellow);
							}
							if (stillActive && match.SignalLost) {
								ActiveMatches[match.Icao].SignalLost = false;
								_logger.Log(DateTime.Now.ToLongTimeString() + " | RETND SGNL | " + match.Icao + " | " + match.Conditions[0].Condition.Name, Color.LightGoldenrodYellow);
							}
						}
					}
				}

				//Cancel if thread is supposed to stop
				if (_stopping) return;

				//Set thread status to waiting
				StatusChanged?.Invoke(this, "Waiting for next check...");

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
			SendingAlert?.Invoke(condition, aircraft, receiver, isFirst);

			//Make a ding noise
			if (_settingsManagerService.Settings.SoundAlerts) SystemSounds.Exclamation.Play();

			//Log
			LogAlert(condition, aircraft, receiver, isFirst);

			if (condition.EmailEnabled) {
				//Send emails
				foreach (var email in condition.RecieverEmails) {
					_emailService.SendEmail(email, condition, aircraft, receiver, isFirst);
				}
			}
			
			if (condition.TwitterEnabled) {
				var content = isFirst?condition.TweetFirstFormat:condition.TweetLastFormat;

				//Check if selected account is valid
				if (string.IsNullOrWhiteSpace(condition.TwitterAccount)) {
					_logger.Log("ERROR: Please select Twitter account in condition editor", Color.Red);
					return;
				}
				if (!_settingsManagerService.Settings.TwitterUsers.ContainsKey(condition.TwitterAccount)) {
					_logger.Log("ERROR: Selected Twitter account (" + condition.TwitterAccount + ") has not been authenticated", Color.Red);
					return;
				}
				if (string.IsNullOrEmpty(content)) {
					_logger.Log("ERROR: Tweet content can't be empty. Tweet content can be configured in the condition editor.", Color.Red);
					return;
				}

				//Get credentials
				var credentials = _settingsManagerService.Settings.TwitterUsers[condition.TwitterAccount];

				//Replace keywords in content
				content = _stringFormatterService.Format(content, aircraft, condition);
				if (content.Length > 250) {
					var charsOver = content.Length - 250;
					content = content.Substring(0, 250) + "...";
					_logger.Log("WARNING: Tweet content is " + charsOver + " characters over the limit of 280, removing end of message", Color.Orange);
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
					_logger.Log(DateTime.Now.ToLongTimeString() + " | TWEET      | " + aircraft.Icao + " | " + condition.Name, Color.LightBlue);
				}
			}

			//Increase sent alerts for condition and update stats
			condition.AlertsThisSession++;
			_statsService.TotalAlertsSent++;
		}

		public void LogAlert(Condition condition, Aircraft aircraft, string receiver, bool isFirst)
		{
			try
			{
				var message =
					$"{DateTime.Now.ToString("yyyy/MM/dd HH:mm")} | {condition.Name} | {receiver} | {(isFirst ? "FIRST" : "LAST")} CONTACT ALERT: " +
					Environment.NewLine;
				message += aircraft.ToJson() + Environment.NewLine + Environment.NewLine;

				File.AppendAllText("alerts.log", message);
			}
			catch (Exception e)
			{
				_logger.Log("ERROR: Error writing to alerts.log file: " + e.Message, Color.Red);
			}
		}
	}
}
