using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Models;

namespace PlaneAlerter {
	/// <summary>
	/// Class for storing settings
	/// </summary>
	public static class Settings {
		/// <summary>
		/// Email to send alerts from
		/// </summary>
		public static string senderEmail;

		/// <summary>
		/// AircraftList.json url
		/// </summary>
		public static string acListUrl;

		/// <summary>
		/// Do we need authentication for VRS?
		/// </summary>
		public static bool VRSAuthenticate;

		/// <summary>
		/// VRS username
		/// </summary>
		public static string VRSUsr;

		/// <summary>
		/// VRS password
		/// </summary>
		public static string VRSPwd;

		/// <summary>
		/// Latitude of user
		/// </summary>
		public static decimal Lat;

		/// <summary>
		/// Longitude of user
		/// </summary>
		public static decimal Long;

		/// <summary>
		/// Radar URL
		/// </summary>
		public static string radarUrl;

		/// <summary>
		/// Centre maps on aircraft
		/// </summary>
		public static bool centreMapOnAircraft;

		/// <summary>
		/// Removal timeout
		/// </summary>
		public static int removalTimeout;

		/// <summary>
		/// Checker refresh rate
		/// </summary>
		public static int refreshRate;

		/// <summary>
		/// Start checker on program start
		/// </summary>
		public static bool startOnStart;

		/// <summary>
		/// Checker request timeout
		/// </summary>
		public static int timeout;

		/// <summary>
		/// Show notifications
		/// </summary>
		public static bool showNotifications;

		/// <summary>
		/// Sound alerts
		/// </summary>
		public static bool soundAlerts;

		/// <summary>
		/// Ignore aircraft beyond a specific distance?
		/// </summary>
		public static bool filterDistance;

		/// <summary>
		/// Ignore aircraft above a specific altitude?
		/// </summary>
		public static bool filterAltitude;

		/// <summary>
		/// Ignore Mode-S (when filtering by distance)
		/// </summary>
		public static bool ignoreModeS;

		/// <summary>
		/// What distance to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public static double ignoreDistance;

		/// <summary>
		/// What altitude to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public static int ignoreAltitude;

		/// <summary>
		/// Filter by receiver?
		/// </summary>
		public static bool filterReceiver;

		/// <summary>
		/// Selected receiver to filter aircraft with
		/// </summary>
		public static int filterReceiverId;

		/// <summary>
		/// Number of checks between trail updates
		/// </summary>
		public static int trailsUpdateFrequency;

		/// <summary>
		/// SMTP Host
		/// </summary>
		public static string SMTPHost;

		/// <summary>
		/// SMTP Port
		/// </summary>
		public static int SMTPPort;

		/// <summary>
		/// SMTP Username
		/// </summary>
		public static string SMTPUsr;

		/// <summary>
		/// SMTP Password
		/// </summary>
		public static string SMTPPwd;

		/// <summary>
		/// Do we use SSL?
		/// </summary>
		public static bool SMTPSSL;
		
		/// <summary>
		/// Are the settings loaded?
		/// </summary>
		public static bool SettingsLoaded = false;


		/// <summary>
		/// List of Twitter user credentials
		/// </summary>
		public static Dictionary<string, string[]> TwitterUsers = new Dictionary<string, string[]>();

		/// <summary>
		/// Email content config
		/// </summary>
		public static class EmailContentConfig {
			public static bool ReceiverName;
			public static bool TransponderType;
			public static bool RadarLink;
			public static bool ReportLink;
			public static bool AfLookup;
			public static bool AircraftPhotos;
			public static bool Map;
            public static bool TwitterOptimised;
			public static bool KMLfile;
			public static Core.PropertyListType PropertyList;
		}

