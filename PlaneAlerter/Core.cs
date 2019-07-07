using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Linq;

namespace PlaneAlerter {
	/// <summary>
	/// Important variables and things
	/// </summary>
	public static class Core {
		/// <summary>
		/// List of current conditions
		/// </summary>
		public static Dictionary<int, Condition> conditions = new Dictionary<int, Condition>();

		/// <summary>
		/// List of current aircraft
		/// </summary>
		public static List<Aircraft> aircraftlist = new List<Aircraft>();

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		public static List<Reciever> receivers = new List<Reciever>();

		/// <summary>
		/// VRS property descriptions
		/// </summary>
		public static Dictionary<vrsProperty, string[]> vrsPropertyData = new Dictionary<vrsProperty, string[]>();

		/// <summary>
		/// Types of comparisons usable with triggers
		/// </summary>
		public static Dictionary<string, string[]> comparisonTypes = new Dictionary<string, string[]>();

		/// <summary>
		/// Properties shown when essential properties selected
		/// </summary>
		public static List<vrsProperty> essentialProperties = new List<vrsProperty>();

		/// <summary>
		/// List of current matches
		/// </summary>
		public static Dictionary<string, Match> activeMatches = new Dictionary<string, Match>();

		/// <summary>
		/// Matches waiting to be verified
		/// </summary>
		public static List<string[]> waitingMatches = new List<string[]>();

		/// <summary>
		/// Thread for checking operations
		/// </summary>
		public static Thread loopThread;

		/// <summary>
		/// Thread for updating statistics
		/// </summary>
		public static Thread statsThread;

		/// <summary>
		/// File stream for log file
		/// </summary>
		public static FileStream logFile = new FileStream("alerts.log", FileMode.Append, FileAccess.Write);

		/// <summary>
		/// Log file stream writer
		/// </summary>
		public static StreamWriter logFileSW = new StreamWriter(logFile);

		/// <summary>
		/// Active form
		/// </summary>
		public static PlaneAlerter UI;

		/// <summary>
		/// Class to store aircraft information
		/// </summary>
		public class Aircraft {
			/// <summary>
			/// List of property values retrieved from last aircraftlist.json
			/// </summary>
			private Dictionary<string, string> Properties = new Dictionary<string, string>();

			/// <summary>
			/// ICAO hex of aircraft
			/// </summary>
			public string ICAO;

			/// <summary>
			/// Position track from last aircraftlist.json
			/// </summary>
			public double[] Trail = new double[0];

			/// <summary>
			/// Get property value from property list
			/// </summary>
			/// <param name="key">Property name</param>
			/// <returns>Value of property</returns>
			public string GetProperty(string key) {
				if (Properties.ContainsKey(key))
					return Properties[key];
				else
					return null;
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
					Properties.Add(key, value);
			}

			/// <summary>
			/// Get list of keys from property list
			/// </summary>
			/// <returns>List of property keys</returns>
			public Dictionary<string, string>.KeyCollection GetPropertyKeys() {
				return Properties.Keys;
			}

			/// <summary>
			/// Aircraft class constructor
			/// </summary>
			/// <param name="icao">ICAO of aircraft</param>
			public Aircraft(string icao) {
				ICAO = icao;
			}
		}

		/// <summary>
		/// VRS Receiver information
		/// </summary>
		public struct Reciever {
			/// <summary>
			/// ID of receiver
			/// </summary>
			public string Id;

			/// <summary>
			/// Name or receiver
			/// </summary>
			public string Name;

