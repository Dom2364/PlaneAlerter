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
using PlaneAlerter.Infrastructure;

namespace PlaneAlerter.Services
{
	internal interface IVrsService
	{
		/// <summary>
		/// List of current aircraft
		/// </summary>
		public IReadOnlyCollection<Aircraft> GetAircraftList();

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		public IReadOnlyDictionary<string, string> GetReceivers();

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		Task UpdateAircraftList(bool modeSOnly, bool clearExisting, bool noActiveMatches, bool forceRequestTrails = false);

		Task<IReadOnlyDictionary<string, string>?> UpdateReceivers();

		Task<string[]> GetAircraftThumbnails(string icao);
	}

	internal class VrsService : IVrsService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;
		private readonly HttpClient _httpClient;
		private readonly SemaphoreLocker _aircraftListLock = new();

		/// <summary>
		/// List of current aircraft
		/// </summary>
		private readonly List<Aircraft> _aircraftList = new();

		/// <summary>
		/// List of receivers from the last aircraftlist.json response
		/// </summary>
		private readonly Dictionary<string, string> _receivers = new();

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
		
		public IReadOnlyCollection<Aircraft> GetAircraftList() => _aircraftList.ToList();
		public IReadOnlyDictionary<string, string> GetReceivers() => _receivers.ToDictionary(k => k.Key, v => v.Value);

		public async Task UpdateAircraftList(bool modeSOnly, bool clearExisting, bool noActiveMatches,
			bool forceRequestTrails = false)
		{
			await _aircraftListLock.LockAsync(async () =>
				await UpdateAircraftListLocked(modeSOnly, clearExisting, noActiveMatches, forceRequestTrails));
		}
		
		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		private async Task UpdateAircraftListLocked(bool modeSOnly, bool clearExisting, bool noActiveMatches, bool forceRequestTrails = false)
		{
			var requestTrails = forceRequestTrails || ShouldRequestTrails(noActiveMatches);

			try
			{
				var url = GenerateAircraftListUrl(modeSOnly, requestTrails);
				
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
				var oldTrails = !requestTrails
					? _aircraftList.ToDictionary(x => x.Icao, x => x.Trail)
					: null;
				
				//Increment trails age
				if (requestTrails)
					_trailsAge++;

				//Clear aircraft list if required
				if (clearExisting)
					_aircraftList.Clear();

				//Parse aircraft data
				foreach (var a in responseJson.RequiredValue<List<JObject?>>("acList"))
				{
					//Ignore if null
					if (a == null)
						continue;

					//Create new aircraft
					var aircraft = new Aircraft(a.RequiredValue<string>("Icao"));

					//Parse aircraft trail from acList if requested, else use old trail
					if (requestTrails)
					{
						aircraft.Trail = a.OptionalValue<double?[]>("Cot") ?? Array.Empty<double?>();
					}
					else if (oldTrails != null && oldTrails.TryGetValue(aircraft.Icao, out var trail))
					{
						aircraft.Trail = trail;
					}

					//Parse aircraft properties
					var properties = a.Properties().ToList();
					foreach (var property in properties)
						aircraft.AddProperty(property.Name, property.Value.ToString());

					//Remove the existing aircraft if it exists
					_aircraftList.RemoveAll(x => x.Icao == aircraft.Icao);

					//Add aircraft to list
					_aircraftList.Add(aircraft);
				}
				
				//Get list of receivers
				_receivers.Clear();
				foreach (var f in responseJson.RequiredValue<JToken>("feeds"))
					_receivers[f.RequiredValue<string>("id")] = f.RequiredValue<string>("name");

				//Try to clean up json parsing
				responseJson.RemoveAll();
			}
			catch (UriFormatException)
			{
				_logger.Log("ERROR: AircraftList.json url invalid (" + _settingsManagerService.Settings.AircraftListUrl + ")", Color.Red);
			}
			catch (InvalidDataException)
			{
				_logger.Log("ERROR: Data returned from " + _settingsManagerService.Settings.AircraftListUrl + " was not gzip compressed", Color.Red);
			}
			catch (WebException e)
			{
				_logger.Log("ERROR: Error while connecting to AircraftList.json (" + e.Message + ")", Color.Red);
			}
			catch (JsonReaderException e)
			{
				_logger.Log("ERROR: Error parsing JSON response (" + e.Message + ")", Color.Red);
			}
		}

