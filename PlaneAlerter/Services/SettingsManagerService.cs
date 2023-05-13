using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;
using PlaneAlerter.Extensions;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services {
	internal interface ISettingsManagerService
	{
		Settings Settings { get; set; }
		EmailContentConfig EmailContentConfig { get; set; }

		/// <summary>
		/// Save Settings
		/// </summary>
		void Save();

		/// <summary>
		/// Load Settings
		/// </summary>
		void Load();
	}

	/// <summary>
	/// Class for storing settings
	/// </summary>
	internal class SettingsManagerService : ISettingsManagerService
	{
		private readonly ILoggerWithQueue _logger;

		public Settings Settings { get; set; } = null!;
		public EmailContentConfig EmailContentConfig { get; set; } = null!;
		public bool LoadedSuccessfully { get; set; }

		//Constructor
		public SettingsManagerService(ILoggerWithQueue logger)
		{
			_logger = logger;
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
				{ "flashWindow", Settings.FlashWindow },
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
		/// Get a dictionary of email content config data
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
				{ "KMLfile", EmailContentConfig.KmlFile }
			};
		}

        /// <summary>
        /// Load default ecc settings
        /// </summary>
        private void LoadEccDefaults()
        {
	        EmailContentConfig = new EmailContentConfig(
		        afLookup: true,
		        aircraftPhotos: true,
		        map: true,
		        kmlFile: true,
		        propertyList: PropertyListType.All,
		        radarLink: true,
		        reportLink: true,
		        receiverName: true,
		        transponderType: true,
		        twitterOptimised: false);
            
            File.WriteAllText("emailconfig.json", JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented));
        }

		/// <summary>
		/// Save Settings
		/// </summary>
		public void Save()
		{
			if (!LoadedSuccessfully)
				return;

			//Serialise and save settings and email config
			var serialisedSettings = JsonConvert.SerializeObject(GetSettingsDictionary(), Formatting.Indented);
			File.WriteAllText("settings.json", serialisedSettings);

			var serialisedEcc = JsonConvert.SerializeObject(GetECCDictionary(), Formatting.Indented);
			File.WriteAllText("emailconfig.json", serialisedEcc);
		}

		public void Load()
		{
			//Create settings file if one does not exist
			if (!File.Exists("settings.json"))
			{
				_logger.Log("No settings file! Creating one...", Color.White);
				File.WriteAllText("settings.json", JsonConvert.SerializeObject(new Dictionary<string, object>(), Formatting.Indented));
			}

			//Deserialise settings file
			JObject? settingsJson = null;
			try
			{
				using var fileStream = new FileStream("settings.json", FileMode.Open);
				using var reader = new StreamReader(fileStream);
				using var jsonReader = new JsonTextReader(reader);
				settingsJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);
			}
			catch (Exception e)
			{
				MessageBox.Show($"Unable to deserialise settings.json, exiting program\n\n{e.Message}\n\n{e.StackTrace}", "settings.json parse error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
			}
			
			if (settingsJson == null)
			{
				MessageBox.Show("Error loading settings.json, exiting program", "settings.json error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
			}

			//Copy settings from parsed json
			//Don't use RequiredValue here as creating a new file due to it missing will not have any values
			try
			{
				Settings = new Settings(
					settingsJson.OptionalValue<string>("senderEmail") ?? "",
					settingsJson.OptionalValue<string>("acListUrl") ?? "",
					settingsJson.OptionalValue<string>("VRSUsr") ?? "",
					settingsJson.OptionalValue<string>("VRSPwd") ?? "",
					settingsJson.OptionalValueStruct<decimal>("Lat") ?? 0,
					settingsJson.OptionalValueStruct<decimal>("Long") ?? 0,
					settingsJson.OptionalValue<string>("radarURL") ?? "",
					settingsJson.OptionalValueStruct<bool>("centreMapOnAircraft") ?? true,
					settingsJson.OptionalValueStruct<int>("timeoutLength") ?? 60,
					settingsJson.OptionalValueStruct<int>("refreshRate") ?? 60,
					settingsJson.OptionalValueStruct<bool>("startOnStart") ?? true,
					settingsJson.OptionalValueStruct<int>("timeout") ?? 5,
					settingsJson.OptionalValueStruct<bool>("showNotifications") ?? true,
					settingsJson.OptionalValueStruct<bool>("soundAlerts") ?? false,
					settingsJson.OptionalValueStruct<bool>("flashWindow") ?? false,
					settingsJson.OptionalValueStruct<bool>("filterDistance") ?? false,
					settingsJson.OptionalValueStruct<bool>("filterAltitude") ?? false,
					settingsJson.OptionalValueStruct<bool>("ignoreModeS") ?? true,
					settingsJson.OptionalValueStruct<double>("ignoreDistance") ?? 30000,
					settingsJson.OptionalValueStruct<int>("ignoreAltitude") ?? 100000,
					settingsJson.OptionalValueStruct<bool>("filterReceiver") ?? false,
					settingsJson.OptionalValueStruct<int>("filterReceiverId") ?? 1,
					settingsJson.OptionalValueStruct<int>("trailsUpdateFrequency") ?? 1,
					settingsJson.OptionalValue<string>("SMTPHost") ?? "",
					settingsJson.OptionalValueStruct<int>("SMTPPort") ?? 21,
					settingsJson.OptionalValue<string>("SMTPUsr") ?? "",
					settingsJson.OptionalValue<string>("SMTPPwd") ?? "",
					settingsJson.OptionalValueStruct<bool>("SMTPSSL") ?? true,
					settingsJson.OptionalValue<Dictionary<string, string[]>>("TwitterUsers") ?? new Dictionary<string, string[]>());
			}
			catch (Exception e)
			{
				MessageBox.Show($"Unable to parse settings.json, exiting program\n\n{e.Message}\n\n{e.StackTrace}");
				Environment.Exit(0);
			}

			//Fix invalid settings
			if (Settings.RefreshRate < 1) Settings.RefreshRate = 1;
			if (Settings.Timeout < 5) Settings.Timeout = 5;
			if (Settings.SMTPPort == default) Settings.SMTPPort = 21;
			if (!string.IsNullOrEmpty(Settings.AircraftListUrl) && Settings.AircraftListUrl.Length > 3 &&
			    Settings.AircraftListUrl.Substring(0, 4) != "http")
				Settings.AircraftListUrl = "http://" + Settings.AircraftListUrl;

			//Log to UI
			_logger.Log("Settings Loaded", Color.White);

			//If email content config file does not exist, create one
			if (!File.Exists("emailconfig.json"))
			{
				_logger.Log("No email content config file! Creating one...", Color.White);
				LoadEccDefaults();
			}

			//Decode ecc json
			JObject? eccJson = null;
			try
			{
				using var fileStream = new FileStream("emailconfig.json", FileMode.Open);
				using var reader = new StreamReader(fileStream);
				using var jsonReader = new JsonTextReader(reader);
				eccJson = JsonSerializer.Create().Deserialize<JObject>(jsonReader);
			}
			catch (Exception e)
			{
				MessageBox.Show(
					$"Unable to deserialise emailconfig.json, exiting program\n\n{e.Message}\n\n{e.StackTrace}",
					"emailconfig.json parse error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
			}

			if (eccJson == null)
			{
				MessageBox.Show("Error loading emailconfig.json, exiting program", "emailconfig.json error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
			}

			try
			{
				//Copy ecc from parsed json
				EmailContentConfig = new EmailContentConfig(
					eccJson.RequiredValueStruct<bool>("AfLookup"),
					eccJson.RequiredValueStruct<bool>("AircraftPhotos"),
					eccJson.RequiredValueStruct<bool>("Map"),
					eccJson.OptionalValueStruct<bool>("KMLfile") ?? false,
					eccJson.RequiredValueStruct<PropertyListType>("PropertyList"),
					eccJson.RequiredValueStruct<bool>("RadarLink"),
					eccJson.RequiredValueStruct<bool>("ReportLink"),
					eccJson.RequiredValueStruct<bool>("ReceiverName"),
					eccJson.RequiredValueStruct<bool>("TransponderType"),
					eccJson.RequiredValueStruct<bool>("TwitterOptimised")
				);
			}
			catch (Exception e)
			{
				MessageBox.Show($"Unable to parse emailconfig.json, exiting program\n\n{e.Message}\n\n{e.StackTrace}");
				Environment.Exit(0);
			}

			//Log to UI
			_logger.Log("Email Content Config Loaded", Color.White);

			LoadedSuccessfully = true;
		}
	}
}
