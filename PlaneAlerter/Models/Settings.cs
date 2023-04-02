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
		public Dictionary<string, string[]> TwitterUsers { get; set; } = new();
	}
}
