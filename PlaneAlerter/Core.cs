﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using Match = PlaneAlerter.Models.Match;

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
		public static Dictionary<VrsProperty, string[]> VrsPropertyData { get; set; } = new Dictionary<VrsProperty, string[]>();

		/// <summary>
		/// Types of comparisons usable with triggers
		/// </summary>
		public static Dictionary<string, string[]> ComparisonTypes { get; set; } = new Dictionary<string, string[]>();

		/// <summary>
		/// Properties shown when essential properties selected
		/// </summary>
		public static List<VrsProperty> EssentialProperties { get; set; } = new List<VrsProperty>();

		/// <summary>
		/// List of current matches
		/// </summary>
		public static Dictionary<string, Match> ActiveMatches { get; set; } = new Dictionary<string, Match>();

		/// <summary>
		/// Thread for checking operations
		/// </summary>
		public static Thread? LoopThread { get; set; }

		/// <summary>
		/// Thread for updating statistics
		/// </summary>
		public static Thread? StatsThread { get; set; }

		/// <summary>
		/// Active form
		/// </summary>
		public static Forms.PlaneAlerter Ui { get; set; }

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
			VrsPropertyData.Add(VrsProperty.Time_Tracked, new string[] { "Number", "AB", "TSecs", "The number of seconds that the aircraft has been tracked for." });
			VrsPropertyData.Add(VrsProperty.Receiver, new string[] { "Number", "A", "Rcvr", "The ID of the feed that last supplied information about the aircraft." });
			VrsPropertyData.Add(VrsProperty.Icao, new string[] { "String", "ADE", "Icao", "The ICAO of the aircraft." });
			VrsPropertyData.Add(VrsProperty.Invalid_Icao, new string[] { "Boolean", "C", "Bad", "True if the ICAO is known to be invalid." });
			VrsPropertyData.Add(VrsProperty.Registration, new string[] { "String", "ADE", "Reg", "The registration." });
			VrsPropertyData.Add(VrsProperty.Altitude, new string[] { "Number", "B", "Alt", "The altitude in feet at standard pressure." });
			VrsPropertyData.Add(VrsProperty.Altitude_AMSL, new string[] { "Number", "B", "GAlt", "The altitude adjusted for local air pressure, should be roughly the height above mean sea level." });
			VrsPropertyData.Add(VrsProperty.Altitude_Type, new string[] { "Number", "A", "AltT", "0 = altitude is barometric, 1 = altitude is geometric. Default to barometric until told otherwise." });
			VrsPropertyData.Add(VrsProperty.Air_Pressure, new string[] { "Number", "B", "InHg", "The air pressure in inches of mercury that was used to calculate the AMSL altitude from the standard pressure altitude." });
			VrsPropertyData.Add(VrsProperty.Target_Altitude, new string[] { "Number", "AB", "TAlt", "The target altitude, in feet, set on the autopilot / FMS etc." });
			VrsPropertyData.Add(VrsProperty.Callsign, new string[] { "String", "ADE", "Call", "The callsign." });
			VrsPropertyData.Add(VrsProperty.Callsign_Inaccurate, new string[] { "Boolean", "C", "CallSus", "True if the callsign may not be correct." });
			VrsPropertyData.Add(VrsProperty.Latitude, new string[] { "Number", "B", "Lat", "The aircraft's latitude over the ground." });
			VrsPropertyData.Add(VrsProperty.Longitude, new string[] { "Number", "B", "Long", "The aircraft's longitude over the ground." });
			VrsPropertyData.Add(VrsProperty.Mlat, new string[] { "Boolean", "C", "Mlat", "True if the aircraft's position was determined using Multilateration." });
			VrsPropertyData.Add(VrsProperty.Is_Tisb, new string[] { "Boolean", "C", "Tisb", "True if the last message received was from a TIS-B source." });
			VrsPropertyData.Add(VrsProperty.Speed, new string[] { "Number", "B", "Spd", "The speed in knots." });
			VrsPropertyData.Add(VrsProperty.Speed_Type, new string[] { "Number", "A", "SpdTyp", "The type of speed that Speed represents. Only used with raw feeds. 0/missing = ground speed, 1 = ground speed reversing, 2 = indicated air speed, 3 = true air speed." });
			VrsPropertyData.Add(VrsProperty.Vertical_Speed, new string[] { "Number", "B", "Vsi", "Vertical speed in feet per minute." });
			VrsPropertyData.Add(VrsProperty.Vertical_Speed_Type, new string[] { "Number", "A", "VsiT", "0 = vertical speed is barometric, 1 = vertical speed is geometric. Default to barometric until told otherwise." });
			VrsPropertyData.Add(VrsProperty.Track, new string[] { "Number", "B", "Trak", "Aircraft's track angle across the ground clockwise from 0° north." });
			VrsPropertyData.Add(VrsProperty.Is_Track_Heading, new string[] { "Boolean", "C", "TrkH", "True if Trak is the aircraft's heading, false if it's the ground track. Default to ground track until told otherwise." });
			VrsPropertyData.Add(VrsProperty.Target_Heading, new string[] { "Number", "B", "TTrk", "The track or heading currently set on the aircraft's autopilot or FMS." });
			VrsPropertyData.Add(VrsProperty.Aircraft_Model_Icao, new string[] { "String", "ADE", "Type", "The aircraft model's ICAO type code." });
			VrsPropertyData.Add(VrsProperty.Aircraft_Model, new string[] { "String", "AE", "Mdl", "A description of the aircraft's model." });
			VrsPropertyData.Add(VrsProperty.Manufacturer, new string[] { "String", "A", "Man", "The manufacturer of the aircraft." });
			VrsPropertyData.Add(VrsProperty.Serial, new string[] { "String", "ADE", "CNum", "The aircraft's construction or serial number." });
			VrsPropertyData.Add(VrsProperty.Departure_Airport, new string[] { "String", "AE", "From", "The code and name of the departure airport." });
			VrsPropertyData.Add(VrsProperty.Arrival_Airport, new string[] { "String", "AE", "To", "The code and name of the arrival airport." });
			VrsPropertyData.Add(VrsProperty.Operator, new string[] { "String", "AE", "Op", "The name of the aircraft's operator." });
			VrsPropertyData.Add(VrsProperty.Operator_Icao_Code, new string[] { "String", "A", "OpIcao", "The operator's ICAO code." });
			VrsPropertyData.Add(VrsProperty.Squawk, new string[] { "Number", "ABD", "Sqk", "The squawk." });
			VrsPropertyData.Add(VrsProperty.Ident, new string[] { "Boolean", "C", "Ident", "True if the aircraft is squawking ident." });
			VrsPropertyData.Add(VrsProperty.Is_In_Emergency, new string[] { "Boolean", "C", "Help", "True if the aircraft is transmitting an emergency squawk." });
			VrsPropertyData.Add(VrsProperty.Distance, new string[] { "Number", "B", "Dst", "The distance to the aircraft in kilometres." });
			VrsPropertyData.Add(VrsProperty.Bearing, new string[] { "Number", "B", "Brng", "The bearing to the aircraft from 0° north" });
			VrsPropertyData.Add(VrsProperty.Wake_Turbulence_Category, new string[] { "Number", "AB", "WTC", "The wake turbulence category of the aircraft. 1 = none, 2 = light, 3 = medium, 4 = heavy" });
			VrsPropertyData.Add(VrsProperty.Engines, new string[] { "Number", "AB", "Engines", "The number of engines the aircraft has." });
			VrsPropertyData.Add(VrsProperty.Engine_Type, new string[] { "Number", "A", "EngType", "The type of engine the aircraft uses. 0 = none, 1 = piston, 2 = turbo, 3 = jet, 4 = electric" });
			VrsPropertyData.Add(VrsProperty.Engine_Mount, new string[] { "Number", "A", "EngMount", "The placement of engines on the aircraft. 0 = unknown, 1 = aft mounted, 2 = wing buried, 3 = fuselage buried, 4 = nose mounted, 5 = wing mounted" });
			VrsPropertyData.Add(VrsProperty.Species, new string[] { "Number", "A", "Species", "The species of the aircraft (helicopter, jet etc.). 0 = none, 1 = landplane, 2 = seaplane, 3 = amphibian, 4 = helicopter, 5 = gyrocopter, 6 = tiltwing, 7 = ground vehicle, 8 = tower" });
			VrsPropertyData.Add(VrsProperty.Is_Military, new string[] { "Boolean", "C", "Mil", "True if the aircraft appears to be operated by the military." });
			VrsPropertyData.Add(VrsProperty.Registered_Country, new string[] { "String", "A", "Cou", "The country that the aircraft is registered to." });
			VrsPropertyData.Add(VrsProperty.Flight_Count, new string[] { "Number", "AB", "FlightsCount", "The number of Flights records the aircraft has in the database." });
			VrsPropertyData.Add(VrsProperty.Message_Count, new string[] { "Number", "AB", "CMsgs", "The count of messages received for the aircraft." });
			VrsPropertyData.Add(VrsProperty.Is_On_Ground, new string[] { "Boolean", "C", "Gnd", "True if the aircraft is on the ground." });
			VrsPropertyData.Add(VrsProperty.User_Tag, new string[] { "String", "AE", "Tag", "The user tag found for the aircraft in the BaseStation.sqb local database." });
			VrsPropertyData.Add(VrsProperty.Notes, new string[] { "String", "AE", "Notes", "The notes found for the aircraft in the BaseStation.sqb local database." });
			VrsPropertyData.Add(VrsProperty.Is_Interesting, new string[] { "Boolean", "C", "Interested", "True if the aircraft is flagged as interesting in the BaseStation.sqb local database." });
			VrsPropertyData.Add(VrsProperty.Transponder_Type, new string[] { "Number", "AB", "Trt", "Transponder type. 0 = Unknown, 1 = Mode-S, 2 = ADS-B (unknown version), 3 = ADS-B 1, 4 = ADS-B 2." });
			VrsPropertyData.Add(VrsProperty.Year, new string[] { "Number", "AB", "Year", "The year the aircraft was manufactured." });
			VrsPropertyData.Add(VrsProperty.Has_Signal_Level, new string[] { "Boolean", "C", "HasSig", "True if the aircraft has a signal level associated with it." });
			VrsPropertyData.Add(VrsProperty.Signal_Level, new string[] { "Number", "B", "Sig", "The signal level for the last message received from the aircraft, as reported by the receiver. Not all receivers pass signal levels. The value's units are receiver-dependent." });
			VrsPropertyData.Add(VrsProperty.Is_Ferry_Flight, new string[] { "Boolean", "C", "IsFerryFlight", "True if this is a ferry flight." });
			VrsPropertyData.Add(VrsProperty.Is_Charter_Flight, new string[] { "Boolean", "C", "IsCharterFlight", "True if this is a charter flight." });

			//Add essential properties to list
			EssentialProperties.Add(VrsProperty.Receiver);
			EssentialProperties.Add(VrsProperty.Icao);
			EssentialProperties.Add(VrsProperty.Registration);
			EssentialProperties.Add(VrsProperty.Altitude);
			EssentialProperties.Add(VrsProperty.Callsign);
			EssentialProperties.Add(VrsProperty.Speed);
			EssentialProperties.Add(VrsProperty.Vertical_Speed);
			EssentialProperties.Add(VrsProperty.Aircraft_Model);
			EssentialProperties.Add(VrsProperty.Aircraft_Model_Icao);
			EssentialProperties.Add(VrsProperty.Departure_Airport);
			EssentialProperties.Add(VrsProperty.Arrival_Airport);
			EssentialProperties.Add(VrsProperty.Operator);
			EssentialProperties.Add(VrsProperty.Operator_Icao_Code);
			EssentialProperties.Add(VrsProperty.Squawk);
			EssentialProperties.Add(VrsProperty.Registered_Country);
			EssentialProperties.Add(VrsProperty.Transponder_Type);
			EssentialProperties.Add(VrsProperty.Manufacturer);

			//Add comparison types
			ComparisonTypes.Add("A", new string[] { "Equals", "Not Equals" });
			ComparisonTypes.Add("B", new string[] { "Higher Than", "Lower Than" });
			ComparisonTypes.Add("C", new string[] { "Equals", "Not Equals" });
			ComparisonTypes.Add("D", new string[] { "Starts With", "Ends With" });
			ComparisonTypes.Add("E", new string[] { "Contains" });
		}

		public static void LogAlert(Condition condition, Aircraft aircraft, string receiver, bool isFirst) {
			try {
				string message = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm")} | {condition.Name} | {receiver} | {(isFirst?"FIRST":"LAST")} CONTACT ALERT: " + Environment.NewLine;
				message += aircraft.ToJson() + Environment.NewLine + Environment.NewLine;

				File.AppendAllText("alerts.log", message);
			}
			catch (Exception e) {
				Ui.WriteToConsole("ERROR: Error writing to alerts.log file: " + e.Message, Color.Red);
			}
		}

		/// <summary>
		/// Parse a custom format string to replace property names with values
		/// </summary>
		/// <returns>Parsed string</returns>
		public static string ParseCustomFormatString(string format, Aircraft aircraft, Condition condition) {
			Dictionary<string, string> variables = new Dictionary<string, string> {
				{ "ConditionName", condition.Name },
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
					if (EnumUtils.TryGetConvertedValue(info[2], value, out string convertedvalue)) {
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
			if (string.IsNullOrWhiteSpace(Settings.RadarUrl)) {
				Ui.WriteToConsole("ERROR: Please enter radar URL in settings", Color.Red);
				return "";
			}
			if (!Settings.RadarUrl.ToLower().Contains("virtualradar")) {
				Ui.WriteToConsole("WARNING: Radar URL must end with /VirtualRadar/ for report links to work", Color.Orange);
				return "";
			}


			reportUrl += Settings.RadarUrl;
			if (Settings.RadarUrl[Settings.RadarUrl.Length - 1] != '/') reportUrl += "/";

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
				if (Settings.CentreMapOnAircraft) {
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
