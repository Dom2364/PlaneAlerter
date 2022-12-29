using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;

namespace PlaneAlerter {
	/// <summary>
	/// Class for storing settings
	/// </summary>
	public static class Settings {
		/// <summary>
		/// Email to send alerts from
		/// </summary>
		public static string SenderEmail { get; set; }

		/// <summary>
		/// AircraftList.json url
		/// </summary>
		public static string AircraftListUrl { get; set; }

		/// <summary>
		/// Do we need authentication for VRS?
		/// </summary>
		public static bool VRSAuthenticate { get; set; }

		/// <summary>
		/// VRS username
		/// </summary>
		public static string VRSUser { get; set; }

		/// <summary>
		/// VRS password
		/// </summary>
		public static string VRSPassword { get; set; }

		/// <summary>
		/// Latitude of user
		/// </summary>
		public static decimal Lat { get; set; }

		/// <summary>
		/// Longitude of user
		/// </summary>
		public static decimal Long { get; set; }

		/// <summary>
		/// Radar URL
		/// </summary>
		public static string RadarUrl { get; set; }

		/// <summary>
		/// Centre maps on aircraft
		/// </summary>
		public static bool CentreMapOnAircraft { get; set; }

		/// <summary>
		/// Removal timeout
		/// </summary>
		public static int RemovalTimeout { get; set; }

		/// <summary>
		/// Checker refresh rate
		/// </summary>
		public static int RefreshRate { get; set; }

		/// <summary>
		/// Start checker on program start
		/// </summary>
		public static bool StartOnStart { get; set; }

		/// <summary>
		/// Checker request timeout
		/// </summary>
		public static int Timeout { get; set; }

		/// <summary>
		/// Show notifications
		/// </summary>
		public static bool ShowNotifications { get; set; }

		/// <summary>
		/// Sound alerts
		/// </summary>
		public static bool SoundAlerts { get; set; }

		/// <summary>
		/// Ignore aircraft beyond a specific distance?
		/// </summary>
		public static bool FilterDistance { get; set; }

		/// <summary>
		/// Ignore aircraft above a specific altitude?
		/// </summary>
		public static bool FilterAltitude { get; set; }

		/// <summary>
		/// Ignore Mode-S (when filtering by distance)
		/// </summary>
		public static bool IgnoreModeS { get; set; }

		/// <summary>
		/// What distance to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public static double IgnoreDistance { get; set; }

		/// <summary>
		/// What altitude to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public static int IgnoreAltitude { get; set; }

		/// <summary>
		/// Filter by receiver?
		/// </summary>
		public static bool FilterReceiver { get; set; }

		/// <summary>
		/// Selected receiver to filter aircraft with
		/// </summary>
		public static int FilterReceiverId { get; set; }

		/// <summary>
		/// Number of checks between trail updates
		/// </summary>
		public static int TrailsUpdateFrequency { get; set; }

		/// <summary>
		/// SMTP Host
		/// </summary>
		public static string SMTPHost { get; set; }

		/// <summary>
		/// SMTP Port
		/// </summary>
		public static int SMTPPort { get; set; }

		/// <summary>
		/// SMTP Username
		/// </summary>
		public static string SMTPUser { get; set; }

		/// <summary>
		/// SMTP Password
		/// </summary>
		public static string SMTPPassword { get; set; }

		/// <summary>
		/// Do we use SSL?
		/// </summary>
		public static bool SMTPSSL { get; set; }

		/// <summary>
		/// Are the settings loaded?
		/// </summary>
		public static bool SettingsLoaded { get; set; } = false;


		/// <summary>
		/// List of Twitter user credentials
		/// </summary>
		public static Dictionary<string, string[]> TwitterUsers { get; set; } = new Dictionary<string, string[]>();

		/// <summary>
		/// Email content config
		/// </summary>
		public static class EmailContentConfig {
			public static bool ReceiverName { get; set; }
			public static bool TransponderType { get; set; }
			public static bool RadarLink { get; set; }
			public static bool ReportLink { get; set; }
			public static bool AfLookup { get; set; }
			public static bool AircraftPhotos { get; set; }
			public static bool Map { get; set; }
			public static bool TwitterOptimised { get; set; }
			public static bool KMLfile { get; set; }
			public static PropertyListType PropertyList { get; set; }
		}

