using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace PlaneAlerter {
	public static class Core {
		public static Dictionary<int, Condition> conditions = new Dictionary<int, Condition>();
		public static List<Core.Aircraft> aircraftlist = new List<Core.Aircraft>();
		public static List<Core.Reciever> receivers = new List<Core.Reciever>();
		public static Dictionary<vrsProperty, string[]> vrsPropertyData = new Dictionary<vrsProperty, string[]>();
		public static Dictionary<string, string[]> comparisonTypes = new Dictionary<string, string[]>();
		public static List<vrsProperty> essentialProperties = new List<vrsProperty>();
		public static Dictionary<string, Match> activeMatches = new Dictionary<string, Match>();
		public static List<string[]> waitingMatches = new List<string[]>();
		public static Thread loopThread;
		public static Thread statsThread;
		public static FileStream logFile = new FileStream("alerts.log", FileMode.Append, FileAccess.Write);
		public static StreamWriter logFileSW = new StreamWriter(logFile);
		public static PlaneAlerter UI;

		public class Aircraft {
			private Dictionary<string, string> Properties = new Dictionary<string, string>();
			public string ICAO;
			public double[] Trail = new double[0];

			public string GetProperty(string key) {
				if (Properties.ContainsKey(key))
					return Properties[key];
				else
					return null;
			}

			public void AddProperty(string key, string value) {
				if (key != "Cot" && key != "Stops")
					Properties.Add(key, value);
			}

			public Dictionary<string, string>.KeyCollection GetPropertyKeys() {
				return Properties.Keys;
			}

			public Aircraft(string icao) {
				ICAO = icao;
			}
		}

		public struct Reciever {
			public string Id;
			public string Name;

			public Reciever(string id, string name) {
				Id = id;
				Name = name;
			}
		}

		public enum AlertType {
			Disabled,
			First,
			Last,
			Both
		}

		public enum PropertyListType {
			Hidden,
			Essentials,
			All
		}

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

		public class Trigger {
			public vrsProperty Property;
			public string Value;
			public string ComparisonType;

			public Trigger(vrsProperty property, string value, string comparisonType) {
				Property = property;
				Value = value;
				ComparisonType = comparisonType;
			}
		}

		public class Condition {
			public string conditionName;
			public bool ignoreFollowing;
			public int emailsThisSession = 0;
			public List<string> recieverEmails = new List<string>();
			public AlertType alertType;
			public vrsProperty emailProperty;
			public Dictionary<int, Trigger> triggers = new Dictionary<int, Trigger>();

			public void increaseSentEmails() {
				emailsThisSession++;
				Stats.totalEmailsSent++;
			}

			public Condition() {
				conditionName = "New Condition";
			}
		}

		public class Match {
			public string Icao { get; private set; }
			public DateTime SignalLostTime;
			public bool SignalLost;
			public string DisplayName { get; private set; }
			public readonly List<MatchedCondition> Conditions;

			private void GenerateDisplayName() {
				DisplayName = "";
				foreach (MatchedCondition c in Conditions) {
					DisplayName += c.DisplayName + ", ";
				}
				DisplayName = DisplayName.Substring(0, DisplayName.Length - 2);
			}

			public void AddCondition(int id, Condition match, Core.Aircraft aircraftInfo) {
				Conditions.Add(new MatchedCondition(id, match, aircraftInfo));
				GenerateDisplayName();
			}

			public void RemoveCondition(MatchedCondition match) {
				Conditions.Remove(match);
			}

			public Match(string icao) {
				Icao = icao;
				DisplayName = "";
				Conditions = new List<MatchedCondition>();
			}
		}

		public class MatchedCondition {
			public string DisplayName { get; private set; }
			public Condition Match;
			public int ConditionID;
			public Core.Aircraft AircraftInfo;

			public MatchedCondition(int conditionID, Condition c, Core.Aircraft aircraftInfo) {
				Match = c;
				AircraftInfo = aircraftInfo;
				ConditionID = conditionID;
				string emailpropertyName = Core.vrsPropertyData[c.emailProperty][2];
				string emailpropertyText = (aircraftInfo.GetProperty(emailpropertyName) != null) ? " (" + c.emailProperty + ": " + aircraftInfo.GetProperty(emailpropertyName) + ")" : "";
				DisplayName = "Condition: " + c.conditionName + emailpropertyText;
			}
		}

		static Core() {
			//LEGEND
			//A = Equals/Not Equals
			//B = Higher Than + Lower Than
			//C = True/False Boolean
			//D = Starts With + Ends With
			//E = Contains

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
			vrsPropertyData.Add(vrsProperty.Squawk, new string[] { "Number", "A", "Sqk", "The squawk." });
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

			comparisonTypes.Add("A", new string[] { "Equals", "Not Equals" });
			comparisonTypes.Add("B", new string[] { "Higher Than", "Lower Than" });
			comparisonTypes.Add("C", new string[] { "Equals", "Not Equals" });
			comparisonTypes.Add("D", new string[] { "Starts With", "Ends With" });
			comparisonTypes.Add("E", new string[] { "Contains" });
		}
	}
}
