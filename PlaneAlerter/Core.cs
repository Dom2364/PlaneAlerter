using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlaneAlerter {
	/// <summary>
	/// Important variables and things
	/// </summary>
	public static class Core {
		/// <summary>
		/// List of current conditions
		/// </summary>
		public static Dictionary<int, Condition> Conditions { get; set; } = new Dictionary<int, Condition>();

		/// <summary>
		/// List of current aircraft
		/// </summary>
		public static List<Aircraft> AircraftList { get; set; } = new List<Aircraft>();

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		public static Dictionary<string, string> Receivers { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// VRS property descriptions
		/// </summary>
		public static Dictionary<vrsProperty, string[]> VrsPropertyData { get; set; } = new Dictionary<vrsProperty, string[]>();

		/// <summary>
		/// Types of comparisons usable with triggers
		/// </summary>
		public static Dictionary<string, string[]> ComparisonTypes { get; set; } = new Dictionary<string, string[]>();

		/// <summary>
		/// Properties shown when essential properties selected
		/// </summary>
		public static List<vrsProperty> EssentialProperties { get; set; } = new List<vrsProperty>();

		/// <summary>
		/// List of current matches
		/// </summary>
		public static Dictionary<string, Match> ActiveMatches { get; set; } = new Dictionary<string, Match>();

		/// <summary>
		/// Thread for checking operations
		/// </summary>
		public static Thread LoopThread { get; set; }

		/// <summary>
		/// Thread for updating statistics
		/// </summary>
		public static Thread StatsThread { get; set; }

		/// <summary>
		/// Active form
		/// </summary>
		public static Forms.PlaneAlerter Ui { get; set; }

		/// <summary>
		/// Class to store aircraft information
		/// </summary>
		public class Aircraft {
			/// <summary>
			/// List of property values retrieved from last aircraftlist.json
			/// </summary>
			private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();

			/// <summary>
			/// ICAO hex of aircraft
			/// </summary>
			public string Icao { get; set; }

			/// <summary>
			/// Position track from last aircraftlist.json
			/// </summary>
			public double[] Trail { get; set; } = Array.Empty<double>();

			/// <summary>
			/// Get property value from property list
			/// </summary>
			/// <param name="key">Property name</param>
			/// <returns>Value of property</returns>
			public string? GetProperty(string key)
			{
				return _properties.ContainsKey(key) ? _properties[key] : null;
			}

			/// <summary>
			/// Set property value
			/// </summary>
			/// <param name="key">Property name</param>
			/// <param name="value">Value</param>
			public void SetProperty(string key, string value) {
				if (_properties.ContainsKey(key))
					_properties[key] = value;
			}

			/// <summary>
			/// Add a property to the list
			/// </summary>
			/// <param name="key">Key of property</param>
			/// <param name="value">Value of property</param>
			public void AddProperty(string key, string value) {
				//Add if property is not position trail or stops list
				//Arrays can't be used for conditions or displaying so theyre just ignored
				if (key != "Cot" && key != "Stops")
					_properties.Add(key, value);
			}

			/// <summary>
			/// Get list of keys from property list
			/// </summary>
			/// <returns>List of property keys</returns>
			public Dictionary<string, string>.KeyCollection GetPropertyKeys() {
				return _properties.Keys;
			}

			/// <summary>
			/// ToJSON
			/// </summary>
			public string ToJson() {
				return Newtonsoft.Json.JsonConvert.SerializeObject(_properties);
			}

			public override string ToString() {
				return $"Aircraft: { GetProperty("Icao")} | { GetProperty("Reg")} | { GetProperty("Type")} | { GetProperty("Call")}";
			}

			/// <summary>
			/// Aircraft class constructor
			/// </summary>
			/// <param name="icao">ICAO of aircraft</param>
			public Aircraft(string icao) {
				Icao = icao;
			}
		}

		/// <summary>
		/// Alert types
		/// </summary>
		public enum AlertType {
			Disabled,
			First_Contact,
			Last_Contact,
			First_and_Last_Contact
		}

		/// <summary>
		/// Tweet Link Types
		/// </summary>
		public enum TweetLink {
			None,
			Radar_link,
			Radar_link_with_aircraft_selected,
			Report_link
		}

		/// <summary>
		/// Property list types used in email config
		/// </summary>
		public enum PropertyListType {
			Hidden,
			Essentials,
			All
		}

		/// <summary>
		/// List of VRS properties
		/// </summary>
		/// //ADD NEW ITEMS TO END
		public enum vrsProperty {
			Time_Tracked,
			Receiver,
			Icao,
			Invalid_Icao,
			Registration,
			Altitude,
			Altitude_AMSL,
			Altitude_Type,
			Target_Altitude,
			Air_Pressure,
			Callsign,
			Callsign_Inaccurate,
			Speed,
			Speed_Type,
			Vertical_Speed,
			Vertical_Speed_Type,
			Aircraft_Model,
			Aircraft_Model_Icao,
			Departure_Airport,
			Arrival_Airport,
			Operator,
			Operator_Icao_Code,
			Squawk,
			Is_In_Emergency,
			Distance,
			Wake_Turbulence_Category,
			Engines,
			Engine_Type,
			Species,
			Is_Military,
			Registered_Country,
			Flight_Count,
			Message_Count,
			Is_On_Ground,
			User_Tag,
			Is_Interesting,
			Transponder_Type,
			Has_Signal_Level,
			Signal_Level,
			Mlat,
			Manufacturer,
			Bearing,
			Engine_Mount,
			Year,
			Serial,
			Is_Tisb,
			Track,
			Is_Track_Heading,
			Latitude,
			Longitude,
			Is_Ferry_Flight,
			Is_Charter_Flight,
			Ident,
			Target_Heading,
			Notes
		}

		/// <summary>
		/// Trigger information
		/// </summary>
		public class Trigger {
			/// <summary>
			/// Property to be checked
			/// </summary>
			public vrsProperty Property;

			/// <summary>
			/// Value to be checked against
			/// </summary>
			public string Value;

			/// <summary>
			/// Type of comparison
			/// </summary>
			public string ComparisonType;

			/// <summary>
			/// Trigger constructor
			/// </summary>
			/// <param name="property">Property to check</param>
			/// <param name="value">Value to be checked against</param>
			/// <param name="comparisonType">Type of comparison</param>
			public Trigger(vrsProperty property, string value, string comparisonType) {
				Property = property;
				Value = value;
				ComparisonType = comparisonType;
			}
		}

		/// <summary>
		/// Condition information
		/// </summary>
		public class Condition {
			/// <summary>
			/// Name of condition
			/// </summary>
			public string conditionName;

			/// <summary>
			/// Ignore following conditions
			/// </summary>
			public bool ignoreFollowing;

			/// <summary>
			/// Alerts sent for this condition
			/// </summary>
			public int alertsThisSession = 0;

			/// <summary>
			/// Send emails?
			/// </summary>
			public bool emailEnabled;

			/// <summary>
			/// Emails to send alert to
			/// </summary>
			public List<string> recieverEmails = new List<string>();

			/// <summary>
			/// Email first contact alert format
			/// </summary>
			public string emailFirstFormat;

			/// <summary>
			/// Email last contact alert format
			/// </summary>
			public string emailLastFormat;

			/// <summary>
			/// Send tweets?
			/// </summary>
			public bool twitterEnabled;

			/// <summary>
			/// Twitter account to send from
			/// </summary>
			public string twitterAccount;

			/// <summary>
			/// Tweet first contact alert format
			/// </summary>
			public string tweetFirstFormat;

			/// <summary>
			/// Tweet last contact alert format
			/// </summary>
			public string tweetLastFormat;

			/// <summary>
			/// Link to be attached in tweets
			/// </summary>
			public TweetLink tweetLink;

			/// <summary>
			/// Attach map?
			/// </summary>
			public bool tweetMap;

			/// <summary>
			/// Type of alert
			/// </summary>
			public AlertType alertType;

			/// <summary>
			/// List of triggers
			/// </summary>
			public Dictionary<int, Trigger> triggers = new Dictionary<int, Trigger>();

			/// <summary>
			/// Increase sent emails counter for condition and total alerts sent
			/// </summary>
			public void increaseSentAlerts() {
				alertsThisSession++;
				Stats.totalAlertsSent++;
			}

			/// <summary>
			/// Creates a new condition
			/// </summary>
			public Condition() {
				
			}
		}

		/// <summary>
		/// Match information
		/// </summary>
		public class Match {
			/// <summary>
			/// ICAO of aircraft matched
			/// </summary>
			public string Icao { get; private set; }

			/// <summary>
			/// Time signal was lost
			/// </summary>
			public DateTime SignalLostTime;

			/// <summary>
			/// Has signal been lost?
			/// </summary>
			public bool SignalLost;

			/// <summary>
			/// Ignore if any other alerts match for this aircraft
			/// </summary>
			public bool IgnoreFollowing;

			/// <summary>
			/// Conditions matched to
			/// </summary>
			public readonly List<MatchedCondition> Conditions;

			/// <summary>
			/// Add a condition to the match
			/// </summary>
			/// <param name="id">ID of condition</param>
			/// <param name="match">Condition</param>
			/// <param name="aircraftInfo">Aircraft information when matched</param>
			public void AddCondition(int id, Condition match, Aircraft aircraftInfo) {
				Conditions.Add(new MatchedCondition(id, match, aircraftInfo));
				IgnoreFollowing = match.ignoreFollowing;
			}

			/// <summary>
			/// Remove condition from match
			/// </summary>
			/// <param name="match"></param>
			public void RemoveCondition(MatchedCondition match) {
				Conditions.Remove(match);
			}

			/// <summary>
			/// Create match
			/// </summary>
			/// <param name="icao">ICAO of aircraft matched</param>
			public Match(string icao) {
				Icao = icao;
				Conditions = new List<MatchedCondition>();
			}
		}

		/// <summary>
		/// Matched condition information
		/// </summary>
		public class MatchedCondition {
			/// <summary>
			/// Condition matched with
			/// </summary>
			public Condition Condition;

			/// <summary>
			/// ID of condition matched with
			/// </summary>
			public int ConditionID;

			/// <summary>
			/// Information of aircraft matched with
			/// </summary>
			public Aircraft AircraftInfo;

			/// <summary>
			/// Create condition match
			/// </summary>
			/// <param name="conditionID">ID of condition</param>
			/// <param name="c">Condition matched with</param>
			/// <param name="aircraftInfo">Aircraft information of matched aircraft</param>
			public MatchedCondition(int conditionID, Condition c, Aircraft aircraftInfo) {
				Condition = c;
				AircraftInfo = aircraftInfo;
				ConditionID = conditionID;
			}
		}

		/// <summary>
		/// Core static constructor
		/// </summary>
		static Core() {
			//LEGEND
			//A = Equals/Not Equals
			//B = Higher Than + Lower Than
			//C = True/False Boolean
			//D = Starts With + Ends With
			//E = Contains

			//Add all of the property information
			VrsPropertyData.Add(vrsProperty.Time_Tracked, new string[] { "Number", "AB", "TSecs", "The number of seconds that the aircraft has been tracked for." });
			VrsPropertyData.Add(vrsProperty.Receiver, new string[] { "Number", "A", "Rcvr", "The ID of the feed that last supplied information about the aircraft." });
			VrsPropertyData.Add(vrsProperty.Icao, new string[] { "String", "ADE", "Icao", "The ICAO of the aircraft." });
			VrsPropertyData.Add(vrsProperty.Invalid_Icao, new string[] { "Boolean", "C", "Bad", "True if the ICAO is known to be invalid." });
			VrsPropertyData.Add(vrsProperty.Registration, new string[] { "String", "ADE", "Reg", "The registration." });
			VrsPropertyData.Add(vrsProperty.Altitude, new string[] { "Number", "B", "Alt", "The altitude in feet at standard pressure." });
			VrsPropertyData.Add(vrsProperty.Altitude_AMSL, new string[] { "Number", "B", "GAlt", "The altitude adjusted for local air pressure, should be roughly the height above mean sea level." });
			VrsPropertyData.Add(vrsProperty.Altitude_Type, new string[] { "Number", "A", "AltT", "0 = altitude is barometric, 1 = altitude is geometric. Default to barometric until told otherwise." });
			VrsPropertyData.Add(vrsProperty.Air_Pressure, new string[] { "Number", "B", "InHg", "The air pressure in inches of mercury that was used to calculate the AMSL altitude from the standard pressure altitude." });
			VrsPropertyData.Add(vrsProperty.Target_Altitude, new string[] { "Number", "AB", "TAlt", "The target altitude, in feet, set on the autopilot / FMS etc." });
			VrsPropertyData.Add(vrsProperty.Callsign, new string[] { "String", "ADE", "Call", "The callsign." });
			VrsPropertyData.Add(vrsProperty.Callsign_Inaccurate, new string[] { "Boolean", "C", "CallSus", "True if the callsign may not be correct." });
			VrsPropertyData.Add(vrsProperty.Latitude, new string[] { "Number", "B", "Lat", "The aircraft's latitude over the ground." });
			VrsPropertyData.Add(vrsProperty.Longitude, new string[] { "Number", "B", "Long", "The aircraft's longitude over the ground." });
			VrsPropertyData.Add(vrsProperty.Mlat, new string[] { "Boolean", "C", "Mlat", "True if the aircraft's position was determined using Multilateration." });
			VrsPropertyData.Add(vrsProperty.Is_Tisb, new string[] { "Boolean", "C", "Tisb", "True if the last message received was from a TIS-B source." });
			VrsPropertyData.Add(vrsProperty.Speed, new string[] { "Number", "B", "Spd", "The speed in knots." });
			VrsPropertyData.Add(vrsProperty.Speed_Type, new string[] { "Number", "A", "SpdTyp", "The type of speed that Speed represents. Only used with raw feeds. 0/missing = ground speed, 1 = ground speed reversing, 2 = indicated air speed, 3 = true air speed." });
			VrsPropertyData.Add(vrsProperty.Vertical_Speed, new string[] { "Number", "B", "Vsi", "Vertical speed in feet per minute." });
			VrsPropertyData.Add(vrsProperty.Vertical_Speed_Type, new string[] { "Number", "A", "VsiT", "0 = vertical speed is barometric, 1 = vertical speed is geometric. Default to barometric until told otherwise." });
			VrsPropertyData.Add(vrsProperty.Track, new string[] { "Number", "B", "Trak", "Aircraft's track angle across the ground clockwise from 0° north." });
			VrsPropertyData.Add(vrsProperty.Is_Track_Heading, new string[] { "Boolean", "C", "TrkH", "True if Trak is the aircraft's heading, false if it's the ground track. Default to ground track until told otherwise." });
			VrsPropertyData.Add(vrsProperty.Target_Heading, new string[] { "Number", "B", "TTrk", "The track or heading currently set on the aircraft's autopilot or FMS." });
			VrsPropertyData.Add(vrsProperty.Aircraft_Model_Icao, new string[] { "String", "ADE", "Type", "The aircraft model's ICAO type code." });
			VrsPropertyData.Add(vrsProperty.Aircraft_Model, new string[] { "String", "AE", "Mdl", "A description of the aircraft's model." });
			VrsPropertyData.Add(vrsProperty.Manufacturer, new string[] { "String", "A", "Man", "The manufacturer of the aircraft." });
			VrsPropertyData.Add(vrsProperty.Serial, new string[] { "String", "ADE", "CNum", "The aircraft's construction or serial number." });
			VrsPropertyData.Add(vrsProperty.Departure_Airport, new string[] { "String", "AE", "From", "The code and name of the departure airport." });
			VrsPropertyData.Add(vrsProperty.Arrival_Airport, new string[] { "String", "AE", "To", "The code and name of the arrival airport." });
			VrsPropertyData.Add(vrsProperty.Operator, new string[] { "String", "AE", "Op", "The name of the aircraft's operator." });
			VrsPropertyData.Add(vrsProperty.Operator_Icao_Code, new string[] { "String", "A", "OpIcao", "The operator's ICAO code." });
			VrsPropertyData.Add(vrsProperty.Squawk, new string[] { "Number", "ABD", "Sqk", "The squawk." });
			VrsPropertyData.Add(vrsProperty.Ident, new string[] { "Boolean", "C", "Ident", "True if the aircraft is squawking ident." });
			VrsPropertyData.Add(vrsProperty.Is_In_Emergency, new string[] { "Boolean", "C", "Help", "True if the aircraft is transmitting an emergency squawk." });
			VrsPropertyData.Add(vrsProperty.Distance, new string[] { "Number", "B", "Dst", "The distance to the aircraft in kilometres." });
			VrsPropertyData.Add(vrsProperty.Bearing, new string[] { "Number", "B", "Brng", "The bearing to the aircraft from 0° north" });
			VrsPropertyData.Add(vrsProperty.Wake_Turbulence_Category, new string[] { "Number", "AB", "WTC", "The wake turbulence category of the aircraft. 1 = none, 2 = light, 3 = medium, 4 = heavy" });
			VrsPropertyData.Add(vrsProperty.Engines, new string[] { "Number", "AB", "Engines", "The number of engines the aircraft has." });
			VrsPropertyData.Add(vrsProperty.Engine_Type, new string[] { "Number", "A", "EngType", "The type of engine the aircraft uses. 0 = none, 1 = piston, 2 = turbo, 3 = jet, 4 = electric" });
			VrsPropertyData.Add(vrsProperty.Engine_Mount, new string[] { "Number", "A", "EngMount", "The placement of engines on the aircraft. 0 = unknown, 1 = aft mounted, 2 = wing buried, 3 = fuselage buried, 4 = nose mounted, 5 = wing mounted" });
			VrsPropertyData.Add(vrsProperty.Species, new string[] { "Number", "A", "Species", "The species of the aircraft (helicopter, jet etc.). 0 = none, 1 = landplane, 2 = seaplane, 3 = amphibian, 4 = helicopter, 5 = gyrocopter, 6 = tiltwing, 7 = ground vehicle, 8 = tower" });
			VrsPropertyData.Add(vrsProperty.Is_Military, new string[] { "Boolean", "C", "Mil", "True if the aircraft appears to be operated by the military." });
			VrsPropertyData.Add(vrsProperty.Registered_Country, new string[] { "String", "A", "Cou", "The country that the aircraft is registered to." });
			VrsPropertyData.Add(vrsProperty.Flight_Count, new string[] { "Number", "AB", "FlightsCount", "The number of Flights records the aircraft has in the database." });
			VrsPropertyData.Add(vrsProperty.Message_Count, new string[] { "Number", "AB", "CMsgs", "The count of messages received for the aircraft." });
			VrsPropertyData.Add(vrsProperty.Is_On_Ground, new string[] { "Boolean", "C", "Gnd", "True if the aircraft is on the ground." });
			VrsPropertyData.Add(vrsProperty.User_Tag, new string[] { "String", "AE", "Tag", "The user tag found for the aircraft in the BaseStation.sqb local database." });
			VrsPropertyData.Add(vrsProperty.Notes, new string[] { "String", "AE", "Notes", "The notes found for the aircraft in the BaseStation.sqb local database." });
			VrsPropertyData.Add(vrsProperty.Is_Interesting, new string[] { "Boolean", "C", "Interested", "True if the aircraft is flagged as interesting in the BaseStation.sqb local database." });
			VrsPropertyData.Add(vrsProperty.Transponder_Type, new string[] { "Number", "AB", "Trt", "Transponder type. 0 = Unknown, 1 = Mode-S, 2 = ADS-B (unknown version), 3 = ADS-B 1, 4 = ADS-B 2." });
			VrsPropertyData.Add(vrsProperty.Year, new string[] { "Number", "AB", "Year", "The year the aircraft was manufactured." });
			VrsPropertyData.Add(vrsProperty.Has_Signal_Level, new string[] { "Boolean", "C", "HasSig", "True if the aircraft has a signal level associated with it." });
			VrsPropertyData.Add(vrsProperty.Signal_Level, new string[] { "Number", "B", "Sig", "The signal level for the last message received from the aircraft, as reported by the receiver. Not all receivers pass signal levels. The value's units are receiver-dependent." });
			VrsPropertyData.Add(vrsProperty.Is_Ferry_Flight, new string[] { "Boolean", "C", "IsFerryFlight", "True if this is a ferry flight." });
			VrsPropertyData.Add(vrsProperty.Is_Charter_Flight, new string[] { "Boolean", "C", "IsCharterFlight", "True if this is a charter flight." });

			//Add essential properties to list
			EssentialProperties.Add(vrsProperty.Receiver);
			EssentialProperties.Add(vrsProperty.Icao);
			EssentialProperties.Add(vrsProperty.Registration);
			EssentialProperties.Add(vrsProperty.Altitude);
			EssentialProperties.Add(vrsProperty.Callsign);
			EssentialProperties.Add(vrsProperty.Speed);
			EssentialProperties.Add(vrsProperty.Vertical_Speed);
			EssentialProperties.Add(vrsProperty.Aircraft_Model);
			EssentialProperties.Add(vrsProperty.Aircraft_Model_Icao);
			EssentialProperties.Add(vrsProperty.Departure_Airport);
			EssentialProperties.Add(vrsProperty.Arrival_Airport);
			EssentialProperties.Add(vrsProperty.Operator);
			EssentialProperties.Add(vrsProperty.Operator_Icao_Code);
			EssentialProperties.Add(vrsProperty.Squawk);
			EssentialProperties.Add(vrsProperty.Registered_Country);
			EssentialProperties.Add(vrsProperty.Transponder_Type);
			EssentialProperties.Add(vrsProperty.Manufacturer);

			//Add comparison types
			ComparisonTypes.Add("A", new string[] { "Equals", "Not Equals" });
			ComparisonTypes.Add("B", new string[] { "Higher Than", "Lower Than" });
			ComparisonTypes.Add("C", new string[] { "Equals", "Not Equals" });
			ComparisonTypes.Add("D", new string[] { "Starts With", "Ends With" });
			ComparisonTypes.Add("E", new string[] { "Contains" });
		}

		public static void LogAlert(Condition condition, Aircraft aircraft, string receiver, bool isFirst) {
			try {
				string message = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm")} | {condition.conditionName} | {receiver} | {(isFirst?"FIRST":"LAST")} CONTACT ALERT: " + Environment.NewLine;
				message += aircraft.ToJson() + Environment.NewLine + Environment.NewLine;

				File.AppendAllText("alerts.log", message);
			}
			catch (Exception e) {
				Ui.writeToConsole("ERROR: Error writing to alerts.log file: " + e.Message, Color.Red);
			}
		}

		/// <summary>
		/// Parse a custom format string to replace property names with values
		/// </summary>
		/// <returns>Parsed string</returns>
		public static string ParseCustomFormatString(string format, Aircraft aircraft, Condition condition) {
			Dictionary<string, string> variables = new Dictionary<string, string> {
				{ "ConditionName", condition.conditionName },
				{ "RcvrName", Receivers.ContainsKey(aircraft.GetProperty("Rcvr")) ? Receivers[aircraft.GetProperty("Rcvr")] : "" },
				{ "Date", DateTime.Now.ToString("d") },
				{ "Time", DateTime.Now.ToString("t") },
			};

			//Iterate variables
			foreach (string varkey in variables.Keys) {
				//Check if content contains keyword
				if (format.ToLower().Contains(@"[" + varkey.ToLower() + @"]")) {
					//Replace keyword with value
					format = Regex.Replace(format, @"\[" + varkey + @"\]", variables[varkey], RegexOptions.IgnoreCase);
				}
			}

			//Iterate properties
			foreach (string[] info in VrsPropertyData.Values) {
				//Check if content contains keyword
				if (format.ToLower().Contains(@"[" + info[2].ToLower() + @"]")) {
					//Replace keyword with value
					string value = aircraft.GetProperty(info[2]) ?? "";

					//If enum, replace with string value
					if (Enums.TryGetConvertedValue(info[2], value, out string convertedvalue)) {
						value = convertedvalue;
					}

					format = Regex.Replace(format, @"\[" + info[2] + @"\]", value, RegexOptions.IgnoreCase);
				}
			}

			return format;
		}

		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		public static string GenerateReportURL(string ICAO, bool mobile) {
			string reportUrl = "";
			if (string.IsNullOrWhiteSpace(Settings.radarUrl)) {
				Ui.writeToConsole("ERROR: Please enter radar URL in settings", Color.Red);
				return "";
			}
			if (!Settings.radarUrl.ToLower().Contains("virtualradar")) {
				Ui.writeToConsole("WARNING: Radar URL must end with /VirtualRadar/ for report links to work", Color.Orange);
				return "";
			}


			reportUrl += Settings.radarUrl;
			if (Settings.radarUrl[Settings.radarUrl.Length - 1] != '/') reportUrl += "/";

			if (mobile) reportUrl += "mobileReport.html?sort1=date&sortAsc1=0&icao-Q=" + ICAO;
			else reportUrl += "desktopReport.html?sort1=date&sortAsc1=0&icao-Q=" + ICAO;

			return reportUrl;
		}

		/// <summary>
		/// Generate a map
		/// </summary>
		public static string GenerateMapURL(Aircraft aircraft) {
			string staticMapUrl = "";
			//If aircraft has a position, generate a google map url
			if (aircraft.GetProperty("Lat") != null)
				if (Settings.centreMapOnAircraft) {
					if (aircraft.Trail.Count() != 4)
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
					else
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&zoom=8&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				}
				else { 
					if (aircraft.Trail.Count() != 4)
					staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + Settings.Lat + "," + Settings.Long + "&size=800x800&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				else
					staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + Settings.Lat + "," + Settings.Long + "&size=800x800&zoom=8&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				}
					//Process aircraft trail
			for (int i = (aircraft.Trail.Count() / 4) - 1; i >= 0; i--) {
				//Get coordinate
				string[] coord = new string[] {
						aircraft.Trail[i * 4].ToString("#.####"),
						aircraft.Trail[i * 4 + 1].ToString("#.####"),
						aircraft.Trail[i * 4 + 2].ToString("#.####"),
						aircraft.Trail[i * 4 + 3].ToString("#.####")
					};
				string coordstring = coord[0] + "," + coord[1]  + "|";

				//Check if adding another coordinate will make the url too long
				if (staticMapUrl.Length + coordstring.Length > 8000) break; //Limit is 8192, using 8000 to give some headroom. Allows for about 440 points

				//Add coordinate to google map url
				staticMapUrl += coordstring;
			}

			//Return empty string if no positions
			if (staticMapUrl == "" || staticMapUrl.Length == 0) return "";

			return staticMapUrl.Substring(0, staticMapUrl.Length-1);
		}

		/// <summary>
		/// Generate a KML
		/// </summary>
		public static string GenerateKML(Aircraft aircraft) {
			string KMLData = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
	<Document>
		<name>{aircraft.GetProperty("Reg")}</name>
		<Placemark>
			<name>Flight path of {aircraft.GetProperty("Reg")}</name>
			<Style>
				<LineStyle>
					<color>7fffffff</color>
					<width>2</width>
				</LineStyle>
				<PolyStyle>
					<color>7fffffff</color>
				</PolyStyle>
			</Style>
			<LineString>
				<extrude>1</extrude>
				<altitudeMode>absolute</altitudeMode>
				<coordinates>";

			//Process aircraft trail
			for (int i = 0; i < aircraft.Trail.Count() / 4; i++) {
				//Get coordinate
				string[] coord = new string[] {
					aircraft.Trail[i * 4].ToString("#.####"),
					aircraft.Trail[i * 4 + 1].ToString("#.####"),
					aircraft.Trail[i * 4 + 2].ToString("#.####"),
					(aircraft.Trail[i * 4 + 3] * 0.3048).ToString("#.####") // Convert feet to metres for KML
				};

				//Add coordinate to KML
				KMLData += coord[1] + "," + coord[0] + "," + coord[3] + " ";
			}

			//Add rest of KML
			KMLData += @"</coordinates>
			</LineString>
		</Placemark>
	</Document>
</kml>";
			
			return KMLData;
		}
	}
}
