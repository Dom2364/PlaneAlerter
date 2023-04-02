using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PlaneAlerter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PlaneAlerter.Extensions;

namespace PlaneAlerter.Services
{
	internal interface IVrsService
	{
		/// <summary>
		/// List of current aircraft
		/// </summary>
		public List<Aircraft> AircraftList { get; set; }

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		public Dictionary<string, string> Receivers { get; set; }

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		Task GetAircraft(bool modeSOnly, bool clearExisting, bool noActiveMatches, bool forceRequestTrails = false);

		Task<Dictionary<string, string>?> GetReceivers();

		Task<string[]> GetAircraftThumbnails(string icao);
	}

	internal class VrsService : IVrsService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;
		private readonly HttpClient _httpClient;

		/// <summary>
		/// List of current aircraft
		/// </summary>
		public List<Aircraft> AircraftList { get; set; } = new List<Aircraft>();

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		public Dictionary<string, string> Receivers { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// How many checks ago were the trails requested
		/// </summary>
		private int _trailsAge = 1;

		public VrsService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger, HttpClient httpClient)
		{
			_settingsManagerService	= settingsManagerService;
			_logger = logger;
			_httpClient = httpClient;
		}

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		public async Task GetAircraft(bool modeSOnly, bool clearExisting, bool noActiveMatches, bool forceRequestTrails = false)
		{
			var requestTrails = _settingsManagerService.Settings.TrailsUpdateFrequency == 1;

			//Force request trails
			if (forceRequestTrails)
			{
				requestTrails = true;
			}
			//No matches so we don't need trails
			else if (noActiveMatches)
			{
				requestTrails = false;
			}
			//Threshold enabled
			else if (_settingsManagerService.Settings.TrailsUpdateFrequency >= 2)
			{
				if (_trailsAge >= _settingsManagerService.Settings.TrailsUpdateFrequency)
				{
					requestTrails = true;
					_trailsAge = 0;
				}
				_trailsAge++;
			}

			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains("?") ? "&" : "?";
			url += "lat=" + _settingsManagerService.Settings.Lat + "&lng=" + _settingsManagerService.Settings.Long;
			if (_settingsManagerService.Settings.FilterDistance && !modeSOnly) url += "&fDstU=" + _settingsManagerService.Settings.IgnoreDistance.ToString("#.##");
			if (_settingsManagerService.Settings.FilterAltitude) url += "&fAltU=" + _settingsManagerService.Settings.IgnoreAltitude;
			if (_settingsManagerService.Settings.FilterReceiver) url += "&feed=" + _settingsManagerService.Settings.FilterReceiverId;
			if (modeSOnly) url += "&fNoPosQN=1";
			if (requestTrails) url += "&trFmt=fa&refreshTrails=1";

			try
			{
				JObject? responseJson;
				try
				{
					responseJson = await RequestAircraftListAsync(url);
				}
				catch (Exception e)
				{
					_logger.Log("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return;
				}

				if (responseJson == null)
				{
					_logger.Log("ERROR: Unable to deserialise AircraftList.json", Color.Red);
					return;
				}

				//Check if we actually got data
				if (responseJson.OptionalValue<JToken>("acList") == null ||
				    responseJson.OptionalValue<JToken>("stm") == null)
				{
					_logger.Log("ERROR: Invalid response received from server", Color.Red);
					return;
				}

				//Save old trails if not requesting new ones
				Dictionary<string, double?[]>? oldTrails = null;
				if (!requestTrails)
					oldTrails = AircraftList.ToDictionary(x => x.Icao, x => x.Trail);

				if (clearExisting) AircraftList.Clear();

				//Parse aircraft data
				foreach (var a in responseJson.RequiredValue<List<JObject?>>("acList"))
				{
					//Ignore if null
					if (a == null)
						continue;

					//Create new aircraft
					var aircraft = new Aircraft(a.RequiredValue<string>("Icao"));

					//Parse aircraft trail
					if (requestTrails)
					{
						aircraft.Trail = a.OptionalValue<double?[]>("Cot") ?? Array.Empty<double?>();
					}
					else
					{
						if (oldTrails != null && oldTrails.ContainsKey(aircraft.Icao)) aircraft.Trail = oldTrails[aircraft.Icao];
					}

					//Parse aircraft properties
					var properties = a.Properties().ToList();
					foreach (var property in properties)
						aircraft.AddProperty(property.Name, property.Value.ToString());

					//Remove the existing aircraft if it exists
					AircraftList.RemoveAll(x => x.Icao == aircraft.Icao);

					//Add aircraft to list
					AircraftList.Add(aircraft);
				}

				//Get list of receivers
				Receivers.Clear();
				foreach (var f in responseJson.RequiredValue<JToken>("feeds"))
					Receivers.Add(f.RequiredValue<string>("id"), f.RequiredValue<string>("name"));

				//Try to clean up json parsing
				responseJson.RemoveAll();
				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException)
			{
				_logger.Log("ERROR: AircraftList.json url invalid (" + _settingsManagerService.Settings.AircraftListUrl + ")", Color.Red);
				return;
			}
			catch (InvalidDataException)
			{
				_logger.Log("ERROR: Data returned from " + _settingsManagerService.Settings.AircraftListUrl + " was not gzip compressed", Color.Red);
				return;
			}
			catch (WebException e)
			{
				_logger.Log("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return;
			}
			catch (JsonReaderException e)
			{
				_logger.Log("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return;
			}
		}

		private async Task<JObject?> RequestAircraftListAsync(string url)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, url);

			if (_settingsManagerService.Settings.VRSAuthenticate)
			{
				var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(
					$"{_settingsManagerService.Settings.VRSUser}:{_settingsManagerService.Settings.VRSPassword}"));

				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);
			}

