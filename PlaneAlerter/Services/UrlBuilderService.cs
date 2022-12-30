using System.Drawing;
using System.Linq;
using PlaneAlerter.Forms;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IUrlBuilderService
	{
		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		string GenerateReportUrl(string icao, bool mobile);

		/// <summary>
		/// Generate a map
		/// </summary>
		string GenerateMapUrl(Aircraft aircraft);
	}

	internal class UrlBuilderService : IUrlBuilderService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;

		public UrlBuilderService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
			_logger = logger;
		}

		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		public string GenerateReportUrl(string icao, bool mobile)
		{
			var reportUrl = "";
			if (string.IsNullOrWhiteSpace(_settingsManagerService.Settings.RadarUrl))
			{
				_logger.Log("ERROR: Please enter radar URL in settings", Color.Red);
				return "";
			}
			if (!_settingsManagerService.Settings.RadarUrl.ToLower().Contains("virtualradar"))
			{
				_logger.Log("WARNING: Radar URL must end with /VirtualRadar/ for report links to work", Color.Orange);
				return "";
			}


			reportUrl += _settingsManagerService.Settings.RadarUrl;
			if (_settingsManagerService.Settings.RadarUrl[_settingsManagerService.Settings.RadarUrl.Length - 1] != '/') reportUrl += "/";

			if (mobile) reportUrl += "mobileReport.html?sort1=date&sortAsc1=0&icao-Q=" + icao;
			else reportUrl += "desktopReport.html?sort1=date&sortAsc1=0&icao-Q=" + icao;

			return reportUrl;
		}

		/// <summary>
		/// Generate a map
		/// </summary>
		public string GenerateMapUrl(Aircraft aircraft)
		{
			var staticMapUrl = "";
			//If aircraft has a position, generate a google map url
			if (aircraft.GetProperty("Lat") != null)
				if (_settingsManagerService.Settings.CentreMapOnAircraft)
				{
					if (aircraft.Trail.Length != 4)
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
					else
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&size=800x800&zoom=8&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				}
				else
				{
					if (aircraft.Trail.Count() != 4)
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" +
						               _settingsManagerService.Settings.Lat + "," + _settingsManagerService.Settings.Long + "&size=800x800&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
					else
						staticMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" +
						               _settingsManagerService.Settings.Lat + "," + _settingsManagerService.Settings.Long + "&size=800x800&zoom=8&markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&key=AIzaSyCJxiyiDWBHiYSMm7sjSTJkQubuo3XuR7s&path=color:0x000000|";
				}
			//Process aircraft trail
			for (var i = (aircraft.Trail.Count() / 4) - 1; i >= 0; i--)
			{
				//Get coordinate
				var coordinate = new[] {
						aircraft.Trail[i * 4].ToString("#.####"),
						aircraft.Trail[i * 4 + 1].ToString("#.####"),
						aircraft.Trail[i * 4 + 2].ToString("#.####"),
						aircraft.Trail[i * 4 + 3].ToString("#.####")
					};
				var coordinateString = coordinate[0] + "," + coordinate[1] + "|";

				//Check if adding another coordinate will make the url too long
				if (staticMapUrl.Length + coordinateString.Length > 8000) break; //Limit is 8192, using 8000 to give some headroom. Allows for about 440 points

				//Add coordinate to google map url
				staticMapUrl += coordinateString;
			}

			//Return empty string if no positions
			if (staticMapUrl == "" || staticMapUrl.Length == 0) return "";

			return staticMapUrl.Substring(0, staticMapUrl.Length - 1);
		}
	}
}