		/// <summary>
		/// Get a dictionary of settings data
		/// </summary>
		/// <returns>A dictionary of settings data</returns>
		public static Dictionary<string, object> GetSettingsDictionary() {
			return new Dictionary<string, object>
			{
				{ "senderEmail", SenderEmail },
				{ "acListUrl", AircraftListUrl },
				{ "radarURL", RadarUrl },
				{ "VRSUsr", VRSUser },
				{ "VRSPwd", VRSPassword },
				{ "Lat", Lat },
				{ "Long", Long },
				{ "timeoutLength", RemovalTimeout },
				{ "refreshRate", RefreshRate },
				{ "startOnStart", StartOnStart },
				{ "timeout", Timeout },
				{ "showNotifications", ShowNotifications },
				{ "soundAlerts", SoundAlerts },
				{ "ignoreDistance", IgnoreDistance },
				{ "ignoreAltitude", IgnoreAltitude },
				{ "filterDistance", FilterDistance },
				{ "filterAltitude", FilterAltitude },
				{ "ignoreModeS", IgnoreModeS },
				{ "filterReceiver", FilterReceiver },
				{ "filterReceiverId", FilterReceiverId },
				{ "trailsUpdateFrequency", TrailsUpdateFrequency },
				{ "centreMapOnAircraft", CentreMapOnAircraft },
				{ "SMTPHost", SMTPHost },
				{ "SMTPPort", SMTPPort },
				{ "SMTPUsr", SMTPUser },
				{ "SMTPPwd", SMTPPassword },
				{ "SMTPSSL", SMTPSSL },
				{ "TwitterUsers", TwitterUsers }
			};
		}

		/// <summary>
		/// Get a dictonary of email content config data
		/// </summary>
		/// <returns>A dictionary of email content config data</returns>
		public static Dictionary<string, object> GetECCDictionary() {
			return new Dictionary<string, object>
			{
				{ "ReceiverName", EmailContentConfig.ReceiverName },
				{ "TransponderType", EmailContentConfig.TransponderType },
				{ "RadarLink", EmailContentConfig.RadarLink },
				{ "ReportLink", EmailContentConfig.ReportLink },
				{ "AfLookup", EmailContentConfig.AfLookup },
				{ "AircraftPhotos", EmailContentConfig.AircraftPhotos },
				{ "Map", EmailContentConfig.Map },
				{ "TwitterOptimised", EmailContentConfig.TwitterOptimised },
				{ "PropertyList", EmailContentConfig.PropertyList.ToString() },
				{ "KMLfile", EmailContentConfig.KMLfile }
			};
		}

