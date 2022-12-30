using System.Collections.Generic;
using PlaneAlerter.Enums;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Condition information
	/// </summary>
	internal class Condition
	{
		/// <summary>
		/// Name of condition
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Ignore following conditions
		/// </summary>
		public bool IgnoreFollowing { get; set; }

		/// <summary>
		/// Alerts sent for this condition
		/// </summary>
		public int AlertsThisSession { get; set; } = 0;

		/// <summary>
		/// Send emails?
		/// </summary>
		public bool EmailEnabled { get; set; }

		/// <summary>
		/// Emails to send alert to
		/// </summary>
		public List<string> ReceiverEmails { get; set; } = new List<string>();

		/// <summary>
		/// Email first contact alert format
		/// </summary>
		public string EmailFirstFormat { get; set; }

		/// <summary>
		/// Email last contact alert format
		/// </summary>
		public string EmailLastFormat { get; set; }

		/// <summary>
		/// Send tweets?
		/// </summary>
		public bool TwitterEnabled { get; set; }

		/// <summary>
		/// Twitter account to send from
		/// </summary>
		public string TwitterAccount { get; set; }

		/// <summary>
		/// Tweet first contact alert format
		/// </summary>
		public string TweetFirstFormat { get; set; }

		/// <summary>
		/// Tweet last contact alert format
		/// </summary>
		public string TweetLastFormat { get; set; }

		/// <summary>
		/// Link to be attached in tweets
		/// </summary>
		public TweetLink TweetLink { get; set; }

		/// <summary>
		/// Attach map?
		/// </summary>
		public bool TweetMap { get; set; }

		/// <summary>
		/// Type of alert
		/// </summary>
		public AlertType AlertType { get; set; }

		/// <summary>
		/// List of triggers
		/// </summary>
		public Dictionary<int, Trigger> Triggers = new Dictionary<int, Trigger>();

		/// <summary>
		/// Creates a new condition
		/// </summary>
		public Condition()
		{

		}
	}
}
