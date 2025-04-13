using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaneAlerter.Forms;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace PlaneAlerter.Services {
	internal interface ITwitterService
	{
		event EventHandler AccountsUpdated;

		/// <summary>
		/// Posts a tweet
		/// </summary>
		Task<bool> Tweet(string token, string tokenSecret, string content, string mediaUrl);

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		Task AddAccount();

		/// <summary>
		/// Remove a specific account
		/// </summary>
		void RemoveAccount(string screenName);
	}

	/// <summary>
	/// Class for twitter related stuff
	/// </summary>
	internal class TwitterService : ITwitterService
	{
		public event EventHandler? AccountsUpdated;

		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;
		private readonly HttpClient _httpClient;

		//Don't even think about it
		private const string Key = "U1dYZUhZT0RqOG1hV25xRzczMXZ6Y3k3NA==";
		private const string SecretKey = "UWlBcGtJaXJFdnpZaFhVb0FGVE93R003dnR0YnhJdWI1Y1BzVmxxSktuZGlmSkZvMzA=";

		private static readonly string ConsumerKey = Encoding.UTF8.GetString(Convert.FromBase64String(Key));
		private static readonly string ConsumerSecretKey = Encoding.UTF8.GetString(Convert.FromBase64String(SecretKey));

		public TwitterService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger, HttpClient httpClient)
		{
			_settingsManagerService = settingsManagerService;
			_logger = logger;
			_httpClient = httpClient;
		}

		/// <summary>
		/// Posts a tweet
		/// </summary>
		public async Task<bool> Tweet(string token, string tokenSecret, string content, string mediaUrl) {
			if (content.Contains("@")) {
				_logger.Log("ERROR: Mentions are not allowed in tweets", System.Drawing.Color.Red);
				return false;
			}

			//Set credentials
			var twitterClient = new TwitterClient(ConsumerKey, ConsumerSecretKey, token, tokenSecret);

			//Media ID of attached image
			long? mediaId = null;

			//Upload map image if required
			if (!string.IsNullOrEmpty(mediaUrl))
				mediaId = await UploadMedia(twitterClient, mediaUrl);

			var requestBody = mediaId != null
				? $@"{{""text"": ""{content}"", ""media"": {{""media_ids"": [""{mediaId}""]}}}}"
				: $@"{{""text"": ""{content}""}}";

			//Publish tweet
			try {
				await twitterClient.Execute.RequestAsync(request =>
				{
					request.Url = "https://api.twitter.com/2/tweets";
					request.HttpMethod = HttpMethod.POST;
					request.HttpContent = new StringContent(requestBody, Encoding.ASCII, "application/json");
				});
			}
			catch (TwitterException e) {
				string? detailedContent = null;
				if (e.Content != null) {
					var contentJson = JsonSerializer.Create().Deserialize<Newtonsoft.Json.Linq.JObject>(new JsonTextReader(new StringReader(e.Content)));
					
					detailedContent = contentJson?["detail"]?.ToString();
				}

				_logger.Log($"ERROR: Error publishing tweet: {(detailedContent != null ? detailedContent + Environment.NewLine : "")}{e}", System.Drawing.Color.Red);
				return false;
			}
			catch (Exception e) {
				_logger.Log($"ERROR: {e.GetType()} error publishing tweet: " + e.Message, System.Drawing.Color.Red);
				return false;
			}

			return true;
		}

		private async Task<long?> UploadMedia(ITwitterClient client, string mediaUrl)
		{
			byte[] imageBytes;

			try
			{
				//Download the map image
				imageBytes = await _httpClient.GetByteArrayAsync(mediaUrl);
			}
			catch (Exception e)
			{
				_logger.Log($"ERROR: {e.GetType()} error downloading map image: {e.Message}",
					System.Drawing.Color.Red);
				return null;
			}

			try
			{
				//Upload it to twitter
				var media = await client.Upload.UploadTweetImageAsync(imageBytes);

				//Check if it was uploaded properly
				if (!media.HasBeenUploaded)
					_logger.Log("ERROR: Error uploading map", System.Drawing.Color.Red);
				
				return media.Id;
			}
			catch (TwitterException e)
			{
				_logger.Log($"ERROR: Error uploading map image to Twitter: {e}", System.Drawing.Color.Red);
			}
			catch (Exception e)
			{
				_logger.Log($"ERROR: {e.GetType()} error uploading map image to Twitter: {e.Message}",
					System.Drawing.Color.Red);
			}

			return null;
		}

		/// <summary>
		/// Authenticate account and add to accounts list
		/// </summary>
		public async Task AddAccount() {
			var appClient = new TwitterClient(ConsumerKey, ConsumerSecretKey);

			//Authenticate PlaneAlerter
			IAuthenticationRequest authenticationRequest;
			try {
				authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();
			}
			catch (TwitterAuthException e) {
				MessageBox.Show("Auth error authenticating PlaneAlerter with Twitter, please contact developer with details of this message: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}
			catch (TwitterException e) {
				MessageBox.Show("Error authenticating PlaneAlerter with Twitter, please contact developer with details of this message: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}
			catch (Exception e) {
				MessageBox.Show($"{e.GetType()} error authenticating PlaneAlerter with Twitter: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}

			//Open URL for user to authenticate PlaneAlerter app
			Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL) { UseShellExecute = true });

			//Show dialog for user to enter pin
			var dialog = Program.ServiceProvider.GetRequiredService<PinPromptDialog>();
			if (dialog.ShowDialog() != DialogResult.OK)
				return;
			
			//Get credentials from Twitter
			ITwitterCredentials userCredentials;
			try {
				userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(dialog.Pin, authenticationRequest);
			}
			catch (TwitterAuthException e) {
				MessageBox.Show("Auth error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}
			catch (TwitterException e) {
				MessageBox.Show("Error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}
			catch (Exception e) {
				MessageBox.Show($"{e.GetType()} error while retrieving user credentials: " + Environment.NewLine + Environment.NewLine + e, "Authentication Error");
				return;
			}

			//Set active credentials
			var userClient = new TwitterClient(userCredentials);
			var user = await userClient.Users.GetAuthenticatedUserAsync();
			//Get screen name
			var screenName = user.ScreenName;

			//Check if users list already contains user that was just authenticated
			if (_settingsManagerService.Settings.TwitterUsers.ContainsKey(screenName)) {
				MessageBox.Show("User '" + screenName + "' already added", "User already added");
				return;
			}

			//Add user
			_settingsManagerService.Settings.TwitterUsers.Add(screenName, new[] { userCredentials.AccessToken, userCredentials.AccessTokenSecret });
			MessageBox.Show("Twitter user '" + screenName + "' authorized!", "User Authorized");
			
			AccountsUpdated?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Remove a specific account
		/// </summary>
		public void RemoveAccount(string screenName) {
			//Check if users list contains user
			if (!_settingsManagerService.Settings.TwitterUsers.ContainsKey(screenName)) {
				MessageBox.Show("User '" + screenName + "' does not exist", "User doesn't exist");
				return;
			}

			//Remove user
			_settingsManagerService.Settings.TwitterUsers.Remove(screenName);
			MessageBox.Show("Twitter user '" + screenName + "' removed!", "User Removed");
			
			AccountsUpdated?.Invoke(this, EventArgs.Empty);
		}
	}
}
