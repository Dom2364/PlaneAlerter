using System;
using System.Diagnostics;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;


namespace PlaneAlerter {
	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	static class Twitter {
		public static IConsumerCredentials AppCredentials { get; set; }

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public static void Tweet(string accessToken, string accessTokenSecret, string content) {
			Auth.SetUserCredentials(AppCredentials.ConsumerKey, AppCredentials.ConsumerSecret, accessToken, accessTokenSecret);
			Tweetinvi.Tweet.PublishTweet(content);
		}

		/// <summary>
		/// Add an account to the list of accounts to pick from
		/// </summary>
		public static void AddAccount() {
			IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(AppCredentials);
			Process.Start(authenticationContext.AuthorizationURL);
			
			//Show dialog for user to enter pin
			string pinCode = Console.ReadLine();
			
			ITwitterCredentials userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext);

			//Store credentials
		}
	}
}