		/// <summary>
		/// Get a dictionary of settings data
		/// </summary>
		/// <returns>A dictionary of settings data</returns>
		public static Dictionary<string, object> returnSettingsDictionary() {
			//Create the dictionary, add the settings values then return it
			Dictionary<string, object> settingsDictionary = new Dictionary<string, object>();
			settingsDictionary.Add("senderEmail", senderEmail);
			settingsDictionary.Add("acListUrl", acListUrl);
			settingsDictionary.Add("radarURL", radarUrl);
			settingsDictionary.Add("VRSUsr", VRSUsr);
			settingsDictionary.Add("VRSPwd", VRSPwd);
			settingsDictionary.Add("Lat", Lat);
			settingsDictionary.Add("Long", Long);
			settingsDictionary.Add("timeoutLength", removalTimeout);
			settingsDictionary.Add("refreshRate", refreshRate);
			settingsDictionary.Add("startOnStart", startOnStart);
			settingsDictionary.Add("timeout", timeout);
			settingsDictionary.Add("showNotifications", showNotifications);
			settingsDictionary.Add("soundAlerts", soundAlerts);
			settingsDictionary.Add("ignoreDistance", ignoreDistance);
			settingsDictionary.Add("ignoreAltitude", ignoreAltitude);
			settingsDictionary.Add("filterDistance", filterDistance);
			settingsDictionary.Add("filterAltitude", filterAltitude);
			settingsDictionary.Add("ignoreModeS", ignoreModeS);
			settingsDictionary.Add("filterReceiver", filterReceiver);
			settingsDictionary.Add("filterReceiverId", filterReceiverId);
			settingsDictionary.Add("trailsUpdateFrequency", trailsUpdateFrequency);
			settingsDictionary.Add("centreMapOnAircraft", centreMapOnAircraft);
			settingsDictionary.Add("SMTPHost", SMTPHost);
			settingsDictionary.Add("SMTPPort", SMTPPort);
			settingsDictionary.Add("SMTPUsr", SMTPUsr);
			settingsDictionary.Add("SMTPPwd", SMTPPwd);
			settingsDictionary.Add("SMTPSSL", SMTPSSL);
			settingsDictionary.Add("TwitterUsers", TwitterUsers);
			return settingsDictionary;
		}

		/// <summary>
		/// Get a dictonary of email content config data
		/// </summary>
		/// <returns>A dictionary of email content config data</returns>
		public static Dictionary<string, object> returnECCDictionary() {
			Dictionary<string, object> eccDictionary = new Dictionary<string, object>();
			eccDictionary.Add("ReceiverName", EmailContentConfig.ReceiverName);
			eccDictionary.Add("TransponderType", EmailContentConfig.TransponderType);
			eccDictionary.Add("RadarLink", EmailContentConfig.RadarLink);
			eccDictionary.Add("ReportLink", EmailContentConfig.ReportLink);
			eccDictionary.Add("AfLookup", EmailContentConfig.AfLookup);
			eccDictionary.Add("AircraftPhotos", EmailContentConfig.AircraftPhotos);
			eccDictionary.Add("Map", EmailContentConfig.Map);
            eccDictionary.Add("TwitterOptimised", EmailContentConfig.TwitterOptimised);
            eccDictionary.Add("PropertyList", EmailContentConfig.PropertyList.ToString());
			eccDictionary.Add("KMLfile", EmailContentConfig.KMLfile);
			return eccDictionary;
		}

