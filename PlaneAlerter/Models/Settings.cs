using System.Collections.Generic;

namespace PlaneAlerter.Models
{
	internal class Settings
	{
		/// <summary>
		/// Email to send alerts from
		/// </summary>
		public string SenderEmail { get; set; }

		/// <summary>
		/// AircraftList.json url
		/// </summary>
		public string AircraftListUrl { get; set; }

		/// <summary>
		/// Do we need authentication for VRS?
		/// </summary>
		public bool VRSAuthenticate => !string.IsNullOrEmpty(VRSUser);

		/// <summary>
		/// VRS username
		/// </summary>
		public string VRSUser { get; set; }

		/// <summary>
		/// VRS password
		/// </summary>
		public string VRSPassword { get; set; }

		/// <summary>
		/// Latitude of user
		/// </summary>
		public decimal Lat { get; set; }

		/// <summary>
		/// Longitude of user
		/// </summary>
		public decimal Long { get; set; }

		/// <summary>
		/// Radar URL
		/// </summary>
		public string RadarUrl { get; set; }

		/// <summary>
		/// Centre maps on aircraft
		/// </summary>
		public bool CentreMapOnAircraft { get; set; }

		/// <summary>
		/// Removal timeout
		/// </summary>
		public int RemovalTimeout { get; set; }

		/// <summary>
		/// Checker refresh rate
		/// </summary>
		public int RefreshRate { get; set; }

		/// <summary>
		/// Start checker on program start
		/// </summary>
		public bool StartOnStart { get; set; }

		/// <summary>
		/// Checker request timeout
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		/// Show notifications
		/// </summary>
		public bool ShowNotifications { get; set; }

		/// <summary>
		/// Sound alerts
		/// </summary>
		public bool SoundAlerts { get; set; }

		/// <summary>
		/// Flash the window on an alert
		/// </summary>
		public bool FlashWindow { get; set; }

		/// <summary>
		/// Ignore aircraft beyond a specific distance?
		/// </summary>
		public bool FilterDistance { get; set; }

		/// <summary>
		/// Ignore aircraft above a specific altitude?
		/// </summary>
		public bool FilterAltitude { get; set; }

		/// <summary>
		/// Ignore Mode-S (when filtering by distance)
		/// </summary>
		public bool IgnoreModeS { get; set; }

		/// <summary>
		/// What distance to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public double IgnoreDistance { get; set; }

		/// <summary>
		/// What altitude to ignore aircraft beyond (for bandwidth saving)?
		/// </summary>
		public int IgnoreAltitude { get; set; }

		/// <summary>
		/// Filter by receiver?
		/// </summary>
		public bool FilterReceiver { get; set; }

		/// <summary>
		/// Selected receiver to filter aircraft with
		/// </summary>
		public int FilterReceiverId { get; set; }

		/// <summary>
		/// Number of checks between trail updates
		/// 0 = Never request trails, 1 = Request every check, >=2 = Request every x checks
		/// </summary>
		public int TrailsUpdateFrequency { get; set; }

		/// <summary>
		/// SMTP Host
		/// </summary>
		public string SMTPHost { get; set; }

		/// <summary>
		/// SMTP Port
		/// </summary>
		public int SMTPPort { get; set; }

		/// <summary>
		/// SMTP Username
		/// </summary>
		public string SMTPUser { get; set; }

		/// <summary>
		/// SMTP Password
		/// </summary>
		public string SMTPPassword { get; set; }

		/// <summary>
		/// Do we use SSL?
		/// </summary>
		public bool SMTPSSL { get; set; }

		/// <summary>
		/// List of Twitter user credentials
		/// </summary>
		public Dictionary<string, string[]> TwitterUsers { get; set; }

		public Settings(
			string senderEmail,
			string aircraftListUrl,
			string vrsUser,
			string vrsPassword,
			decimal lat,
			decimal @long,
			string radarUrl,
			bool centreMapOnAircraft,
			int removalTimeout,
			int refreshRate,
			bool startOnStart,
			int timeout,
			bool showNotifications,
			bool soundAlerts,
			bool flashWindow,
			bool filterDistance,
			bool filterAltitude,
			bool ignoreModeS,
			double ignoreDistance,
			int ignoreAltitude,
			bool filterReceiver,
			int filterReceiverId,
			int trailsUpdateFrequency,
			string smtpHost,
			int smtpPort,
			string smtpUser,
			string smtpPassword,
			bool smtpSSL,
			Dictionary<string, string[]> twitterUsers)
		{
			SenderEmail = senderEmail;
			AircraftListUrl = aircraftListUrl;
			VRSUser = vrsUser;
			VRSPassword = vrsPassword;
			Lat = lat;
			Long = @long;
			RadarUrl = radarUrl;
			CentreMapOnAircraft = centreMapOnAircraft;
			RemovalTimeout = removalTimeout;
			RefreshRate = refreshRate;
			StartOnStart = startOnStart;
			Timeout = timeout;
			ShowNotifications = showNotifications;
			SoundAlerts = soundAlerts;
			FlashWindow = flashWindow;
			FilterDistance = filterDistance;
			FilterAltitude = filterAltitude;
			IgnoreModeS = ignoreModeS;
			IgnoreDistance = ignoreDistance;
			IgnoreAltitude = ignoreAltitude;
			FilterReceiver = filterReceiver;
			FilterReceiverId = filterReceiverId;
			TrailsUpdateFrequency = trailsUpdateFrequency;
			SMTPHost = smtpHost;
			SMTPPort = smtpPort;
			SMTPUser = smtpUser;
			SMTPPassword = smtpPassword;
			SMTPSSL = smtpSSL;
			TwitterUsers = twitterUsers;
		}
	}
}
