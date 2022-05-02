using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Exceptions;
using Newtonsoft.Json;


namespace PlaneAlerter {
	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	static class Twitter {
		//Don't even think about it
		private const string Key = "U1dYZUhZT0RqOG1hV25xRzczMXZ6Y3k3NA==";
		private const string SecretKey = "UWlBcGtJaXJFdnpZaFhVb0FGVE93R003dnR0YnhJdWI1Y1BzVmxxSktuZGlmSkZvMzA=";

		private static string ConsumerKey = Encoding.UTF8.GetString(Convert.FromBase64String(Key));
		private static string ConsumerSecretKey = Encoding.UTF8.GetString(Convert.FromBase64String(SecretKey));

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public static async Task<bool> Tweet(string token, string tokensecret, string content, string mediaURL) {
			if (content.Contains("@")) {
				Core.UI.writeToConsole("ERROR: Mentions are not allowed in tweets", System.Drawing.Color.Red);
				return false;
			}

			//Set credentials
			TwitterClient twitterClient = new TwitterClient(ConsumerKey, ConsumerSecretKey, token, tokensecret);

			//Media ID of attached image
			long? mediaid = null;

			//Upload map image if required
			if (mediaURL != "") {
				MemoryStream ms = new MemoryStream();

				try {
					//Download the map image from google
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(mediaURL);
					using (WebResponse res = req.GetResponse()) {
						res.GetResponseStream().CopyTo(ms);
					}	
				}
				catch (Exception e) {
					Core.UI.writeToConsole($"ERROR: {e.GetType()} error downloading map image from Google: {e.Message}", System.Drawing.Color.Red);
					return false;
				}

				try {
					//Upload it to twitter
					byte[] imageBytes = ms.ToArray();
					IMedia media = await twitterClient.Upload.UploadTweetImageAsync(imageBytes);
					ms.Dispose();

					//Check if it was uploaded properly
					if (media.HasBeenUploaded) mediaid = media.Id;
					else Core.UI.writeToConsole("ERROR: Error uploading map", System.Drawing.Color.Red);
				}
				catch (TwitterException e) {
					Core.UI.writeToConsole($"ERROR: Error uploading map image to Twitter: {e.ToString()}", System.Drawing.Color.Red);
				}
				catch (Exception e) { 
					Core.UI.writeToConsole($"ERROR: {e.GetType()} error uploading map image to Twitter: {e.Message}", System.Drawing.Color.Red);
				}
			}

			//Publish tweet
			try {
				string requestbody = null;
				if (mediaid != null) {
					requestbody = $@"{{""text"": ""{content}"", ""media"": {{""media_ids"": [""{mediaid}""]}}}}";
				}
				else {
					requestbody = $@"{{""text"": ""{content}""}}";
				}
				var tweet = await twitterClient.Execute.RequestAsync(request =>
				{
					request.Url = "https://api.twitter.com/2/tweets";
					request.HttpMethod = HttpMethod.POST;
					request.HttpContent = new System.Net.Http.StringContent(requestbody, Encoding.ASCII, "application/json");
				});
			}
			catch (TwitterException e) {
				string detailedcontent = null;
				if (e.Content != null) {
					Newtonsoft.Json.Linq.JObject contentjson = JsonSerializer.Create().Deserialize<Newtonsoft.Json.Linq.JObject>(new JsonTextReader(new StringReader(e.Content)));
					detailedcontent = contentjson["detail"]!=null? contentjson["detail"].ToString() : null;
				}

				Core.UI.writeToConsole($"ERROR: Error publishing tweet: {(detailedcontent!=null?detailedcontent + Environment.NewLine:"")}{e.ToString()}", System.Drawing.Color.Red);
				return false;
			}
			catch (Exception e) {
				Core.UI.writeToConsole($"ERROR: {e.GetType()} error publishing tweet: " + e.Message, System.Drawing.Color.Red);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		public static async void AddAccount() {
			TwitterClient appClient = new TwitterClient(ConsumerKey, ConsumerSecretKey);

			//Authenticate PlaneAlerter
			IAuthenticationRequest authenticationRequest;
			try {
				authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();
			}
			catch (TwitterAuthException e) {
				MessageBox.Show("Auth error authenticating PlaneAlerter with Twitter, please contact developer with details of this message: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
				return;
			}
			catch (TwitterException e) {
				MessageBox.Show("Error authenticating PlaneAlerter with Twitter, please contact developer with details of this message: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
				return;
			}
			catch (Exception e) {
				MessageBox.Show($"{e.GetType()} error authenticating PlaneAlerter with Twitter: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
				return;
			}

			//Open URL for user to authenticate PlaneAlerter app
			Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL) { UseShellExecute = true });

			//Show dialog for user to enter pin
			PinPromptDialog dialog = new PinPromptDialog();
			if (dialog.ShowDialog() == DialogResult.OK) {
				//Get credentials from Twitter
				ITwitterCredentials userCredentials;
				try {
					userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(dialog.PIN, authenticationRequest);
				}
				catch (TwitterAuthException e) {
					MessageBox.Show("Auth error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
					return;
				}
				catch (TwitterException e) {
					MessageBox.Show("Error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
					return;
				}
				catch (Exception e) {
					MessageBox.Show($"{e.GetType()} error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Authentication Error");
					return;
				}

				//Set active credentials
				TwitterClient userClient = new TwitterClient(userCredentials);
				IAuthenticatedUser user = await userClient.Users.GetAuthenticatedUserAsync();
				//Get screen name
				string screenname = user.ScreenName;

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