		/// <summary>
		/// Update settings
		/// </summary>
		/// <param name="update">Is this an update?</param>
		public static void UpdateSettings(bool update) {
			if (Core.LoopThread != null)
				Core.Ui.WriteToConsole("Reloading Settings...", Color.White);
			VRSAuthenticate = (VRSUser != "");

			//Update UI
			foreach (TreeNode settingsGroupNode in Core.Ui.conditionTreeView.Nodes[1].Nodes)
				settingsGroupNode.Nodes.Clear();

			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + SenderEmail);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + SMTPHost + ":" + SMTPPort);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + SMTPSSL);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + SMTPUser);

			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " + AircraftListUrl);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + RadarUrl);
			if (VRSAuthenticate) {
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + VRSUser);
			}
			else
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + RemovalTimeout + " secs");
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft further than: " + IgnoreDistance + " km away");
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft higher than: " + IgnoreAltitude + " ft");
			//If its an update, restart threads
			if (update) ThreadManager.Restart();
		}

        /// <summary>
        /// Load default ecc settings
        /// </summary>
        private static void LoadECCDefaults() {
            EmailContentConfig.AfLookup = true;
            EmailContentConfig.AircraftPhotos = true;
            EmailContentConfig.Map = true;
			EmailContentConfig.KMLfile = true;
            EmailContentConfig.PropertyList = PropertyListType.All;
            EmailContentConfig.RadarLink = true;
            EmailContentConfig.ReportLink = true;
            EmailContentConfig.ReceiverName = true;
            EmailContentConfig.TransponderType = true;
            EmailContentConfig.TwitterOptimised = false;
            File.WriteAllText("emailconfig.json", JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented));
        }

		/// <summary>
		/// Save Settings
		/// </summary>
		public static void Save() {
			//Serialise and save settings and email config
			var serialisedSettings = JsonConvert.SerializeObject(GetSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);
			var serialisedEcc = JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}

        //Constructor
        static Settings() {
			//Initialise aircraftlist.json url as empty
			AircraftListUrl = "";

			//Create settings file if one does not exist
			if (!File.Exists("settings.json")) {
				Core.Ui.WriteToConsole("No settings file! Creating one...", Color.White);
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(new Dictionary<string, object>(), Formatting.Indented));
			}

			//Deserialise settings file
			JObject? settingsJson;
			using (var fileStream = new FileStream("settings.json", FileMode.Open))
				using (var reader = new StreamReader(fileStream))
					using (var jsonReader = new JsonTextReader(reader))
						settingsJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);

			//If file could not be parsed, create a new one, else parse settings
			if (settingsJson == null) {
				MessageBox.Show("Unable to parse settings.json, exiting program", "Settings.json parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			else {
				//Copy settings from parsed json
				if (settingsJson["senderEmail"] != null) SenderEmail = settingsJson["senderEmail"].ToString();
				if (settingsJson["acListUrl"] != null) AircraftListUrl = settingsJson["acListUrl"].ToString();
				if (AircraftListUrl.ToString() != "" && AircraftListUrl.ToString().Length > 3 && AircraftListUrl.Substring(0, 4) != "http")
					AircraftListUrl = "http://" + AircraftListUrl;
				if (settingsJson["VRSUsr"] != null) VRSUser = settingsJson["VRSUsr"].ToString();
				if (settingsJson["VRSPwd"] != null) VRSPassword = settingsJson["VRSPwd"].ToString();
				VRSAuthenticate = (VRSUser != "");
				if (settingsJson["Lat"] != null) Lat = Convert.ToDecimal(settingsJson["Lat"]);
				if (settingsJson["Long"] != null) Long = Convert.ToDecimal(settingsJson["Long"]);
				if (settingsJson["filterDistance"] != null) FilterDistance = (settingsJson["filterDistance"].ToString().ToLower() == "true"); else FilterDistance = false;
				if (settingsJson["filterAltitude"] != null) FilterAltitude = (settingsJson["filterAltitude"].ToString().ToLower() == "true"); else FilterAltitude = false;
				if (settingsJson["ignoreModeS"] != null) IgnoreModeS = (settingsJson["ignoreModeS"].ToString().ToLower() == "true"); else IgnoreModeS = true;
				if (settingsJson["ignoreDistance"] != null) IgnoreDistance = Convert.ToDouble(settingsJson["ignoreDistance"]); else IgnoreDistance = 30000;
				if (settingsJson["ignoreAltitude"] != null) IgnoreAltitude = Convert.ToInt32(settingsJson["ignoreAltitude"]); else IgnoreAltitude = 100000;
				if (settingsJson["filterReceiver"] != null) FilterReceiver = (settingsJson["filterReceiver"].ToString().ToLower() == "true"); else FilterReceiver = false;
				if (settingsJson["filterReceiverId"] != null) FilterReceiverId = Convert.ToInt32(settingsJson["filterReceiverId"]); else FilterReceiverId = 1;
				if (settingsJson["trailsUpdateFrequency"] != null) TrailsUpdateFrequency = Convert.ToInt32(settingsJson["trailsUpdateFrequency"]); else TrailsUpdateFrequency = 1;
				if (settingsJson["timeoutLength"] != null) RemovalTimeout = Convert.ToInt32(settingsJson["timeoutLength"]); else RemovalTimeout = 60;
				if (settingsJson["refreshRate"] != null) RefreshRate = Convert.ToInt32(settingsJson["refreshRate"]); else RefreshRate = 60;
				if (RefreshRate < 1) RefreshRate = 1;
				if (settingsJson["startOnStart"] != null) StartOnStart = (settingsJson["startOnStart"].ToString().ToLower() == "true"); else StartOnStart = true;
				if (settingsJson["timeout"] != null && Convert.ToInt32(settingsJson["timeout"]) >= 5) Timeout = Convert.ToInt32(settingsJson["timeout"]); else Timeout = 5;
				if (settingsJson["showNotifications"] != null) ShowNotifications = (settingsJson["showNotifications"].ToString().ToLower() == "true"); else ShowNotifications = true;
				if (settingsJson["soundAlerts"] != null) SoundAlerts = (settingsJson["soundAlerts"].ToString().ToLower() == "true"); else SoundAlerts = true;
				if (settingsJson["centreMapOnAircraft"] != null) CentreMapOnAircraft = (settingsJson["centreMapOnAircraft"].ToString().ToLower() == "true"); else CentreMapOnAircraft = true;
				if (settingsJson["radarURL"] != null) RadarUrl = settingsJson["radarURL"].ToString();
				if (settingsJson["SMTPHost"] != null) SMTPHost = settingsJson["SMTPHost"].ToString();
				
				if (settingsJson["SMTPPort"] != null) {
					try {
						SMTPPort = Convert.ToInt32(settingsJson["SMTPPort"]);
						if (SMTPPort == 0) SMTPPort = 21;
					}
					catch {
						SMTPPort = 21;
					}
				} else SMTPPort = 21;
				if (settingsJson["SMTPUsr"] != null) SMTPUser = settingsJson["SMTPUsr"].ToString();
				if (settingsJson["SMTPPwd"] != null) SMTPPassword = settingsJson["SMTPPwd"].ToString();
				if (settingsJson["SMTPSSL"] != null) SMTPSSL = (settingsJson["SMTPSSL"].ToString().ToLower() == "true");
				if (settingsJson["TwitterUsers"] != null) TwitterUsers = settingsJson["TwitterUsers"].ToObject<Dictionary<string, string[]>>();

				//Clear settings json to hopefully save some memory
				settingsJson.RemoveAll();
			}

			//Log to UI
			Core.Ui.WriteToConsole("Settings Loaded", Color.White);

			//If email content config file does not exist, create one
			if (!File.Exists("emailconfig.json")) {
				Core.Ui.WriteToConsole("No email content config file! Creating one...", Color.White);
                LoadECCDefaults();
			}

			//Decode ecc json
			JObject? eccJson;
			using (var fileStream = new FileStream("emailconfig.json", FileMode.Open))
				using (var reader = new StreamReader(fileStream))
					using (var jsonReader = new JsonTextReader(reader))
						eccJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);

            try {
                //Copy ecc from parsed json
                EmailContentConfig.ReceiverName = (bool)eccJson["ReceiverName"];
                EmailContentConfig.TransponderType = (bool)eccJson["TransponderType"];
                EmailContentConfig.RadarLink = (bool)eccJson["RadarLink"];
                EmailContentConfig.ReportLink = (bool)eccJson["ReportLink"];
                EmailContentConfig.AfLookup = (bool)eccJson["AfLookup"];
                EmailContentConfig.AircraftPhotos = (bool)eccJson["AircraftPhotos"];
                EmailContentConfig.Map = (bool)eccJson["Map"];
                EmailContentConfig.TwitterOptimised = (bool)eccJson["TwitterOptimised"];
                EmailContentConfig.PropertyList = (PropertyListType)Enum.Parse(typeof(PropertyListType), eccJson["PropertyList"].ToString());
				EmailContentConfig.KMLfile = (bool)eccJson["KMLfile"];
            }
			catch {
                File.Delete("emailconfig.json");
                LoadECCDefaults();
            }

			//Clear ecc json to hopefully save some memory
			eccJson.RemoveAll();

			//Log to UI
			Core.Ui.WriteToConsole("Email Content Config Loaded", Color.White);

			//Update settings
			UpdateSettings(false);

			//Update twitter accounts list
			Core.Ui.UpdateTwitterAccounts();

			SettingsLoaded = true;
		}
	}
}