		/// <summary>
		/// Update settings
		/// </summary>
		/// <param name="update">Is this an update?</param>
		public static void updateSettings(bool update) {
			if (Core.LoopThread != null)
				Core.Ui.writeToConsole("Reloading Settings...", Color.White);
			VRSAuthenticate = (VRSUsr != "");

			//Update UI
			foreach (TreeNode settingsGroupNode in Core.Ui.conditionTreeView.Nodes[1].Nodes)
				settingsGroupNode.Nodes.Clear();

			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + senderEmail);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + SMTPHost + ":" + SMTPPort);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + SMTPSSL);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + SMTPUsr);

			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " + acListUrl);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + radarUrl);
			if (VRSAuthenticate) {
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + VRSUsr);
			}
			else
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + removalTimeout + " secs");
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft further than: " + ignoreDistance + " km away");
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft higher than: " + ignoreAltitude + " ft");
			//If its an update, restart threads
			if (update) ThreadManager.Restart();
		}

        /// <summary>
        /// Load default ecc settings
        /// </summary>
        static void LoadECCDefaults() {
            EmailContentConfig.AfLookup = true;
            EmailContentConfig.AircraftPhotos = true;
            EmailContentConfig.Map = true;
			EmailContentConfig.KMLfile = true;
            EmailContentConfig.PropertyList = Core.PropertyListType.All;
            EmailContentConfig.RadarLink = true;
            EmailContentConfig.ReportLink = true;
            EmailContentConfig.ReceiverName = true;
            EmailContentConfig.TransponderType = true;
            EmailContentConfig.TwitterOptimised = false;
            File.WriteAllText("emailconfig.json", JsonConvert.SerializeObject(returnECCDictionary(), Formatting.Indented));
        }

		/// <summary>
		/// Save Settings
		/// </summary>
		public static void Save() {
			//Serialise and save settings and email config
			string serialisedSettings = JsonConvert.SerializeObject(returnSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);
			string serialisedEcc = JsonConvert.SerializeObject(returnECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}

        //Constructor
        static Settings() {
			//Initialise aircraftlist.json url as empty
			acListUrl = "";

			//Create settings file if one does not exist
			if (!File.Exists("settings.json")) {
				Core.Ui.writeToConsole("No settings file! Creating one...", Color.White);
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(new Dictionary<string, object>(), Formatting.Indented));
			}

			//Deserialise settings file
			JObject settingsJson;
			using (FileStream filestream = new FileStream("settings.json", FileMode.Open))
				using (StreamReader reader = new StreamReader(filestream))
					using (JsonTextReader jsonreader = new JsonTextReader(reader))
						settingsJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);

			//If file could not be parsed, create a new one, else parse settings
			if (settingsJson == null) {
				MessageBox.Show("Unable to parse settings.json, exiting program", "Settings.json parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			else {
				//Copy settings from parsed json
				if (settingsJson["senderEmail"] != null) senderEmail = settingsJson["senderEmail"].ToString();
				if (settingsJson["acListUrl"] != null) acListUrl = settingsJson["acListUrl"].ToString();
				if (acListUrl.ToString() != "" && acListUrl.ToString().Length > 3 && acListUrl.Substring(0, 4) != "http")
					acListUrl = "http://" + acListUrl;
				if (settingsJson["VRSUsr"] != null) VRSUsr = settingsJson["VRSUsr"].ToString();
				if (settingsJson["VRSPwd"] != null) VRSPwd = settingsJson["VRSPwd"].ToString();
				VRSAuthenticate = (VRSUsr != "");
				if (settingsJson["Lat"] != null) Lat = Convert.ToDecimal(settingsJson["Lat"]);
				if (settingsJson["Long"] != null) Long = Convert.ToDecimal(settingsJson["Long"]);
				if (settingsJson["filterDistance"] != null) filterDistance = (settingsJson["filterDistance"].ToString().ToLower() == "true"); else filterDistance = false;
				if (settingsJson["filterAltitude"] != null) filterAltitude = (settingsJson["filterAltitude"].ToString().ToLower() == "true"); else filterAltitude = false;
				if (settingsJson["ignoreModeS"] != null) ignoreModeS = (settingsJson["ignoreModeS"].ToString().ToLower() == "true"); else ignoreModeS = true;
				if (settingsJson["ignoreDistance"] != null) ignoreDistance = Convert.ToDouble(settingsJson["ignoreDistance"]); else ignoreDistance = 30000;
				if (settingsJson["ignoreAltitude"] != null) ignoreAltitude = Convert.ToInt32(settingsJson["ignoreAltitude"]); else ignoreAltitude = 100000;
				if (settingsJson["filterReceiver"] != null) filterReceiver = (settingsJson["filterReceiver"].ToString().ToLower() == "true"); else filterReceiver = false;
				if (settingsJson["filterReceiverId"] != null) filterReceiverId = Convert.ToInt32(settingsJson["filterReceiverId"]); else filterReceiverId = 1;
				if (settingsJson["trailsUpdateFrequency"] != null) trailsUpdateFrequency = Convert.ToInt32(settingsJson["trailsUpdateFrequency"]); else trailsUpdateFrequency = 1;
				if (settingsJson["timeoutLength"] != null) removalTimeout = Convert.ToInt32(settingsJson["timeoutLength"]); else removalTimeout = 60;
				if (settingsJson["refreshRate"] != null) refreshRate = Convert.ToInt32(settingsJson["refreshRate"]); else refreshRate = 60;
				if (refreshRate < 1) refreshRate = 1;
				if (settingsJson["startOnStart"] != null) startOnStart = (settingsJson["startOnStart"].ToString().ToLower() == "true"); else startOnStart = true;
				if (settingsJson["timeout"] != null && Convert.ToInt32(settingsJson["timeout"]) >= 5) timeout = Convert.ToInt32(settingsJson["timeout"]); else timeout = 5;
				if (settingsJson["showNotifications"] != null) showNotifications = (settingsJson["showNotifications"].ToString().ToLower() == "true"); else showNotifications = true;
				if (settingsJson["soundAlerts"] != null) soundAlerts = (settingsJson["soundAlerts"].ToString().ToLower() == "true"); else soundAlerts = true;
				if (settingsJson["centreMapOnAircraft"] != null) centreMapOnAircraft = (settingsJson["centreMapOnAircraft"].ToString().ToLower() == "true"); else centreMapOnAircraft = true;
				if (settingsJson["radarURL"] != null) radarUrl = settingsJson["radarURL"].ToString();
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
				if (settingsJson["SMTPUsr"] != null) SMTPUsr = settingsJson["SMTPUsr"].ToString();
				if (settingsJson["SMTPPwd"] != null) SMTPPwd = settingsJson["SMTPPwd"].ToString();
				if (settingsJson["SMTPSSL"] != null) SMTPSSL = (settingsJson["SMTPSSL"].ToString().ToLower() == "true");
				if (settingsJson["TwitterUsers"] != null) TwitterUsers = settingsJson["TwitterUsers"].ToObject<Dictionary<string, string[]>>();
			}

			//Clear settings json to hopefully save some memory
			settingsJson.RemoveAll();
			settingsJson = null;

			//Log to UI
			Core.Ui.writeToConsole("Settings Loaded", Color.White);

			//If email content config file does not exist, create one
			if (!File.Exists("emailconfig.json")) {
				Core.Ui.writeToConsole("No email content config file! Creating one...", Color.White);
                LoadECCDefaults();
			}

			//Decode ecc json
			JObject eccJson;
			using (FileStream filestream = new FileStream("emailconfig.json", FileMode.Open))
				using (StreamReader reader = new StreamReader(filestream))
					using (JsonTextReader jsonreader = new JsonTextReader(reader))
						eccJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);

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
                EmailContentConfig.PropertyList = (Core.PropertyListType)Enum.Parse(typeof(Core.PropertyListType), eccJson["PropertyList"].ToString());
				EmailContentConfig.KMLfile = (bool)eccJson["KMLfile"];
            }
			catch {
                File.Delete("emailconfig.json");
                LoadECCDefaults();
            }

			//Clear ecc json to hopefully save some memory
			eccJson.RemoveAll();
			eccJson = null;

			//Log to UI
			Core.Ui.writeToConsole("Email Content Config Loaded", Color.White);

			//Update settings
			updateSettings(false);

			//Update twitter accounts list
			Core.Ui.updateTwitterAccounts();

			SettingsLoaded = true;
		}
	}
}
