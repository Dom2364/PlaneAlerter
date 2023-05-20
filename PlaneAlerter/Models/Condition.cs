using System.Collections.Generic;
using Newtonsoft.Json;
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
		public string Name { get; }

		/// <summary>
		/// Ignore following conditions
		/// </summary>
		public bool IgnoreFollowing { get; }

		/// <summary>
		/// Alerts sent for this condition
		/// </summary>
		[JsonIgnore]
		public int AlertsThisSession { get; set; } = 0;

		/// <summary>
		/// Send emails?
		/// </summary>
		public bool EmailEnabled { get; }

		/// <summary>
		/// Emails to send alert to
		/// </summary>
		public List<string> RecieverEmails { get; }

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
		public bool TwitterEnabled { get; }

		/// <summary>
		/// Twitter account to send from
		/// </summary>
		public string TwitterAccount { get; }

		/// <summary>
		/// Tweet first contact alert format
		/// </summary>
		public string TweetFirstFormat { get; }

		/// <summary>
		/// Tweet last contact alert format
		/// </summary>
		public string TweetLastFormat { get; }

		/// <summary>
		/// Link to be attached in tweets
		/// </summary>
		public TweetLink TweetLink { get; }

		/// <summary>
		/// Attach map?
		/// </summary>
		public bool TweetMap { get; }

		/// <summary>
		/// Type of alert
		/// </summary>
		public AlertType AlertType { get; }

		/// <summary>
		/// List of triggers
		/// </summary>
		public Dictionary<int, Trigger> Triggers { get; init; } = new();

		public Condition(
			string name,
			AlertType alertType,
			bool ignoreFollowing,
			bool emailEnabled,
			string emailFirstFormat,
			string emailLastFormat,
			List<string> recieverEmails,
			bool twitterEnabled,
			string twitterAccount,
			string tweetFirstFormat,
			string tweetLastFormat,
			bool tweetMap,
			TweetLink tweetLink)
		{
			Name = name;
			AlertType = alertType;
			IgnoreFollowing = ignoreFollowing;
			EmailEnabled = emailEnabled;
			EmailFirstFormat = emailFirstFormat;
			EmailLastFormat = emailLastFormat;
			RecieverEmails = recieverEmails;
			TwitterEnabled = twitterEnabled;
			TwitterAccount = twitterAccount;
			TweetFirstFormat = tweetFirstFormat;
			TweetLastFormat = tweetLastFormat;
			TweetMap = tweetMap;
			TweetLink = tweetLink;
		}
	}
}