			using var tokenSource = new CancellationTokenSource();
			tokenSource.CancelAfter(_settingsManagerService.Settings.Timeout * 1000);

			var response = await _httpClient.SendAsync(request, tokenSource.Token);

			var responseContent = await response.Content.ReadAsStringAsync(tokenSource.Token);

			return (JObject?)JsonConvert.DeserializeObject(responseContent);
		}

		public async Task<Dictionary<string, string>?> GetReceivers()
		{
			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains("?") ? "&" : "?";
			url += "fUtQ=abc";

			try
			{
				JObject? responseJson;
				try
				{
					responseJson = await RequestAircraftListAsync(url);
				}
				catch (Exception e)
				{
					_logger.Log("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return null;
				}

				if (responseJson == null)
				{
					_logger.Log("ERROR: Unable to deserialise AircraftList.json", Color.Red);
					return null;
				}

				//Check if we actually got data
				if (responseJson.OptionalValue<JToken>("feeds") == null ||
				    responseJson.OptionalValue<JToken>("stm") == null)
				{
					_logger.Log("ERROR: Invalid response received from server", Color.Red);
					return null;
				}

				//Get list of receivers
				Receivers.Clear();
				foreach (var f in responseJson.RequiredValue<JToken>("feeds"))
					Receivers.Add(f.RequiredValue<string>("id"), f.RequiredValue<string>("name"));

				//Try to clean up json parsing
				responseJson.RemoveAll();

				GC.Collect(2, GCCollectionMode.Forced);
			}
			catch (UriFormatException)
			{
				_logger.Log("ERROR: AircraftList.json url invalid (" + _settingsManagerService.Settings.AircraftListUrl + ")", Color.Red);
				return null;
			}
			catch (InvalidDataException)
			{
				_logger.Log("ERROR: Data returned from " + _settingsManagerService.Settings.AircraftListUrl + " was not gzip compressed", Color.Red);
				return null;
			}
			catch (WebException e)
			{
				_logger.Log("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
				return null;
			}
			catch (JsonReaderException e)
			{
				_logger.Log("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
				return null;
			}

			return Receivers;
		}

		public async Task<string[]> GetAircraftThumbnails(string icao)
		{
			//Aircraft image urls
			var imageLinks = new List<string>();

			var url =
				$"{_settingsManagerService.Settings.AircraftListUrl.Substring(0, _settingsManagerService.Settings.AircraftListUrl.LastIndexOf("/", StringComparison.Ordinal) + 1)}AirportDataThumbnails.json?icao={icao}&numThumbs=2";

			var request = new HttpRequestMessage(HttpMethod.Get, url);

			if (_settingsManagerService.Settings.VRSAuthenticate)
			{
				var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(
					$"{_settingsManagerService.Settings.VRSUser}:{_settingsManagerService.Settings.VRSPassword}"));

				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);
			}

			using var tokenSource = new CancellationTokenSource();
			tokenSource.CancelAfter(TimeSpan.FromSeconds(5));

			//Send request and parse response
			try
			{
				var response = await _httpClient.SendAsync(request, tokenSource.Token);

				var responseContent = await response.Content.ReadAsStringAsync(tokenSource.Token);

				//Parse json
				var responseJson = (JObject?)JsonConvert.DeserializeObject(responseContent);

				if (responseJson == null)
					return Array.Empty<string>();

				//If status is not 404, add images to result
				if (responseJson.Value<string>("status") != "404" && responseJson.TryGetValue("data", out var data))
					imageLinks.AddRange(data.Select(image => image.Value<string>("image"))
							.Where(x => x != null)
							.Select(x => x!));

				return imageLinks.ToArray();
			}
			catch (Exception e)
			{
				_logger.Log($"ERROR: {e.GetType()} error getting aircraft images from VRS: {e.Message}", Color.Red);
				return Array.Empty<string>();
			}
		}
	}
}
