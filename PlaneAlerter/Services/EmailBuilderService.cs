using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IEmailBuilderService
	{
		Task<MailMessage> Build(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection);
	}

	internal class EmailBuilderService : IEmailBuilderService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IUrlBuilderService _urlBuilderService;
		private readonly IStringFormatterService _stringFormatterService;
		private readonly IKmlService _kmlService;
		private readonly IVrsEnumService _vrsEnumService;
		private readonly IVrsService _vrsService;

		public EmailBuilderService(ISettingsManagerService settingsManagerService, IUrlBuilderService urlBuilderService,
			IStringFormatterService stringFormatterService, IKmlService kmlService,
			IVrsEnumService vrsEnumService, IVrsService vrsService)
		{
			_settingsManagerService = settingsManagerService;
			_urlBuilderService = urlBuilderService;
			_stringFormatterService = stringFormatterService;
			_kmlService = kmlService;
			_vrsEnumService = vrsEnumService;
			_vrsService = vrsService;
		}

		public async Task<MailMessage> Build(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection)
		{
			var message = new MailMessage();

			//Set message type to html
			message.IsBodyHtml = true;

			//Set subject
			message.Subject = _stringFormatterService.Format(isDetection ? condition.EmailFirstFormat : condition.EmailLastFormat, aircraft, condition);
			
			//Build body
			if (_settingsManagerService.EmailContentConfig.TwitterOptimised)
				message.Body = BuildTwitterOptimisedBody(condition, aircraft, isDetection);
			else
				message.Body = await BuildBody(condition, aircraft, receiverName, isDetection);

			//Attach KML
			if (_settingsManagerService.EmailContentConfig.KmlFile && aircraft.GetProperty("Lat") != null)
			{
				var kml = _kmlService.GenerateTrailKml(aircraft);

				message.Attachments.Add(Attachment.CreateAttachmentFromString(kml, aircraft.Icao + ".kml"));
			}

			return message;
		}

		private async Task<string> BuildBody(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection)
		{
			//Create the main html document
			var body = "<!DOCTYPE html PUBLIC ' -W3CDTD XHTML 1.0 TransitionalEN' 'http:www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><body>";
			body += isDetection ? "<h1>Plane Alert - First Contact</h1>" : "<h1>Plane Alert - Last Contact</h1>";

			//Condition name
			body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Condition: " + condition.Name + "</h2>";

			//Receiver name
			if (_settingsManagerService.EmailContentConfig.ReceiverName)
				body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Receiver: " + receiverName + "</h2>";

			//Transponder type
			if (_settingsManagerService.EmailContentConfig.TransponderType)
			{
				var transponderName = _vrsEnumService.TryToString("Trt",
					aircraft.GetProperty("Trt"), out var convertedTrtValue)
					? convertedTrtValue
					: "Unknown";

				body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Transponder: " + transponderName + "</h2>";
			}

			//Radar url
			if (_settingsManagerService.EmailContentConfig.RadarLink)
				body += $"<h3><a style='text-decoration: none;' href='{_settingsManagerService.Settings.RadarUrl}?icao={aircraft.Icao}'>Goto Radar</a></h3>";

			//Report url
			if (_settingsManagerService.EmailContentConfig.ReportLink)
				body += $"<h3>VRS Report: <a style='text-decoration: none;' href='{_urlBuilderService.GenerateReportUrl(aircraft.Icao, false)}'>Desktop</a>   <a style='text-decoration: none;' href='{_urlBuilderService.GenerateReportUrl(aircraft.Icao, true)}'>Mobile</a></h3>";

			//Airframes.org url
			if (_settingsManagerService.EmailContentConfig.AfLookup && !string.IsNullOrEmpty(aircraft.GetProperty("Reg")))
				body +=
					$"<h3><a style='text-decoration: none;' href='{_urlBuilderService.GenerateAirframesOrgUrl(aircraft.GetProperty("Reg")!)}'>Airframes.org Lookup</a></h3>";

			body += "<table><tr><td>";

			//Property list
			if (_settingsManagerService.EmailContentConfig.PropertyList != PropertyListType.Hidden)
				body += BuildPropertiesTable(aircraft, receiverName);
			
			body += "</td><td style='padding-left: 10px;vertical-align: top;'>";

			//Aircraft photos
			if (_settingsManagerService.EmailContentConfig.AircraftPhotos)
			{
				var aircraftImages = await _vrsService.GetAircraftThumbnails(aircraft.Icao);

				var imageHtml = "";
				foreach (var image in aircraftImages)
					imageHtml += $"<img style='margin: 0px 5px 5px 0px;border: 2px solid #444;' src='{image}' />";

				body += imageHtml + "<br><br>";
			}

			//Map
			if (_settingsManagerService.EmailContentConfig.Map && aircraft.GetProperty("Lat") != null)
			{
				var googleMapsUrl =
					$"https://www.google.com.au/maps/search/{aircraft.GetProperty("Lat")},{aircraft.GetProperty("Long")}";

				body += "<h3 style='margin: 0px'><a style='text-decoration: none' href=" + googleMapsUrl + ">Open in Google Maps</a></h3><br />" +
					"<img style='border: 2px solid #444;' alt='Loading Map...' src='" + _urlBuilderService.GenerateGoogleStaticMapUrl(aircraft).Replace("&", "&amp;") + "' />";
			}

			body += "</td></tr></table></body></html>";

			return body;
		}

		private string BuildPropertiesTable(Aircraft aircraft, string receiverName)
		{
			var table = "<table style='border: 2px solid #444;border-spacing: 0px;border-collapse: collapse;' id='acTable'>";

			var aircraftPropertyKeys = aircraft.GetPropertyKeys();
			foreach (var propertyKey in aircraftPropertyKeys)
			{
				var property = VrsProperties.VrsPropertyData.Where(x => x.Value[2] == propertyKey)
					.Select(x => (VrsProperty?)x.Key).FirstOrDefault();

				string propertyName;

				if (property != null)
				{
					//If property list type is essentials and this property is not in the list of essentials then skip
					if (_settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials &&
					    !VrsProperties.EssentialProperties.Contains(property.Value))
						continue;

					propertyName = property.Value.ToString();
				}
				else
				{
					//If property list type is essentials then skip
					if (_settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials)
						continue;

					//Set parameter to a readable name if it's not included in vrs property info
					switch (propertyKey)
					{
						case "FSeen":
							propertyName = "First_Seen";
							break;
						case "HasPic":
							propertyName = "Has_Picture";
							break;
						case "Id":
							propertyName = "Id";
							break;
						case "PosTime":
							propertyName = "Position_Time";
							break;
						case "TT":
						case "ResetTrail":
							continue;
						default:
							propertyName = propertyKey;
							break;
					}
				}

				//Get value
				var value = aircraft.GetProperty(propertyKey);

				//Add string conversions of enum values
				if (_vrsEnumService.TryToString(propertyKey, value, out var convertedValue))
				{
					value += " (" + convertedValue + ")";
				}

				//If rcvr, add receiver name
				if (propertyKey == "Rcvr")
				{
					value += " (" + receiverName + ")";
				}

				//Add html for property
				if (propertyKey != aircraftPropertyKeys.Last())
					table += "<tr style='border-bottom: 1px dashed #999;'>";
				else
					table += "<tr>";
				table += "<td style='padding: 3px 6px;font-weight:bold;'>" + propertyName.Replace('_', ' ') + "</td>";
				table += "<td style='padding: 3px 6px;'>" + value + "</td>";
				table += "</tr>";
			}
			table += "</table>";

			return table;
		}

		private string BuildTwitterOptimisedBody(Condition condition, Aircraft aircraft, bool isDetection)
		{
			var typeString = !isDetection ? "Last Contact" : "First Contact";
			var regoString = aircraft.GetProperty("Reg") != null ? aircraft.GetProperty("Reg") + ", " : "No rego, ";
			var callsignString = aircraft.GetProperty("Call") != null ? aircraft.GetProperty("Call") + ", " : "No callsign, ";
			var operatorString = aircraft.GetProperty("OpIcao") != null ? aircraft.GetProperty("OpIcao") + ", " : "No operator, ";

			return
				$"[{typeString}] {condition.Name}, {regoString}{operatorString}{aircraft.GetProperty("Type")}, {callsignString}{_settingsManagerService.Settings.RadarUrl}";
			//[Last Contact] USAF (RCH9701), 79-1951, RCH, DC10, RCH9701 http://aussieadsb.com
		}
	}
}
