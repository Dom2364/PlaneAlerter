using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PlaneAlerter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using PlaneAlerter.Forms;

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
		void GetAircraft(bool modeSOnly, bool clearExisting, bool forceRequestTrails = false);

		Dictionary<string, string>? GetReceivers();
	}

	internal class VrsService : IVrsService
	{
		private readonly ICheckerService _checkerService;
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;

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

		/// <summary>
		/// Client for sending aircraftlist.json requests
		/// </summary>
		private HttpWebRequest? _request;

		public VrsService(ICheckerService checkerService, ISettingsManagerService settingsManagerService, ILoggerWithQueue logger)
		{
			_checkerService = checkerService;
			_settingsManagerService	= settingsManagerService;
			_logger = logger;
		}

		/// <summary>
		/// Get latest aircraftlist.json
		/// </summary>
		public void GetAircraft(bool modeSOnly, bool clearExisting, bool forceRequestTrails = false)
		{
			var requestTrails = _settingsManagerService.Settings.TrailsUpdateFrequency == 1;

			//Force request trails
			if (forceRequestTrails)
			{
				requestTrails = true;
			}
			//No matches so we don't need trails
			else if (_checkerService.ActiveMatches.Count == 0)
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
				JObject responseJson;
				try
				{
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e)
				{
					_logger.Log("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null)
				{
					_logger.Log("ERROR: Invalid response received from server", Color.Red);
					return;
				}

				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Save old trails if not requesting new ones
				Dictionary<string, double[]>? oldTrails = null;
				if (!requestTrails) oldTrails = AircraftList.ToDictionary(x => x.Icao, x => x.Trail);

				//Parse aircraft data
				if (clearExisting) AircraftList.Clear();
				foreach (JObject a in responseJson["acList"].ToList())
				{
					//Ignore if no icao is provided
					if (a["Icao"] == null) continue;
					//Create new aircraft
					var aircraft = new Aircraft(a["Icao"].ToString());

					//Parse aircraft trail
					if (requestTrails)
					{
						if (a["Cot"] != null)
							aircraft.Trail = new double[a["Cot"].Count()];
						for (var i = 0; i < aircraft.Trail.Length - 1; i++)
							if (a["Cot"][i].Value<string>() != null)
								aircraft.Trail[i] = double.Parse(a["Cot"][i].Value<string>(), CultureInfo.InvariantCulture);
							else
								aircraft.Trail[i] = 0;
					}
					else
					{
						if (oldTrails != null && oldTrails.ContainsKey(aircraft.Icao)) aircraft.Trail = oldTrails[aircraft.Icao];
					}

					//Parse aircraft properties
					var properties = a.Properties().ToList();
					for (var i = 0; i < properties.Count; i++)
						aircraft.AddProperty(properties[i].Name, properties[i].Value.ToString());

					//Add aircraft to list
					AircraftList.Add(aircraft);
				}

				//Get list of receivers
				Receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Receivers.Add(f["id"].ToString(), f["name"].ToString());

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

		private JObject? RequestAircraftList(string url)
		{
			//Create request
			_request = (HttpWebRequest)WebRequest.Create(url);
			_request.Method = "GET";
			_request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			_request.Timeout = _settingsManagerService.Settings.Timeout * 1000;
			//Add credentials if they are provided
			if (_settingsManagerService.Settings.VRSAuthenticate)
			{
				var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(
					_settingsManagerService.Settings.VRSUser + ":" + _settingsManagerService.Settings.VRSPassword));
				_request.Headers.Add("Authorization", "Basic " + encoded);
			}
			//Send request and parse json response
			using var response = (HttpWebResponse)_request.GetResponse();
			using var responseStream = response.GetResponseStream();
			using var reader = new StreamReader(responseStream);
			using var jsonReader = new JsonTextReader(reader);

			return JsonSerializer.Create().Deserialize<JObject>(jsonReader);
		}

		public Dictionary<string, string>? GetReceivers()
		{
			//Generate aircraftlist.json url
			var url = _settingsManagerService.Settings.AircraftListUrl;
			url += _settingsManagerService.Settings.AircraftListUrl.Contains("?") ? "&" : "?";
			url += "fUtQ=abc";

			try
			{
				JObject responseJson;
				try
				{
					responseJson = RequestAircraftList(url);
				}
				catch (Exception e)
				{
					_logger.Log("ERROR: " + e.GetType() + " while downloading AircraftList.json: " + e.Message, Color.Red);
					return null;
				}

				//Check if we actually got aircraft data
				if (responseJson["acList"] == null)
				{
					_logger.Log("ERROR: Invalid response recieved from server", Color.Red);
					return null;
				}
				//Throw error if server time was not parsed
				if (responseJson["stm"] == null)
					throw new JsonReaderException();

				//Get list of receivers
				Receivers.Clear();
				foreach (JObject f in responseJson["feeds"])
					Receivers.Add(f["id"].ToString(), f["name"].ToString());

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
	}
}
