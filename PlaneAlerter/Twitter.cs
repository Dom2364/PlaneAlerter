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
		//Don't even think about it
		private const string Key = "ZGdJYmh6R1hCdldqRUNJUlF2d2p3aVFaQg==";
		private const string SecretKey = "alRlNVh2bk8zS0IzWlNuSnVmalU5NUdqenN6ZUs3SUtiVlBsM1NBZHp1Yjc5eVh1NlY=";
		private static IConsumerCredentials AppCredentials = new ConsumerCredentials(Key, SecretKey);
		public static Dictionary<string, ITwitterCredentials> UserCredentials = new Dictionary<string, ITwitterCredentials>();

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
		}
	}
}
