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
using PlaneAlerter.Forms;

namespace PlaneAlerter {
	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	internal static class Twitter {
		//Don't even think about it
		private const string Key = "U1dYZUhZT0RqOG1hV25xRzczMXZ6Y3k3NA==";
		private const string SecretKey = "UWlBcGtJaXJFdnpZaFhVb0FGVE93R003dnR0YnhJdWI1Y1BzVmxxSktuZGlmSkZvMzA=";

		private static readonly string ConsumerKey = Encoding.UTF8.GetString(Convert.FromBase64String(Key));
		private static readonly string ConsumerSecretKey = Encoding.UTF8.GetString(Convert.FromBase64String(SecretKey));

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public static async Task<bool> Tweet(string token, string tokenSecret, string content, string mediaUrl) {
			if (content.Contains("@")) {
				Core.Ui.WriteToConsole("ERROR: Mentions are not allowed in tweets", System.Drawing.Color.Red);
				return false;
			}

			//Set credentials
			var twitterClient = new TwitterClient(ConsumerKey, ConsumerSecretKey, token, tokenSecret);

			//Media ID of attached image
			long? mediaId = null;

			//Upload map image if required
			if (mediaUrl != "") {
				var ms = new MemoryStream();

				try {
					//Download the map image from google
					var req = (HttpWebRequest)WebRequest.Create(mediaUrl);
					using var res = req.GetResponse();
					await res.GetResponseStream().CopyToAsync(ms);
				}
				catch (Exception e) {
					Core.Ui.WriteToConsole($"ERROR: {e.GetType()} error downloading map image from Google: {e.Message}", System.Drawing.Color.Red);
					return false;
				}

				try {
					//Upload it to twitter
					var imageBytes = ms.ToArray();
					var media = await twitterClient.Upload.UploadTweetImageAsync(imageBytes);
					await ms.DisposeAsync();

					//Check if it was uploaded properly
					if (media.HasBeenUploaded) mediaId = media.Id;
					else Core.Ui.WriteToConsole("ERROR: Error uploading map", System.Drawing.Color.Red);
				}
				catch (TwitterException e) {
					Core.Ui.WriteToConsole($"ERROR: Error uploading map image to Twitter: {e}", System.Drawing.Color.Red);
				}
				catch (Exception e) { 
					Core.Ui.WriteToConsole($"ERROR: {e.GetType()} error uploading map image to Twitter: {e.Message}", System.Drawing.Color.Red);
				}
			}

			//Publish tweet
			try {
				string requestBody;
				if (mediaId != null) {
					requestBody = $@"{{""text"": ""{content}"", ""media"": {{""media_ids"": [""{mediaId}""]}}}}";
				}
				else {
					requestBody = $@"{{""text"": ""{content}""}}";
				}
				await twitterClient.Execute.RequestAsync(request =>
				{
					request.Url = "https://api.twitter.com/2/tweets";
					request.HttpMethod = HttpMethod.POST;
					request.HttpContent = new System.Net.Http.StringContent(requestBody, Encoding.ASCII, "application/json");
				});
			}
			catch (TwitterException e) {
				string? detailedContent = null;
				if (e.Content != null) {
					var contentJson = JsonSerializer.Create().Deserialize<Newtonsoft.Json.Linq.JObject>(new JsonTextReader(new StringReader(e.Content)));
					detailedContent = contentJson["detail"]?.ToString();
				}

				Core.Ui.WriteToConsole($"ERROR: Error publishing tweet: {(detailedContent != null ? detailedContent + Environment.NewLine : "")}{e.ToString()}", System.Drawing.Color.Red);
				return false;
			}
			catch (Exception e) {
				Core.Ui.WriteToConsole($"ERROR: {e.GetType()} error publishing tweet: " + e.Message, System.Drawing.Color.Red);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		public static async void AddAccount() {
			var appClient = new TwitterClient(ConsumerKey, ConsumerSecretKey);

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
			var dialog = new PinPromptDialog();
			if (dialog.ShowDialog() != DialogResult.OK)
				return;
			
			//Get credentials from Twitter
			ITwitterCredentials userCredentials;
			try {
				userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(dialog.Pin, authenticationRequest);
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
			var userClient = new TwitterClient(userCredentials);
			var user = await userClient.Users.GetAuthenticatedUserAsync();
			//Get screen name
			var screenName = user.ScreenName;

			//Check if users list already contains user that was just authenticated
			if (Settings.TwitterUsers.ContainsKey(screenName)) {
				MessageBox.Show("User '" + screenName + "' already added", "User already added");
				return;
			}

			//Add user
			Settings.TwitterUsers.Add(screenName, new[] { userCredentials.AccessToken, userCredentials.AccessTokenSecret });
			MessageBox.Show("Twitter user '" + screenName + "' authorized!", "User Authorized");
			Core.Ui.UpdateTwitterAccounts();
		}

		/// <summary>
		/// Remove a specific account
		/// </summary>
		public static void RemoveAccount(string screenName) {
			//Check if users list contains user
			if (!Settings.TwitterUsers.ContainsKey(screenName)) {
				MessageBox.Show("User '" + screenName + "' does not exist", "User doesn't exist");
				return;
			}

			//Remove user
			Settings.TwitterUsers.Remove(screenName);
			MessageBox.Show("Twitter user '" + screenName + "' removed!", "User Removed");
			Core.Ui.UpdateTwitterAccounts();
		}
	}
}
