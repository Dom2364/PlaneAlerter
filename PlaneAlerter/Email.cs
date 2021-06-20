using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace PlaneAlerter {
	/// <summary>
	/// Class for email operations
	/// </summary>
	static class Email {
		/// <summary>
		/// SMTP client used for sending emails
		/// </summary>
		public static SmtpClient mailClient;

		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="emailaddress">Email address to send to</param>
		/// <param name="message">Message to send</param>
		/// <param name="condition">Condition that triggered alert</param>
		/// <param name="aircraft">Aircraft information for matched aircraft</param>
		/// <param name="recieverName">Name of receiver that got the last aircraft information</param>
		/// <param name="emailPropertyInfo">String for displaying email property value</param>
		/// <param name="isDetection">Is this a detection, not a removal?</param>
		public static void SendEmail(string emailaddress, MailMessage message, Core.Condition condition, Core.Aircraft aircraft, string recieverName, string emailPropertyInfo, bool isDetection) {
			//Array to store position trail
			Dictionary<int, string[]> pathArray = new Dictionary<int, string[]>();
			//Google maps url
			string googleMapsUrl = "";
			//Transponder type from aircraft info
			string transponderName = "";
			//Types of transponders
			string[] transponderTypes = new string[] { "Unknown", "Mode-S", "ADS-B", "ADS-Bv1", "ADS-Bv2" };
			//Aircraft image urls
			string[] imageLinks = new string[2];
			//Table for displaying aircraft property values
			string aircraftTable = "<table style='border: 2px solid;border-radius: 10px;border-spacing: 0px;' id='acTable'>";
			//HTML for aircraft images
			string imageHTML = "";
			//Airframes.org url
			string airframesUrl = "";

			//Set message type to html
			message.IsBodyHtml = true;

			//Add email to message receiver list
			try {
				message.To.Clear();
				message.To.Add(emailaddress);
			}
			catch {
				Core.UI.writeToConsole("ERROR: Email to send to is invalid (" + emailaddress + ")", Color.Red);
				return;
			}

			//Set sender email to the one set in settings
			try {
				message.From = new MailAddress(Settings.senderEmail, "PlaneAlerter Alerts");
			}
			catch {
				Core.UI.writeToConsole("ERROR: Email to send from is invalid (" + Settings.senderEmail + ")", Color.Red);
				return;
			}

            if (Settings.EmailContentConfig.TwitterOptimised) {
                string typestring = "First Contact";
                string regostring = "No rego, ";
                string callsignstring = "No callsign, ";
				string operatorstring = "No operator, ";
				string emailpropertystring = "No " + condition.emailProperty.ToString();
                if (!isDetection) {
                    typestring = "Last Contact";
                }
                if (aircraft.GetProperty("Reg") != null) {
                    regostring = aircraft.GetProperty("Reg") + ", ";
                }
                if (aircraft.GetProperty("Call") != null) {
                    callsignstring = aircraft.GetProperty("Call") + ", ";
                }
				if(aircraft.GetProperty("OpIcao") != null) {
					operatorstring = aircraft.GetProperty("OpIcao") + ", ";
				}
				string emailpropertyvalue = aircraft.GetProperty(Core.vrsPropertyData[condition.emailProperty][2]);
                if (emailpropertyvalue != null) {
                    emailpropertystring = emailpropertyvalue;
                }
                message.Body = "[" + typestring + "] " + condition.conditionName + " (" + emailpropertystring + "), " + regostring + operatorstring + aircraft.GetProperty("Type") + ", " + callsignstring + Settings.radarUrl;
                //[Last Contact] USAF (RCH9701), 79-1951, RCH, DC10, RCH9701 http://seqldradar.net
            }
            else {
                //Create request for aircraft image urls
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.acListUrl.Substring(0, Settings.acListUrl.LastIndexOf("/") + 1) + "AirportDataThumbnails.json?icao=" + aircraft.ICAO + "&numThumbs=" + imageLinks.Length);
                request.Method = "GET";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Timeout = 5000;

                //If vrs authentication is used, add credentials to request
                if (Settings.VRSAuthenticate) {
                    string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Settings.VRSUsr + ":" + Settings.VRSPwd));
                    request.Headers.Add("Authorization", "Basic " + encoded);
                }

                //Send request and parse response
                try {
                    //Get response
                    HttpWebResponse imageResponse = (HttpWebResponse)request.GetResponse();
                    byte[] imageResponseBytes = new byte[32768];
                    imageResponse.GetResponseStream().Read(imageResponseBytes, 0, 32768);
                    string imageResponseText = Encoding.ASCII.GetString(imageResponseBytes);

                    //Parse json
                    JObject imageResponseJson = (JObject)JsonConvert.DeserializeObject(imageResponseText);

                    //If status is not 404, add images to image HTML
                    if (imageResponseJson["status"].ToString() != "404")
                        foreach (JObject image in imageResponseJson["data"])
                            imageHTML += "<img style='margin: 5px;border: 2px solid;border-radius: 10px;' src='" + image["image"].Value<string>() + "' />";
                    imageHTML += "<br>";
                }
                catch (Exception) {

                }
				
				//Generate google maps url
				googleMapsUrl = "https://www.google.com.au/maps/search/" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long");

				//Generate airframes.org url
				if (aircraft.GetProperty("Reg") != null && aircraft.GetProperty("Reg") != "")
                    airframesUrl = "<h3><a style='text-decoration: none;' href='http://www.airframes.org/reg/" + aircraft.GetProperty("Reg").Replace("-", "").ToUpper() + "'>Airframes.org Lookup</a></h3>";

                //Get name of transponder type
                transponderName = transponderTypes[Convert.ToInt32(aircraft.GetProperty("Trt")) - 1];

				//Write to UI
                Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | SENDING    | " + aircraft.ICAO + " | " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightBlue);

                //Generate aircraft property value table
                bool isAlternateStyle = false;
                foreach (string child in aircraft.GetPropertyKeys()) {
                    //TODO ADD TO VRSPROPERTIES
                    string parameter = "UNKNOWN_PARAMETER";
                    //Set parameter to a readable name if it's not included in vrs property info
                    switch (child.ToString()) {
                        case "CNum":
                            parameter = "Aircraft_Serial";
                            break;
                        case "EngMount":
                            parameter = "Engine_Mount";
                            break;
                        case "FSeen":
                            parameter = "First_Seen";
                            break;
                        case "HasPic":
                            parameter = "Has_Picture";
                            break;
                        case "Id":
                            parameter = "Id";
                            break;
                        case "Lat":
                            parameter = "Latitude";
                            break;
                        case "Long":
                            parameter = "Longitude";
                            break;
                        case "PosTime":
                            parameter = "Position_Time";
                            break;
                        case "ResetTrail":
                            parameter = "Reset_Trail";
                            break;
                        case "Tisb":
                            parameter = "TIS-B";
                            break;
                        case "Trak":
                            parameter = "Track";
                            break;
                        case "TrkH":
                            parameter = "Is_Track_Heading";
                            break;
                        case "Year":
                            parameter = "Year";
                            break;
                    }
                    //If parameter is set (not in vrs property info list) and property list type is set to essentials, skip this property
                    if (parameter != "UNKNOWN_PARAMETER" && Settings.EmailContentConfig.PropertyList == Core.PropertyListType.Essentials)
                        continue;
                    //Get parameter information from vrs property info
                    if (parameter == "UNKNOWN_PARAMETER") {
                        foreach (Core.vrsProperty property in Core.vrsPropertyData.Keys) {
                            if (Core.vrsPropertyData[property][2] == child.ToString()) {
                                //If property list type is essentials and this property is not in the list of essentials, leave this property as unknown so it can be skipped
                                if (Settings.EmailContentConfig.PropertyList == Core.PropertyListType.Essentials && !Core.essentialProperties.Contains(property))
                                    continue;
                                parameter = property.ToString();
                            }
                        }
                        if (parameter == "UNKNOWN_PARAMETER")
                            continue;
                    }
                    //Add html for property
                    if (isAlternateStyle)
                        aircraftTable += "<tr style='background-color:#CCC'>";
                    else
                        aircraftTable += "<tr>";
                    aircraftTable += "<td style='padding: 3px;font-weight:bold;'>" + parameter + "</td>";
                    aircraftTable += "<td style='padding: 3px'>" + aircraft.GetProperty(child) + "</td>";
                    aircraftTable += "</tr>";
                    isAlternateStyle = !isAlternateStyle;
                }
                aircraftTable += "</table>";

                //Create the main html document
                message.Body =
                    "<!DOCTYPE html PUBLIC ' -W3CDTD XHTML 1.0 TransitionalEN' 'http:www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><body>";
                if (isDetection)
                    message.Body += "<h1>Plane Alert - First Contact</h1>";
                else
                    message.Body += "<h1>Plane Alert - Last Contact</h1>";

                //Condition name + email property
                message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;margin-left: 10px;'>Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")</h2>";
                //Receiver name
                if (Settings.EmailContentConfig.ReceiverName)
                    message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;margin-left: 10px;'>Reciever: " + recieverName + "</h2>";
                //Transponder type
                if (Settings.EmailContentConfig.TransponderType)
                    message.Body += "<h2 style='margin: 0px;margin-bottom: 2px;margin-left: 10px;'>Transponder: " + transponderName + "</h2>";
                //Radar url
                if (Settings.EmailContentConfig.RadarLink)
                    message.Body += $"<h3><a style='text-decoration: none;' href='{Settings.radarUrl}?icao={aircraft.ICAO}'>Goto Radar</a></h3>";
                //Report url
                if (Settings.EmailContentConfig.ReportLink)
                    message.Body += $"<h3>VRS Report: <a style='text-decoration: none;' href='{Core.GenerateReportURL(aircraft.ICAO, false)}'>Desktop</a>   <a style='text-decoration: none;' href='{Core.GenerateReportURL(aircraft.ICAO, true)}'>Mobile</a></h3>";
                //Airframes.org url
                if (Settings.EmailContentConfig.AfLookup)
                    message.Body += airframesUrl;

                message.Body += "<table><tr><td>";
                //Property list
                if (Settings.EmailContentConfig.PropertyList != Core.PropertyListType.Hidden)
                    message.Body += aircraftTable;
                message.Body += "</td><td style='padding: 10px;vertical-align: top;'>";
                //Aircraft photos
                if (Settings.EmailContentConfig.AircraftPhotos)
                    message.Body += imageHTML;
                //Map
                if (Settings.EmailContentConfig.Map && aircraft.GetProperty("Lat") != null) {
                    message.Body += "<h3 style='margin: 0px'><a style='text-decoration: none' href=" + googleMapsUrl + ">Open in Google Maps</a></h3><br />" +
                    "<img style='border: 2px solid;border-radius: 10px;' alt='Loading Map...' src='" + Core.GenerateMapURL(aircraft).Replace("&", "&amp;") + "' />";
                }
                message.Body += "</td></tr></table></body></html>";
            }

			//Send the alert
			try {
				mailClient.Send(message);
			}
			catch(SmtpException e) {
				if(e.InnerException != null) {
					Core.UI.writeToConsole("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}
				Core.UI.writeToConsole("SMTP ERROR: " + e.Message, Color.Red);
				return;
			}
			catch (InvalidOperationException e) {
				if (e.InnerException != null) {
					Core.UI.writeToConsole("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}
				Core.UI.writeToConsole("SMTP ERROR: " + e.Message, Color.Red);
				return;
			}

			//Log to UI
			Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | SENT       | " + aircraft.ICAO + " | " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightBlue);
		}
	}
}
