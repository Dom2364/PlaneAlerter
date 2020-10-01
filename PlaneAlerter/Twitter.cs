using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using System.Windows.Forms;
using System.IO;


namespace PlaneAlerter {
	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	static class Twitter {
		//Don't even think about it
		private const string Key = "U1dYZUhZT0RqOG1hV25xRzczMXZ6Y3k3NA==";
		private const string SecretKey = "UWlBcGtJaXJFdnpZaFhVb0FGVE93R003dnR0YnhJdWI1Y1BzVmxxSktuZGlmSkZvMzA=";

		private static IConsumerCredentials AppCredentials = new ConsumerCredentials(
			Encoding.UTF8.GetString(Convert.FromBase64String(Key)),
			Encoding.UTF8.GetString(Convert.FromBase64String(SecretKey)));

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public static bool Tweet(string token, string tokensecret, string content, string mediaURL) {
			if (content.Contains("@")) {
				Core.UI.writeToConsole("ERROR: Mentions are not allowed in tweets", System.Drawing.Color.Red);
				return false;
			}

			//Set credentials
			Auth.SetUserCredentials(AppCredentials.ConsumerKey, AppCredentials.ConsumerSecret, token, tokensecret);
			
			PublishTweetOptionalParameters options = new PublishTweetOptionalParameters();
			//Upload map image if required
			if (mediaURL != "") {
				try {
					//Download the map image from google
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(mediaURL);
					WebResponse res = req.GetResponse();
					MemoryStream ms = new MemoryStream();
					res.GetResponseStream().CopyTo(ms);
					byte[] imageBytes = ms.ToArray();

					//Upload it to twitter
					IMedia media = Upload.UploadBinary(imageBytes);
					res.Dispose();
					ms.Dispose();

					//Check if it was uploaded properly
					if (media != null) {
						if (media.HasBeenUploaded) options.Medias.Add(media);
						else Core.UI.writeToConsole("ERROR: Error uploading map", System.Drawing.Color.Red);
					}
					else {
						var e = ExceptionHandler.GetLastException();
						Core.UI.writeToConsole("ERROR: Error uploading map: " + e.TwitterDescription, System.Drawing.Color.Red);
					}
				}
				catch (WebException e) { 
					Core.UI.writeToConsole("ERROR: Error downloading map: " + e.Message, System.Drawing.Color.Red);
				}
			}
			//Publish tweet
			try {
				ITweet tweet = Tweetinvi.Tweet.PublishTweet(content, options);
				if (tweet == null) {
					var e = ExceptionHandler.GetLastException();
					//Check if exception has extra details
					if (e.TwitterExceptionInfos != null) {
						string details = "";
						foreach (Tweetinvi.Core.Exceptions.ITwitterExceptionInfo einfo in e.TwitterExceptionInfos) details += einfo.Message + ", ";
						if (details.Length != 0) details = details.Substring(0, details.Length-2);
						Core.UI.writeToConsole("ERROR: Error publishing tweet: " + e.TwitterDescription + " (" + details + ")", System.Drawing.Color.Red);
					}
					else {
						Core.UI.writeToConsole("ERROR: Error publishing tweet: " + e.TwitterDescription, System.Drawing.Color.Red);
					}
					return false;
				}
				if (!tweet.IsTweetPublished) {
					Core.UI.writeToConsole("ERROR: Unknown error publishing tweet", System.Drawing.Color.Red);
				}
				return tweet.IsTweetPublished;
			}
			catch (Exception e) {
				Core.UI.writeToConsole("ERROR: Error publishing tweet: " + e.Message, System.Drawing.Color.Red);
				return false;
			}
		}

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		public static void AddAccount() {
			//Authenticate PlaneAlerter
			IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(AppCredentials);
			if (authenticationContext == null) {
				var e = ExceptionHandler.GetLastException();
				//Check if exception has extra details
				if (e.TwitterExceptionInfos != null) {
					string details = "";
					foreach (Tweetinvi.Core.Exceptions.ITwitterExceptionInfo einfo in e.TwitterExceptionInfos) details += einfo.Message + ", ";
					if (details.Length != 0) details = details.Substring(0, details.Length - 2);
					MessageBox.Show("ERROR: Error authenticating PlaneAlerter with Twitter, please contact developer with details of this message" + Environment.NewLine + Environment.NewLine + e.TwitterDescription + Environment.NewLine + Environment.NewLine + details, "Authentication Error");
				}
				else {
					MessageBox.Show("ERROR: Error authenticating PlaneAlerter with Twitter, please contact developer with details of this message" + Environment.NewLine + Environment.NewLine + e.TwitterDescription, "Authentication Error");
				}
				return;
			}

			//Open URL for user to authenticate PlaneAlerter app
			Process.Start(authenticationContext.AuthorizationURL);

			//Show dialog for user to enter pin
			PinPromptDialog dialog = new PinPromptDialog();
			if (dialog.ShowDialog() == DialogResult.OK) {
				//Get credentials from Twitter
				ITwitterCredentials userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(dialog.PIN, authenticationContext);
				if (userCredentials == null) {
					MessageBox.Show("Incorrect PIN Entered", "Authentication Error");
					return;
				}

				//Set active credentials
				Auth.SetCredentials(userCredentials);
				//Get screen name
				string screenname = User.GetAuthenticatedUser().ScreenName;

				//Check if users list already contains user that was just authenticated
				if (Settings.TwitterUsers.ContainsKey(screenname)) {
					MessageBox.Show("User '" + screenname + "' already added", "User already added");
					return;
				}

				//Add user
				Settings.TwitterUsers.Add(screenname, new string[] { userCredentials.AccessToken, userCredentials.AccessTokenSecret });
				MessageBox.Show("Twitter user '" + screenname + "' authorized!", "User Authorized");
				Core.UI.updateTwitterAccounts();
			}
		}

		/// <summary>
		/// Remove a specific account
		/// </summary>
		public static void RemoveAccount(string screenname) {
			//Check if users list contains user
			if (!Settings.TwitterUsers.ContainsKey(screenname)) {
				MessageBox.Show("User '" + screenname + "' does not exist", "User doesn't exist");
				return;
			}

			//Remove user
			Settings.TwitterUsers.Remove(screenname);
			MessageBox.Show("Twitter user '" + screenname + "' removed!", "User Removed");
			Core.UI.updateTwitterAccounts();
		}
	}
}