			/// <summary>
			/// Receiver constructor
			/// </summary>
			/// <param name="id">Id of receiver</param>
			/// <param name="name">Name of receiver</param>
			public Reciever(string id, string name) {
				Id = id;
				Name = name;
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
			Manufacturer
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
			/// Property to show in alert email
			/// </summary>
			public vrsProperty emailProperty;

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
			vrsPropertyData.Add(vrsProperty.Time_Tracked, new string[] { "Number", "AB", "TSecs", "The number of seconds that the aircraft has been tracked for." });
			vrsPropertyData.Add(vrsProperty.Receiver, new string[] { "Number", "A", "Rcvr", "The ID of the feed that last supplied information about the aircraft." });
			vrsPropertyData.Add(vrsProperty.Icao, new string[] { "String", "ADE", "Icao", "The ICAO of the aircraft." });
			vrsPropertyData.Add(vrsProperty.Invalid_Icao, new string[] { "Boolean", "C", "Bad", "True if the ICAO is known to be invalid." });
			vrsPropertyData.Add(vrsProperty.Registration, new string[] { "String", "ADE", "Reg", "The registration." });
			vrsPropertyData.Add(vrsProperty.Altitude, new string[] { "Number", "B", "Alt", "The altitude in feet at standard pressure." });
			vrsPropertyData.Add(vrsProperty.Altitude_AMSL, new string[] { "Number", "B", "GAlt", "The altitude adjusted for local air pressure, should be roughly the height above mean sea level." });
			vrsPropertyData.Add(vrsProperty.Altitude_Type, new string[] { "Number", "A", "AltT", "0 = altitude is barometric, 1 = altitude is geometric. Default to barometric until told otherwise." });
			vrsPropertyData.Add(vrsProperty.Air_Pressure, new string[] { "Number", "B", "inHg", "The air pressure in inches of mercury that was used to calculate the AMSL altitude from the standard pressure altitude." });
			vrsPropertyData.Add(vrsProperty.Target_Altitude, new string[] { "Number", "AB", "TAlt", "The target altitude, in feet, set on the autopilot / FMS etc." });
			vrsPropertyData.Add(vrsProperty.Callsign, new string[] { "String", "ADE", "Call", "The callsign." });
			vrsPropertyData.Add(vrsProperty.Callsign_Inaccurate, new string[] { "Boolean", "C", "CallSus", "True if the callsign may not be correct." });
			vrsPropertyData.Add(vrsProperty.Speed, new string[] { "Number", "B", "Spd", "The speed in knots." });
			vrsPropertyData.Add(vrsProperty.Speed_Type, new string[] { "Number", "A", "SpdTyp", "The type of speed that Speed represents. Only used with raw feeds. 0/missing = ground speed, 1 = ground speed reversing, 2 = indicated air speed, 3 = true air speed." });
			vrsPropertyData.Add(vrsProperty.Vertical_Speed, new string[] { "Number", "B", "Vsi", "Vertical speed in feet per minute." });
			vrsPropertyData.Add(vrsProperty.Vertical_Speed_Type, new string[] { "Number", "A", "VsiT", "0 = vertical speed is barometric, 1 = vertical speed is geometric. Default to barometric until told otherwise." });
			vrsPropertyData.Add(vrsProperty.Aircraft_Model, new string[] { "String", "AE", "Mdl", "A description of the aircraft's model." });
			vrsPropertyData.Add(vrsProperty.Aircraft_Model_Icao, new string[] { "String", "ADE", "Type", "The aircraft model's ICAO type code." });
			vrsPropertyData.Add(vrsProperty.Departure_Airport, new string[] { "String", "AE", "From", "The code and name of the departure airport." });
			vrsPropertyData.Add(vrsProperty.Arrival_Airport, new string[] { "String", "AE", "To", "The code and name of the arrival airport." });
			vrsPropertyData.Add(vrsProperty.Operator, new string[] { "String", "AE", "Op", "The name of the aircraft's operator." });
			vrsPropertyData.Add(vrsProperty.Operator_Icao_Code, new string[] { "String", "A", "OpIcao", "The operator's ICAO code." });
			vrsPropertyData.Add(vrsProperty.Squawk, new string[] { "Number", "ABD", "Sqk", "The squawk." });
			vrsPropertyData.Add(vrsProperty.Is_In_Emergency, new string[] { "Boolean", "C", "Help", "True if the aircraft is transmitting an emergency squawk." });
			vrsPropertyData.Add(vrsProperty.Distance, new string[] { "Number", "B", "Dst", "The distance to the aircraft in kilometres." });
			vrsPropertyData.Add(vrsProperty.Wake_Turbulence_Category, new string[] { "Number", "A", "WTC", "The wake turbulence category of the aircraft. 1 = none, 2 = light, 3 = medium, 4 = heavy" });
			vrsPropertyData.Add(vrsProperty.Engines, new string[] { "Number", "AB", "Engines", "The number of engines the aircraft has." });
			vrsPropertyData.Add(vrsProperty.Engine_Type, new string[] { "Number", "A", "EngType", "The type of engine the aircraft uses. 0 = none, 1 = piston, 2 = turbo, 3 = jet, 4 = electric" });
			vrsPropertyData.Add(vrsProperty.Species, new string[] { "Number", "A", "Species", "The species of the aircraft (helicopter, jet etc.). 0 = none, 1 = landplane, 2 = seaplane, 3 = amphibian, 4 = helicopter, 5 = gyrocopter, 6 = tiltwing, 7 = ground vehicle, 8 = tower" });
			vrsPropertyData.Add(vrsProperty.Is_Military, new string[] { "Boolean", "C", "Mil", "True if the aircraft appears to be operated by the military." });
			vrsPropertyData.Add(vrsProperty.Registered_Country, new string[] { "String", "A", "Cou", "The country that the aircraft is registered to." });
			vrsPropertyData.Add(vrsProperty.Flight_Count, new string[] { "Number", "AB", "FlightsCount", "The number of Flights records the aircraft has in the database." });
			vrsPropertyData.Add(vrsProperty.Message_Count, new string[] { "Number", "AB", "CMsgs", "The count of messages received for the aircraft." });
			vrsPropertyData.Add(vrsProperty.Is_On_Ground, new string[] { "Boolean", "C", "Gnd", "True if the aircraft is on the ground." });
			vrsPropertyData.Add(vrsProperty.User_Tag, new string[] { "String", "AE", "Tag", "The user tag found for the aircraft in the BaseStation.sqb local database." });
			vrsPropertyData.Add(vrsProperty.Is_Interesting, new string[] { "Boolean", "C", "Interested", "True if the aircraft is flagged as interesting in the BaseStation.sqb local database." });
			vrsPropertyData.Add(vrsProperty.Transponder_Type, new string[] { "Number", "A", "Trt", "Transponder type. 0 = Unknown, 1 = Mode-S, 2 = ADS-B (unknown version), 3 = ADS-B 1, 4 = ADS-B 2." });
			vrsPropertyData.Add(vrsProperty.Has_Signal_Level, new string[] { "Boolean", "C", "HasSig", "True if the aircraft has a signal level associated with it." });
			vrsPropertyData.Add(vrsProperty.Signal_Level, new string[] { "Number", "B", "Sig", "The signal level for the last message received from the aircraft, as reported by the receiver. Not all receivers pass signal levels. The value's units are receiver-dependent." });
			vrsPropertyData.Add(vrsProperty.Mlat, new string[] { "Boolean", "C", "Mlat", "True if the aircraft's position was determined using Multilateration." });
			vrsPropertyData.Add(vrsProperty.Manufacturer, new string[] { "String", "A", "Man", "The manufacturer of the aircraft." });

			//Add essential properties to list
			essentialProperties.Add(vrsProperty.Receiver);
			essentialProperties.Add(vrsProperty.Icao);
			essentialProperties.Add(vrsProperty.Registration);
			essentialProperties.Add(vrsProperty.Altitude);
			essentialProperties.Add(vrsProperty.Callsign);
			essentialProperties.Add(vrsProperty.Speed);
			essentialProperties.Add(vrsProperty.Vertical_Speed);
			essentialProperties.Add(vrsProperty.Aircraft_Model);
			essentialProperties.Add(vrsProperty.Aircraft_Model_Icao);
			essentialProperties.Add(vrsProperty.Departure_Airport);
			essentialProperties.Add(vrsProperty.Arrival_Airport);
			essentialProperties.Add(vrsProperty.Operator);
			essentialProperties.Add(vrsProperty.Operator_Icao_Code);
			essentialProperties.Add(vrsProperty.Squawk);
			essentialProperties.Add(vrsProperty.Registered_Country);
			essentialProperties.Add(vrsProperty.Transponder_Type);
			essentialProperties.Add(vrsProperty.Manufacturer);

			//Add comparison types
			comparisonTypes.Add("A", new string[] { "Equals", "Not Equals" });
			comparisonTypes.Add("B", new string[] { "Higher Than", "Lower Than" });
			comparisonTypes.Add("C", new string[] { "Equals", "Not Equals" });
			comparisonTypes.Add("D", new string[] { "Starts With", "Ends With" });
			comparisonTypes.Add("E", new string[] { "Contains" });
		}

		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		public static string GenerateReportURL(string ICAO) {
			string reportUrl = "";
			if (string.IsNullOrWhiteSpace(Settings.radarUrl)) {
				UI.writeToConsole("ERROR: Please enter radar URL in settings", Color.Red);
				return "";
			}
			if (!Settings.radarUrl.Contains("VirtualRadar")) {
				UI.writeToConsole("WARNING: Radar URL must end with /VirtualRadar/ for report links to work", Color.Orange);
			}

			if (Settings.radarUrl[Settings.radarUrl.Length - 1] == '/')
				reportUrl = Settings.radarUrl + "desktopReport.html?icao-Q=" + ICAO;
			else
				reportUrl = Settings.radarUrl + "/desktopReport.html?icao-Q=" + ICAO;
			return reportUrl;
		}

		/// <summary>
		/// Generate a map
		/// </summary>
		public static string GenerateMapURL(Aircraft aircraft) {
			string staticMapUrl = "";
			//If aircraft has a position, generate a google map url
			if (aircraft.GetProperty("Lat") != null)
				if (aircraft.Trail.Count() != 3)
					staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				else
					staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&zoom=8&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";

			//Process aircraft trail
			for (int i = 0; i < aircraft.Trail.Count() / 3; i++) {
				//Get coordinate
				string[] coord = new string[] {
						aircraft.Trail[i * 3].ToString(),
						aircraft.Trail[i * 3 + 1].ToString(),
						aircraft.Trail[i * 3 + 2].ToString()
					};
				//Add coordinate to google map url
				staticMapUrl += coord[0] + "," + coord[1];
				//If this is not the last coordinate, add a separator
				if (i != (aircraft.Trail.Count() / 3) - 1)
					staticMapUrl += "|";
			}
			return staticMapUrl;
		}
	}
}
