using System;
using System.Diagnostics;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;


namespace PlaneAlerter {
	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	static class Twitter {
		public static IConsumerCredentials AppCredentials;
		public static Dictionary<string, ITwitterCredentials> UserCredentials;

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public static void Tweet(ITwitterCredentials credentials, string content) {
			Auth.SetCredentials(credentials);
			Tweetinvi.Tweet.PublishTweet(content);
		}

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		public static void AddAccount() {
			IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(AppCredentials);
			Process.Start(authenticationContext.AuthorizationURL);
			
			//Show dialog for user to enter pin
			string pinCode = Console.ReadLine();
			
			ITwitterCredentials userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext);

			UserCredentials.Add(User.GetAuthenticatedUser().ScreenName, userCredentials);
			SecureConfig.SaveConfig();
		}
	}
}
