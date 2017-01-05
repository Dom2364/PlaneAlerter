using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlaneAlerter {
	public static class Settings {
		public static string senderEmail;
		public static string acListUrl;
		public static bool VRSAuthenticate;
		public static string VRSUsr;
		public static string VRSPwd;
		public static decimal Lat;
		public static decimal Long;
		public static string radarUrl;
		public static int timeoutLength;
		public static int refreshRate;
		public static string SMTPHost;
		public static int SMTPPort;
		public static string SMTPUsr;
		public static string SMTPPwd;
		public static bool SMTPSSL;
		public static bool SettingsLoaded = false;

		public static class EmailContentConfig {
			public static bool ReceiverName;
			public static bool TransponderType;
			public static bool RadarLink;
			public static bool ReportLink;
			public static bool AfLookup;
			public static bool AircraftPhotos;
			public static bool Map;
			public static Core.PropertyListType PropertyList;
		}

		public static Dictionary<string, object> returnSettingsDictionary() {
			Dictionary<string, object> settingsDictionary = new Dictionary<string, object>();
			settingsDictionary.Add("senderEmail", senderEmail);
			settingsDictionary.Add("acListUrl", acListUrl);
			settingsDictionary.Add("radarURL", radarUrl);
			settingsDictionary.Add("VRSUsr", VRSUsr);
			settingsDictionary.Add("VRSPwd", VRSPwd);
			settingsDictionary.Add("Lat", Lat);
			settingsDictionary.Add("Long", Long);
			settingsDictionary.Add("timeoutLength", timeoutLength);
			settingsDictionary.Add("refreshRate", refreshRate);
			settingsDictionary.Add("SMTPHost", SMTPHost);
			settingsDictionary.Add("SMTPPort", SMTPPort);
			settingsDictionary.Add("SMTPUsr", SMTPUsr);
			settingsDictionary.Add("SMTPPwd", SMTPPwd);
			settingsDictionary.Add("SMTPSSL", SMTPSSL);
			return settingsDictionary;
		}

		public static Dictionary<string, object> returnECCDictionary() {
			Dictionary<string, object> eccDictionary = new Dictionary<string, object>();
			eccDictionary.Add("ReceiverName", EmailContentConfig.ReceiverName);
			eccDictionary.Add("TransponderType", EmailContentConfig.TransponderType);
			eccDictionary.Add("RadarLink", EmailContentConfig.RadarLink);
			eccDictionary.Add("ReportLink", EmailContentConfig.ReportLink);
			eccDictionary.Add("AfLookup", EmailContentConfig.AfLookup);
			eccDictionary.Add("AircraftPhotos", EmailContentConfig.AircraftPhotos);
			eccDictionary.Add("Map", EmailContentConfig.Map);
			eccDictionary.Add("PropertyList", EmailContentConfig.PropertyList.ToString());
			return eccDictionary;
		}

		public static void updateSettings(bool update) {
			if (Core.loopThread != null)
				Core.UI.writeToConsole("Reloading Settings...", Color.White);
			VRSAuthenticate = (VRSUsr != "");

			Email.mailClient = new SmtpClient(SMTPHost);
			Email.mailClient.Port = SMTPPort;
			Email.mailClient.Credentials = new NetworkCredential(SMTPUsr, SMTPPwd);
			Email.mailClient.EnableSsl = SMTPSSL;

			foreach (TreeNode settingsGroupNode in Core.UI.conditionTreeView.Nodes[1].Nodes) {
				settingsGroupNode.Nodes.Clear();
			}

			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("Sender Email: " + senderEmail);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Host: " + SMTPHost + ":" + SMTPPort);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP SSL: " + SMTPSSL);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Username: " + SMTPUsr);
			Core.UI.conditionTreeView.Nodes[1].Nodes[0].Nodes.Add("SMTP Password: *Censored*");

			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("AircraftList.json Url: " + acListUrl);
			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Radar Url: " + radarUrl);
			if (VRSAuthenticate) {
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Username: " + VRSUsr);
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Password: *Censored*");
			}
			else
				Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("VRS Authentication: " + VRSAuthenticate);
			Core.UI.conditionTreeView.Nodes[1].Nodes[1].Nodes.Add("Removal Timeout: " + timeoutLength + " secs");

			if (update)
				ThreadManager.Restart();
		}

		static Settings() {
			acListUrl = "";
			if (!File.Exists("settings.json")) {
				Core.UI.writeToConsole("No settings file! Creating one...", Color.White);
				SMTPPort = 21;
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(returnSettingsDictionary(), Formatting.Indented));
			}
			JObject settingsJson;
			using (FileStream filestream = new FileStream("settings.json", FileMode.Open))
				using (StreamReader reader = new StreamReader(filestream))
					using (JsonTextReader jsonreader = new JsonTextReader(reader))
						settingsJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
			if (settingsJson == null) {
				SMTPPort = 21;
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(returnSettingsDictionary(), Formatting.Indented));
			}
			else {
				senderEmail = settingsJson["senderEmail"].ToString();
				acListUrl = settingsJson["acListUrl"].ToString();
				if (acListUrl.ToString() != "" && acListUrl.ToString().Length > 3 && acListUrl.Substring(0, 4) != "http")
					acListUrl = "http://" + acListUrl;
				VRSUsr = settingsJson["VRSUsr"].ToString();
				VRSPwd = settingsJson["VRSPwd"].ToString();
				Lat = Convert.ToDecimal(settingsJson["Lat"]);
				Long = Convert.ToDecimal(settingsJson["Long"]);
				VRSAuthenticate = (VRSUsr != "");
				timeoutLength = Convert.ToInt32(settingsJson["timeoutLength"]);
				refreshRate = Convert.ToInt32(settingsJson["refreshRate"]);
				radarUrl = settingsJson["radarURL"].ToString();
				SMTPHost = settingsJson["SMTPHost"].ToString();
				try {
					SMTPPort = Convert.ToInt32(settingsJson["SMTPPort"]);
					if (SMTPPort == 0) SMTPPort = 21;
				}
				catch {
					SMTPPort = 21;
				}
				SMTPUsr = settingsJson["SMTPUsr"].ToString();
				SMTPPwd = settingsJson["SMTPPwd"].ToString();
				SMTPSSL = (settingsJson["SMTPSSL"].ToString().ToLower() == "true");
			}

			Email.mailClient = new SmtpClient(SMTPHost);
			Email.mailClient.Port = SMTPPort;
			Email.mailClient.Credentials = new NetworkCredential(SMTPUsr, SMTPPwd);
			Email.mailClient.EnableSsl = SMTPSSL;
			settingsJson.RemoveAll();
			settingsJson = null;
			Core.UI.writeToConsole("Settings Loaded", Color.White);

			if (!File.Exists("emailconfig.json")) {
				Core.UI.writeToConsole("No email content config file! Creating one...", Color.White);
				EmailContentConfig.AfLookup = true;
				EmailContentConfig.AircraftPhotos = true;
				EmailContentConfig.Map = true;
				EmailContentConfig.PropertyList = Core.PropertyListType.All;
				EmailContentConfig.RadarLink = true;
				EmailContentConfig.ReportLink = true;
				EmailContentConfig.ReceiverName = true;
				EmailContentConfig.TransponderType = true;
				File.WriteAllText("emailconfig.json", JsonConvert.SerializeObject(returnECCDictionary(), Formatting.Indented));
			}

			JObject eccJson;
			using (FileStream filestream = new FileStream("emailconfig.json", FileMode.Open))
				using (StreamReader reader = new StreamReader(filestream))
					using (JsonTextReader jsonreader = new JsonTextReader(reader))
						eccJson = JsonSerializer.Create().Deserialize<JObject>(jsonreader);
			EmailContentConfig.ReceiverName = (bool)eccJson["ReceiverName"];
			EmailContentConfig.TransponderType = (bool)eccJson["TransponderType"];
			EmailContentConfig.RadarLink = (bool)eccJson["RadarLink"];
			EmailContentConfig.ReportLink = (bool)eccJson["ReportLink"];
			EmailContentConfig.AfLookup = (bool)eccJson["AfLookup"];
			EmailContentConfig.AircraftPhotos = (bool)eccJson["AircraftPhotos"];
			EmailContentConfig.Map = (bool)eccJson["Map"];
			EmailContentConfig.PropertyList = (Core.PropertyListType)Enum.Parse(typeof(Core.PropertyListType), eccJson["PropertyList"].ToString());
			eccJson.RemoveAll();
			eccJson = null;
			Core.UI.writeToConsole("Email Content Config Loaded", Color.White);

			updateSettings(false);
			SettingsLoaded = true;
		}
	}
}
