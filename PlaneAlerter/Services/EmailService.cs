using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services {
	internal interface IEmailService
	{
		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="emailAddress">Email address to send to</param>
		/// <param name="condition">Condition that triggered alert</param>
		/// <param name="aircraft">Aircraft information for matched aircraft</param>
		/// <param name="receiverName">Name of receiver that got the last aircraft information</param>
		/// <param name="isDetection">Is this a detection, not a removal?</param>
		void SendEmail(string emailAddress, Condition condition, Aircraft aircraft, string receiverName, bool isDetection);
	}

	/// <summary>
	/// Class for email operations
	/// </summary>
	internal class EmailService : IEmailService
	{
        private readonly ISettingsManagerService _settingsManagerService;
        private readonly IUrlBuilderService _urlBuilderService;

		public EmailService(ISettingsManagerService settingsManagerService, IUrlBuilderService urlBuilderService)
		{
			_settingsManagerService = settingsManagerService;
			_urlBuilderService = urlBuilderService;
		}

		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="emailAddress">Email address to send to</param>
		/// <param name="condition">Condition that triggered alert</param>
		/// <param name="aircraft">Aircraft information for matched aircraft</param>
		/// <param name="receiverName">Name of receiver that got the last aircraft information</param>
		/// <param name="isDetection">Is this a detection, not a removal?</param>
		public void SendEmail(string emailAddress, Condition condition, Aircraft aircraft, string receiverName, bool isDetection) {
            var mailClient = new SmtpClient(_settingsManagerService.Settings.SMTPHost);
            mailClient.Port = _settingsManagerService.Settings.SMTPPort;
            mailClient.Credentials = new NetworkCredential(_settingsManagerService.Settings.SMTPUser,
	            _settingsManagerService.Settings.SMTPPassword);
            mailClient.EnableSsl = _settingsManagerService.Settings.SMTPSSL;
            
			//Transponder type from aircraft info
			var transponderName = "Unknown";
			//Aircraft image urls
			var imageLinks = new string[2];
			//Table for displaying aircraft property values
			var aircraftTable = "<table style='border: 2px solid #444;border-spacing: 0px;border-collapse: collapse;' id='acTable'>";
			//HTML for aircraft images
			var imageHtml = "";
			//Airframes.org url
			var airframesUrl = "";

            var message = new MailMessage();

			//Set message type to html
			message.IsBodyHtml = true;

			//Add email to message receiver list
			try {
				message.To.Clear();
				message.To.Add(emailAddress);
			}
			catch {
				Core.Ui.WriteToConsole("ERROR: Email to send to is invalid (" + emailAddress + ")", Color.Red);
				return;
			}

			//Set sender email to the one set in settings
			try {
				message.From = new MailAddress(_settingsManagerService.Settings.SenderEmail, "PlaneAlerter Alerts");
			}
			catch {
				Core.Ui.WriteToConsole("ERROR: Email to send from is invalid (" + _settingsManagerService.Settings.SenderEmail + ")", Color.Red);
				return;
			}

            //Set subject
            message.Subject = Core.ParseCustomFormatString(isDetection ? condition.EmailFirstFormat : condition.EmailLastFormat, aircraft, condition);

            if (_settingsManagerService.EmailContentConfig.TwitterOptimised) {
                var typeString = "First Contact";
                var regoString = "No rego, ";
                var callsignString = "No callsign, ";
				var operatorString = "No operator, ";

                if (!isDetection) {
                    typeString = "Last Contact";
                }
                if (aircraft.GetProperty("Reg") != null) {
                    regoString = aircraft.GetProperty("Reg") + ", ";
                }
                if (aircraft.GetProperty("Call") != null) {
                    callsignString = aircraft.GetProperty("Call") + ", ";
                }
				if(aircraft.GetProperty("OpIcao") != null) {
					operatorString = aircraft.GetProperty("OpIcao") + ", ";
				}
                message.Body = "[" + typeString + "] " + condition.Name + ", " + regoString + operatorString + aircraft.GetProperty("Type") + ", " + callsignString +
                               _settingsManagerService.Settings.RadarUrl;
                //[Last Contact] USAF (RCH9701), 79-1951, RCH, DC10, RCH9701 http://aussieadsb.com
            }
            else {
                //Create request for aircraft image urls
                var request = (HttpWebRequest)WebRequest.Create(_settingsManagerService.Settings.AircraftListUrl.Substring(0,
	                _settingsManagerService.Settings.AircraftListUrl.LastIndexOf("/") + 1) + "AirportDataThumbnails.json?icao=" + aircraft.Icao + "&numThumbs=" + imageLinks.Length);
                request.Method = "GET";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Timeout = 5000;

                //If vrs authentication is used, add credentials to request
                if (_settingsManagerService.Settings.VRSAuthenticate) {
                    var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(
	                    _settingsManagerService.Settings.VRSUser + ":" + _settingsManagerService.Settings.VRSPassword));
                    request.Headers.Add("Authorization", "Basic " + encoded);
                }

                //Send request and parse response
                try {
                    //Get response
                    var imageResponse = (HttpWebResponse)request.GetResponse();
                    var imageResponseBytes = new byte[32768];
                    imageResponse.GetResponseStream().Read(imageResponseBytes, 0, 32768);
                    var imageResponseText = Encoding.ASCII.GetString(imageResponseBytes);

                    //Parse json
                    var imageResponseJson = (JObject)JsonConvert.DeserializeObject(imageResponseText);

                    //If status is not 404, add images to image HTML
                    if (imageResponseJson["status"].ToString() != "404")
                        foreach (JObject image in imageResponseJson["data"])
                            imageHtml += "<img style='margin: 0px 5px 5px 0px;border: 2px solid #444;' src='" + image["image"].Value<string>() + "' />";
                    imageHtml += "<br><br>";
                }
                catch (Exception) {

                }
				
				//Generate google maps url
				var googleMapsUrl = "https://www.google.com.au/maps/search/" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long");

				//Generate airframes.org url
				if (aircraft.GetProperty("Reg") != null && aircraft.GetProperty("Reg") != "")
                    airframesUrl = "<h3><a style='text-decoration: none;' href='http://www.airframes.org/reg/" + aircraft.GetProperty("Reg").Replace("-", "").ToUpper() + "'>Airframes.org Lookup</a></h3>";

                //Get name of transponder type
                if (EnumUtils.TryGetConvertedValue("Trt", aircraft.GetProperty("Trt"), out string convertedTrtValue)) {
                    transponderName = convertedTrtValue;
                }

                //Write to UI
                Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | SENDING    | " + aircraft.Icao + " | " + message.Subject, Color.LightBlue);

                //Generate aircraft property value table
                var aircraftPropertyKeys = aircraft.GetPropertyKeys();
                foreach (var propertyKey in aircraftPropertyKeys) {
                    //TODO ADD TO VRSPROPERTIES
                    var parameter = "UNKNOWN_PARAMETER";
                    //Set parameter to a readable name if it's not included in vrs property info
                    switch (propertyKey) {
                        case "FSeen":
                            parameter = "First_Seen";
                            break;
                        case "HasPic":
                            parameter = "Has_Picture";
                            break;
                        case "Id":
                            parameter = "Id";
                            break;
                        case "PosTime":
                            parameter = "Position_Time";
                            break;
                        case "TT":
                        case "ResetTrail":
                            continue;
                    }
                    //If parameter is set (not in vrs property info list) and property list type is set to essentials, skip this property
                    if (parameter != "UNKNOWN_PARAMETER" && _settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials)
                        continue;
                    //Get parameter information from vrs property info
                    if (parameter == "UNKNOWN_PARAMETER") {
                        foreach (var property in Core.VrsPropertyData.Keys)
                        {
	                        if (Core.VrsPropertyData[property][2] != propertyKey.ToString())
		                        continue;
	                        
	                        //If property list type is essentials and this property is not in the list of essentials, leave this property as unknown so it can be skipped
	                        if (_settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials && !Core.EssentialProperties.Contains(property))
		                        continue;
	                        parameter = property.ToString();
                        }
                        if (parameter == "UNKNOWN_PARAMETER")
                            parameter = propertyKey.ToString();
                    }

                    //Get value
                    var value = aircraft.GetProperty(propertyKey);
                    //Add string conversions of enum values
                    if (EnumUtils.TryGetConvertedValue(propertyKey, value, out string convertedValue)) {
                        value += " (" + convertedValue + ")";
					}
                    //If rcvr, add receiver name
                    if (propertyKey == "Rcvr") {
                        value += " (" + receiverName + ")";
					}

                    //Add html for property
                    if (propertyKey != aircraftPropertyKeys.Last())
                        aircraftTable += "<tr style='border-bottom: 1px dashed #999;'>";
                    else
                        aircraftTable += "<tr>";
                    aircraftTable += "<td style='padding: 3px 6px;font-weight:bold;'>" + parameter.Replace('_', ' ') + "</td>";
                    aircraftTable += "<td style='padding: 3px 6px;'>" + value + "</td>";
                    aircraftTable += "</tr>";
                }
                aircraftTable += "</table>";

                //Create the main html document
                message.Body =
                    "<!DOCTYPE html PUBLIC ' -W3CDTD XHTML 1.0 TransitionalEN' 'http:www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><body>";
                if (isDetection)
                    message.Body += "<h1>Plane Alert - First Contact</h1>";
                else
                    message.Body += "<h1>Plane Alert - Last Contact</h1>";

                //Condition name
                message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Condition: " + condition.Name + "</h2>";
                //Receiver name
                if (_settingsManagerService.EmailContentConfig.ReceiverName)
                    message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Reciever: " + receiverName + "</h2>";
                //Transponder type
                if (_settingsManagerService.EmailContentConfig.TransponderType)
                    message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;'>Transponder: " + transponderName + "</h2>";
                //Radar url
                if (_settingsManagerService.EmailContentConfig.RadarLink)
                    message.Body += $"<h3><a style='text-decoration: none;' href='{_settingsManagerService.Settings.RadarUrl}?icao={aircraft.Icao}'>Goto Radar</a></h3>";
                //Report url
                if (_settingsManagerService.EmailContentConfig.ReportLink)
                    message.Body += $"<h3>VRS Report: <a style='text-decoration: none;' href='{_urlBuilderService.GenerateReportUrl(aircraft.Icao, false)}'>Desktop</a>   <a style='text-decoration: none;' href='{_urlBuilderService.GenerateReportUrl(aircraft.Icao, true)}'>Mobile</a></h3>";
                //Airframes.org url
                if (_settingsManagerService.EmailContentConfig.AfLookup)
                    message.Body += airframesUrl;

                message.Body += "<table><tr><td>";
                //Property list
                if (_settingsManagerService.EmailContentConfig.PropertyList != PropertyListType.Hidden)
                    message.Body += aircraftTable;
                message.Body += "</td><td style='padding-left: 10px;vertical-align: top;'>";
                //Aircraft photos
                if (_settingsManagerService.EmailContentConfig.AircraftPhotos)
                    message.Body += imageHtml;
                //Map
                if (_settingsManagerService.EmailContentConfig.Map && aircraft.GetProperty("Lat") != null) {
                    message.Body += "<h3 style='margin: 0px'><a style='text-decoration: none' href=" + googleMapsUrl + ">Open in Google Maps</a></h3><br />" +
                    "<img style='border: 2px solid #444;' alt='Loading Map...' src='" + _urlBuilderService.GenerateMapUrl(aircraft).Replace("&", "&amp;") + "' />";
                }
                //KML
                if (_settingsManagerService.EmailContentConfig.KMLfile && aircraft.GetProperty("Lat") != null)
                    message.Attachments.Add(Attachment.CreateAttachmentFromString(Core.GenerateKml(aircraft), aircraft.Icao + ".kml"));
                

                message.Body += "</td></tr></table></body></html>";
            }

			//Send the alert
			try {
				mailClient.Send(message);
			}
			catch(SmtpException e) {
				if(e.InnerException != null) {
					Core.Ui.WriteToConsole("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}
				Core.Ui.WriteToConsole("SMTP ERROR: " + e.Message, Color.Red);
				return;
			}
			catch (InvalidOperationException e) {
				if (e.InnerException != null) {
					Core.Ui.WriteToConsole("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}
				Core.Ui.WriteToConsole("SMTP ERROR: " + e.Message, Color.Red);
				return;
			}

			//Log to UI
			Core.Ui.WriteToConsole(DateTime.Now.ToLongTimeString() + " | SENT       | " + aircraft.Icao + " | " + message.Subject, Color.LightBlue);
		}
	}
}
