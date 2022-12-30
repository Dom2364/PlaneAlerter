using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services {
	internal interface ISettingsManagerService
	{
		Settings Settings { get; set; }
		EmailContentConfig EmailContentConfig { get; set; }

		/// <summary>
		/// Are the settings loaded?
		/// </summary>
		bool SettingsLoaded { get; }

		/// <summary>
		/// Update settings
		/// </summary>
		/// <param name="update">Is this an update?</param>
		void UpdateSettings(bool update);

		/// <summary>
		/// Save Settings
		/// </summary>
		void Save();
	}

	/// <summary>
	/// Class for storing settings
	/// </summary>
	internal class SettingsManagerService : ISettingsManagerService
	{
		public Settings Settings { get; set; } = new Settings();
		public EmailContentConfig EmailContentConfig { get; set; } = new EmailContentConfig();

		/// <summary>
		/// Are the settings loaded?
		/// </summary>
		public bool SettingsLoaded { get; } = false;

		//Constructor
		public SettingsManagerService()
		{
			Load();

			//Update settings
			UpdateSettings(false);

			//Update twitter accounts list
			Core.Ui.UpdateTwitterAccounts();

			SettingsLoaded = true;
		}

		/// <summary>
		/// Get a dictionary of settings data
		/// </summary>
		/// <returns>A dictionary of settings data</returns>
		private Dictionary<string, object> GetSettingsDictionary() {
			return new Dictionary<string, object>
			{
				{ "senderEmail", Settings.SenderEmail },
				{ "acListUrl", Settings.AircraftListUrl },
				{ "radarURL", Settings.RadarUrl },
				{ "VRSUsr", Settings.VRSUser },
				{ "VRSPwd", Settings.VRSPassword },
				{ "Lat", Settings.Lat },
				{ "Long", Settings.Long },
				{ "timeoutLength", Settings.RemovalTimeout },
				{ "refreshRate", Settings.RefreshRate },
				{ "startOnStart", Settings.StartOnStart },
				{ "timeout", Settings.Timeout },
				{ "showNotifications", Settings.ShowNotifications },
				{ "soundAlerts", Settings.SoundAlerts },
				{ "ignoreDistance", Settings.IgnoreDistance },
				{ "ignoreAltitude", Settings.IgnoreAltitude },
				{ "filterDistance", Settings.FilterDistance },
				{ "filterAltitude", Settings.FilterAltitude },
				{ "ignoreModeS", Settings.IgnoreModeS },
				{ "filterReceiver", Settings.FilterReceiver },
				{ "filterReceiverId", Settings.FilterReceiverId },
				{ "trailsUpdateFrequency", Settings.TrailsUpdateFrequency },
				{ "centreMapOnAircraft", Settings.CentreMapOnAircraft },
				{ "SMTPHost", Settings.SMTPHost },
				{ "SMTPPort", Settings.SMTPPort },
				{ "SMTPUsr", Settings.SMTPUser },
				{ "SMTPPwd", Settings.SMTPPassword },
				{ "SMTPSSL", Settings.SMTPSSL },
				{ "TwitterUsers", Settings.TwitterUsers }
			};
		}

		/// <summary>
		/// Get a dictonary of email content config data
		/// </summary>
		/// <returns>A dictionary of email content config data</returns>
		private Dictionary<string, object> GetECCDictionary() {
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
		public void UpdateSettings(bool update) {
			if (Core.LoopThread != null)
				Core.Ui.WriteToConsole("Reloading Settings...", Color.White);
			Settings.VRSAuthenticate = (Settings.VRSUser != "");

			//Update UI
			foreach (TreeNode settingsGroupNode in Core.Ui.conditionTreeView.Nodes[1].Nodes)
				settingsGroupNode.Nodes.Clear();

			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + Settings.SenderEmail);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + Settings.SMTPHost + ":" +
			                                                      Settings.SMTPPort);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + Settings.SMTPSSL);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + Settings.SMTPUser);

			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " +
			                                                      Settings.AircraftListUrl);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + Settings.RadarUrl);
			if (Settings.VRSAuthenticate) {
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " +
				                                                      Settings.VRSAuthenticate);
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + Settings.VRSUser);
			}
			else
				Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " +
				                                                      Settings.VRSAuthenticate);
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + Settings.RemovalTimeout + " secs");
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft further than: " +
			                                                      Settings.IgnoreDistance + " km away");
			Core.Ui.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Ignore aircraft higher than: " +
				                                                      Settings.IgnoreAltitude + " ft");
		}

        /// <summary>
        /// Load default ecc settings
        /// </summary>
        private void LoadECCDefaults()
        {
	        EmailContentConfig = new EmailContentConfig
	        {
		        AfLookup = true,
		        AircraftPhotos = true,
		        Map = true,
		        KMLfile = true,
		        PropertyList = PropertyListType.All,
		        RadarLink = true,
		        ReportLink = true,
		        ReceiverName = true,
		        TransponderType = true,
		        TwitterOptimised = false
	        };
            
            File.WriteAllText("emailconfig.json", JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented));
        }

		/// <summary>
		/// Save Settings
		/// </summary>
		public void Save() {
			//Serialise and save settings and email config
			var serialisedSettings = JsonConvert.SerializeObject(GetSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);
			var serialisedEcc = JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}

		private void Load()
		{
			//Initialise aircraftlist.json url as empty
			Settings.AircraftListUrl = "";

			//Create settings file if one does not exist
			if (!File.Exists("settings.json"))
			{
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
			if (settingsJson == null)
			{
				MessageBox.Show("Unable to parse settings.json, exiting program", "Settings.json parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			else
			{
				//Copy settings from parsed json
				if (settingsJson["senderEmail"] != null) Settings.SenderEmail = settingsJson["senderEmail"].ToString();
				if (settingsJson["acListUrl"] != null) Settings.AircraftListUrl = settingsJson["acListUrl"].ToString();
				if (Settings.AircraftListUrl.ToString() != "" && Settings.AircraftListUrl.ToString().Length > 3 && Settings.AircraftListUrl.Substring(0, 4) != "http")
					Settings.AircraftListUrl = "http://" + Settings.AircraftListUrl;
				if (settingsJson["VRSUsr"] != null) Settings.VRSUser = settingsJson["VRSUsr"].ToString();
				if (settingsJson["VRSPwd"] != null) Settings.VRSPassword = settingsJson["VRSPwd"].ToString();
				Settings.VRSAuthenticate = (Settings.VRSUser != "");
				if (settingsJson["Lat"] != null) Settings.Lat = Convert.ToDecimal(settingsJson["Lat"]);
				if (settingsJson["Long"] != null) Settings.Long = Convert.ToDecimal(settingsJson["Long"]);
				if (settingsJson["filterDistance"] != null) Settings.FilterDistance = (settingsJson["filterDistance"].ToString().ToLower() == "true"); else Settings.FilterDistance = false;
				if (settingsJson["filterAltitude"] != null) Settings.FilterAltitude = (settingsJson["filterAltitude"].ToString().ToLower() == "true"); else Settings.FilterAltitude = false;
				if (settingsJson["ignoreModeS"] != null) Settings.IgnoreModeS = (settingsJson["ignoreModeS"].ToString().ToLower() == "true"); else Settings.IgnoreModeS = true;
				if (settingsJson["ignoreDistance"] != null) Settings.IgnoreDistance = Convert.ToDouble(settingsJson["ignoreDistance"]); else Settings.IgnoreDistance = 30000;
				if (settingsJson["ignoreAltitude"] != null) Settings.IgnoreAltitude = Convert.ToInt32(settingsJson["ignoreAltitude"]); else Settings.IgnoreAltitude = 100000;
				if (settingsJson["filterReceiver"] != null) Settings.FilterReceiver = (settingsJson["filterReceiver"].ToString().ToLower() == "true"); else Settings.FilterReceiver = false;
				if (settingsJson["filterReceiverId"] != null) Settings.FilterReceiverId = Convert.ToInt32(settingsJson["filterReceiverId"]); else Settings.FilterReceiverId = 1;
				if (settingsJson["trailsUpdateFrequency"] != null) Settings.TrailsUpdateFrequency = Convert.ToInt32(settingsJson["trailsUpdateFrequency"]); else Settings.TrailsUpdateFrequency = 1;
				if (settingsJson["timeoutLength"] != null) Settings.RemovalTimeout = Convert.ToInt32(settingsJson["timeoutLength"]); else Settings.RemovalTimeout = 60;
				if (settingsJson["refreshRate"] != null) Settings.RefreshRate = Convert.ToInt32(settingsJson["refreshRate"]); else Settings.RefreshRate = 60;
				if (Settings.RefreshRate < 1) Settings.RefreshRate = 1;
				if (settingsJson["startOnStart"] != null) Settings.StartOnStart = (settingsJson["startOnStart"].ToString().ToLower() == "true"); else Settings.StartOnStart = true;
				if (settingsJson["timeout"] != null && Convert.ToInt32(settingsJson["timeout"]) >= 5) Settings.Timeout = Convert.ToInt32(settingsJson["timeout"]); else Settings.Timeout = 5;
				if (settingsJson["showNotifications"] != null) Settings.ShowNotifications = (settingsJson["showNotifications"].ToString().ToLower() == "true"); else Settings.ShowNotifications = true;
				if (settingsJson["soundAlerts"] != null) Settings.SoundAlerts = (settingsJson["soundAlerts"].ToString().ToLower() == "true"); else Settings.SoundAlerts = true;
				if (settingsJson["centreMapOnAircraft"] != null) Settings.CentreMapOnAircraft = (settingsJson["centreMapOnAircraft"].ToString().ToLower() == "true"); else Settings.CentreMapOnAircraft = true;
				if (settingsJson["radarURL"] != null) Settings.RadarUrl = settingsJson["radarURL"].ToString();
				if (settingsJson["SMTPHost"] != null) Settings.SMTPHost = settingsJson["SMTPHost"].ToString();

				if (settingsJson["SMTPPort"] != null)
				{
					try
					{
						Settings.SMTPPort = Convert.ToInt32(settingsJson["SMTPPort"]);
						if (Settings.SMTPPort == 0) Settings.SMTPPort = 21;
					}
					catch
					{
						Settings.SMTPPort = 21;
					}
				}
				else Settings.SMTPPort = 21;
				if (settingsJson["SMTPUsr"] != null) Settings.SMTPUser = settingsJson["SMTPUsr"].ToString();
				if (settingsJson["SMTPPwd"] != null) Settings.SMTPPassword = settingsJson["SMTPPwd"].ToString();
				if (settingsJson["SMTPSSL"] != null) Settings.SMTPSSL = (settingsJson["SMTPSSL"].ToString().ToLower() == "true");
				if (settingsJson["TwitterUsers"] != null) Settings.TwitterUsers = settingsJson["TwitterUsers"].ToObject<Dictionary<string, string[]>>();

				//Clear settings json to hopefully save some memory
				settingsJson.RemoveAll();
			}

			//Log to UI
			Core.Ui.WriteToConsole("Settings Loaded", Color.White);

			//If email content config file does not exist, create one
			if (!File.Exists("emailconfig.json"))
			{
				Core.Ui.WriteToConsole("No email content config file! Creating one...", Color.White);
				LoadECCDefaults();
			}

			//Decode ecc json
			JObject? eccJson;
			using (var fileStream = new FileStream("emailconfig.json", FileMode.Open))
			using (var reader = new StreamReader(fileStream))
			using (var jsonReader = new JsonTextReader(reader))
				eccJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);

			try
			{
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
			catch
			{
				File.Delete("emailconfig.json");
				LoadECCDefaults();
			}

			//Clear ecc json to hopefully save some memory
			eccJson.RemoveAll();

			//Log to UI
			Core.Ui.WriteToConsole("Email Content Config Loaded", Color.White);
		}
	}
}