		private bool ShouldRequestTrails(bool noActiveMatches)
		{
			//No matches so we don't need trails
			if (noActiveMatches)
				return false;
			
			//Request every x checks, decide if we need to request trails based on age
			if (_settingsManagerService.Settings.TrailsUpdateFrequency >= 2)
			{
				if (_trailsAge >= _settingsManagerService.Settings.TrailsUpdateFrequency)
				{
					_trailsAge = 0;
					return true;
				}

				return false;
			}
			
			//Request every check or never
			return _settingsManagerService.Settings.TrailsUpdateFrequency == 1;
		}

		private string GenerateAircraftListUrl(bool modeSOnly, bool requestTrails)
		{
			var url = _settingsManagerService.Settings.AircraftListUrl;
			
			url += url.Contains('?') ? "&" : "?";
			url += "lat=" + _settingsManagerService.Settings.Lat + "&lng=" + _settingsManagerService.Settings.Long;
			
			if (_settingsManagerService.Settings.FilterDistance && !modeSOnly)
				url += "&fDstU=" + _settingsManagerService.Settings.IgnoreDistance.ToString("#.##");
			
			if (_settingsManagerService.Settings.FilterAltitude)
				url += "&fAltU=" + _settingsManagerService.Settings.IgnoreAltitude;
			
			if (_settingsManagerService.Settings.FilterReceiver)
				url += "&feed=" + _settingsManagerService.Settings.FilterReceiverId;
			
			if (modeSOnly)
				url += "&fNoPosQN=1";
			
			if (requestTrails)
				url += "&trFmt=fa&refreshTrails=1";
			
			return url;
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

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync(tokenSource.Token);

			return (JObject?)JsonConvert.DeserializeObject(responseContent);
		}

		public async Task<IReadOnlyDictionary<string, string>?> UpdateReceivers()
		{
			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains('?') ? "&" : "?";
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
				_receivers.Clear();
				foreach (var f in responseJson.RequiredValue<JToken>("feeds"))
					_receivers[f.RequiredValue<string>("id")] = f.RequiredValue<string>("name");

				//Try to clean up json parsing
				responseJson.RemoveAll();
			}
			catch (TaskCanceledException)
			{
				_logger.Log($"ERROR: Request for AircraftList.json exceeded timeout of {_settingsManagerService.Settings.Timeout} seconds", Color.Red);
				return null;
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

			return GetReceivers();
		}

		public async Task<string[]> GetAircraftThumbnails(string icao)
		{
			var url =
				$"{_settingsManagerService.Settings.AircraftListUrl.Substring(0, _settingsManagerService.Settings.AircraftListUrl.LastIndexOf("/", StringComparison.Ordinal) + 1)}AirportDataThumbnails.json?icao={icao}&numThumbs=2";

			var request = new HttpRequestMessage(HttpMethod.Get, url);

			if (_settingsManagerService.Settings.VRSAuthenticate)
			{
				var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(
					$"{_settingsManagerService.Settings.VRSUser}:{_settingsManagerService.Settings.VRSPassword}"));

				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);
			}

			//Send request and parse response
			try
			{
				using var tokenSource = new CancellationTokenSource(_settingsManagerService.Settings.Timeout * 1000);

				var response = await _httpClient.SendAsync(request, tokenSource.Token);

				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync(tokenSource.Token);

				//Parse json
				var responseJson = (JObject?)JsonConvert.DeserializeObject(responseContent);

				if (responseJson == null)
					return Array.Empty<string>();

				var status = responseJson.Value<int>("status");

				if (status == 500)
				{
					_logger.Log(
						$"WARNING: Error getting aircraft images from VRS: {responseJson.Value<string>("error")}",
						Color.Orange);
					return Array.Empty<string>();
				}

				if (status == 404 || !responseJson.TryGetValue("data", out var data))
					return Array.Empty<string>();
				
				return data.Select(image => image.Value<string>("image"))
					.Where(x => x != null)
					.Select(x => x!).ToArray();
			}
			catch (TaskCanceledException)
			{
				_logger.Log($"ERROR: Request for aircraft images from VRS exceeded timeout of {_settingsManagerService.Settings.Timeout} seconds", Color.Red);
				return Array.Empty<string>();
			}
			catch (Exception e)
			{
				_logger.Log($"ERROR: {e.GetType()} error getting aircraft images from VRS: {e.Message}", Color.Red);
				return Array.Empty<string>();
			}
		}
	}
}
