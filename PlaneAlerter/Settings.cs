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
		/// Removal timeout
		/// </summary>
		public static int removalTimeout;

		/// <summary>
		/// Checker refresh rate
		/// </summary>
		public static int refreshRate;

		/// <summary>
		/// Checker request timeout
		/// </summary>
		public static int timeout;

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
			settingsDictionary.Add("timeout", timeout);
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
			return eccDictionary;
		}

		/// <summary>
		/// Update settings
		/// </summary>
		/// <param name="update">Is this an update?</param>
		public static void updateSettings(bool update) {
			if (Core.loopThread != null)
				Core.UI.writeToConsole("Reloading Settings...", Color.White);
			VRSAuthenticate = (VRSUsr != "");

			//Update mail client info
			Email.mailClient = new SmtpClient(SMTPHost);
			Email.mailClient.Port = SMTPPort;
			Email.mailClient.Credentials = new NetworkCredential(SMTPUsr, SMTPPwd);
			Email.mailClient.EnableSsl = SMTPSSL;

			//Update UI
			foreach (TreeNode settingsGroupNode in Core.UI.conditionTreeView.Nodes[1].Nodes)
				settingsGroupNode.Nodes.Clear();

			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + senderEmail);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + SMTPHost + ":" + SMTPPort);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + SMTPSSL);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + SMTPUsr);

			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " + acListUrl);
			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + radarUrl);
			if (VRSAuthenticate) {
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + VRSUsr);
			}
			else
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + removalTimeout + " secs");

			//If its an update, restart threads
			if (update)
				ThreadManager.StartOrRestart();
		}

        /// <summary>
        /// Load default settings
        /// </summary>
        static void LoadDefaults() {
            SMTPPort = 21;
			timeout = 5;
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(returnSettingsDictionary(), Formatting.Indented));
        }

        /// <summary>
        /// Load default ecc settings
        /// </summary>
        static void LoadECCDefaults() {
            EmailContentConfig.AfLookup = true;
            EmailContentConfig.AircraftPhotos = true;
            EmailContentConfig.Map = true;
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
			string serialisedSettings = JsonConvert.SerializeObject(Settings.returnSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);
			string serialisedEcc = JsonConvert.SerializeObject(Settings.returnECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}

        //Constructor
        static Settings() {
			//Initialise aircraftlist.json url as empty
			acListUrl = "";

			//Create settings file if one does not exist
			if (!File.Exists("settings.json")) {
				Core.UI.writeToConsole("No settings file! Creating one...", Color.White);
                LoadDefaults();
			}

			//Deserialise settings file
			JObject settingsJson;
			using (FileStream filestream = new FileStream("settings.json", FileMode.Open))
				using (StreamReader reader = new StreamReader(filestream))
					using (JsonTextReader jsonreader = new JsonTextReader(reader))
						settingsJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);

			//If file could not be parsed, create a new one, else parse settings
			if (settingsJson == null) {
				SMTPPort = 21;
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(returnSettingsDictionary(), Formatting.Indented));
			}
			else {
				//Copy settings from parsed json
				if (settingsJson["senderEmail"] != null) senderEmail = settingsJson["senderEmail"].ToString();
				if (settingsJson["acListUrl"] != null) acListUrl = settingsJson["acListUrl"].ToString();
				if (acListUrl.ToString() != "" && acListUrl.ToString().Length > 3 && acListUrl.Substring(0, 4) != "http")
					acListUrl = "http://" + acListUrl;
				if (settingsJson["VRSUsr"] != null) VRSUsr = settingsJson["VRSUsr"].ToString();
				if (settingsJson["VRSPwd"] != null) VRSPwd = settingsJson["VRSPwd"].ToString();
				if (settingsJson["Lat"] != null) Lat = Convert.ToDecimal(settingsJson["Lat"]);
				if (settingsJson["Long"] != null) Long = Convert.ToDecimal(settingsJson["Long"]);
				VRSAuthenticate = (VRSUsr != "");
				if (settingsJson["timeoutLength"] != null) removalTimeout = Convert.ToInt32(settingsJson["timeoutLength"]); else removalTimeout = 60;
				if (settingsJson["refreshRate"] != null) refreshRate = Convert.ToInt32(settingsJson["refreshRate"]); else refreshRate = 30;
				if (settingsJson["timeout"] != null && Convert.ToInt32(settingsJson["timeout"]) >= 5) timeout = Convert.ToInt32(settingsJson["timeout"]); else timeout = 30;
				if (settingsJson["radarURL"] != null) radarUrl = settingsJson["radarURL"].ToString();
				if (settingsJson["SMTPHost"] != null) SMTPHost = settingsJson["SMTPHost"].ToString();
				try {
					SMTPPort = Convert.ToInt32(settingsJson["SMTPPort"]);
					if (SMTPPort == 0) SMTPPort = 21;
				}
				catch {
					SMTPPort = 21;
				}
				if (settingsJson["SMTPUsr"] != null) SMTPUsr = settingsJson["SMTPUsr"].ToString();
				if (settingsJson["SMTPPwd"] != null) SMTPPwd = settingsJson["SMTPPwd"].ToString();
				if (settingsJson["SMTPSSL"] != null) SMTPSSL = (settingsJson["SMTPSSL"].ToString().ToLower() == "true");
				if (settingsJson["TwitterUsers"] != null) TwitterUsers = settingsJson["TwitterUsers"].ToObject<Dictionary<string, string[]>>();
			}

			//Set mailclient info
			Email.mailClient = new SmtpClient(SMTPHost);
			Email.mailClient.Port = SMTPPort;
			Email.mailClient.Credentials = new NetworkCredential(SMTPUsr, SMTPPwd);
			Email.mailClient.EnableSsl = SMTPSSL;

			//Clear settings json to hopefully save some memory
			settingsJson.RemoveAll();
			settingsJson = null;

			//Log to UI
			Core.UI.writeToConsole("Settings Loaded", Color.White);

			//If email content config file does not exist, create one
			if (!File.Exists("emailconfig.json")) {
				Core.UI.writeToConsole("No email content config file! Creating one...", Color.White);
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
            }
			catch {
                File.Delete("emailconfig.json");
                LoadECCDefaults();
            }

			//Clear ecc json to hopefully save some memory
			eccJson.RemoveAll();
			eccJson = null;

			//Log to UI
			Core.UI.writeToConsole("Email Content Config Loaded", Color.White);

			//Update settings
			updateSettings(false);
			SettingsLoaded = true;
		}
	}
}
