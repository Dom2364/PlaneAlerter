using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace PlaneAlerter {
	static class Email {
		public static SmtpClient mailClient;

		public static void sendEmail(string emailaddress, MailMessage message, Core.Condition condition, Core.Aircraft aircraft, string recieverName, string emailPropertyInfo, bool isDetection) {
			try {
				message.To.Clear();
				message.To.Add(emailaddress);
			}
			catch (FormatException) {
				Core.UI.writeToConsole("ERROR: Email to send to is invalid (" + emailaddress + ")", Color.Red);
				return;
			}

			try {
				message.From = new MailAddress(Settings.senderEmail, "PlaneAlerter Alerts");
			}
			catch (FormatException) {
				Core.UI.writeToConsole("ERROR: Email to send from is invalid (" + Settings.senderEmail + ")", Color.Red);
				return;
			}
			message.IsBodyHtml = true;

			Dictionary<int, string[]> pathArray = new Dictionary<int, string[]>();
			string googleMapUrl = "";
			string transponderName = "";
			string[] transponderTypes = new string[] { "Unknown", "Mode-S", "ADS-B", "ADS-Bv1", "ADS-Bv2" };
			string[] imageLinks = new string[2];
			string aircraftTable = "<table style='border: 2px solid;border-radius: 10px;border-spacing: 0px;' id='acTable'>";
			string imageHTML = "";
			string airframesText = "";

			if (aircraft.GetProperty("Lat") != null) {
				if (aircraft.Trail.Count() != 3) {
					googleMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&amp;size=800x800&amp;markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&amp;path=color:0x000000|";
				}
				else {
					googleMapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&amp;size=800x800&amp;zoom=8&amp;markers=" + aircraft.GetProperty("Lat") + "," + aircraft.GetProperty("Long") + "&amp;path=color:0x000000|";
				}
			}

			//http://122.151.42.191/VirtualRadar/AirportDataThumbnails.json?icao=7C80BD&numThumbs=2
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.acListUrl.Substring(0, Settings.acListUrl.LastIndexOf("/") + 1) + "AirportDataThumbnails.json?icao=" + aircraft.ICAO + "&numThumbs=" + imageLinks.Length);
			request.Method = "GET";
			if (Settings.VRSAuthenticate) {
				String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Settings.VRSUsr + ":" + Settings.VRSPwd));
				request.Headers.Add("Authorization", "Basic " + encoded);
			}
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			request.Timeout = 5000;
			try {
				HttpWebResponse imageResponse = (HttpWebResponse)request.GetResponse();
				byte[] imageResponseBytes = new byte[32768];
				imageResponse.GetResponseStream().Read(imageResponseBytes, 0, 32768);
				string imageResponseText = ASCIIEncoding.ASCII.GetString(imageResponseBytes);

				JObject imageResponseJson = (JObject)JsonConvert.DeserializeObject(imageResponseText);
				if (imageResponseJson["status"].ToString() != "404") {
					int curImg = 0;
					foreach (JObject image in imageResponseJson["data"]) {
						imageLinks[curImg] = image["image"].ToString();
						curImg++;
					}
				}
			}
			catch (Exception) {

			}

			for (int i = 0;i < aircraft.Trail.Count() / 3;i++) {
				string[] coord = new string[] {
					aircraft.Trail[i * 3].ToString(),
					aircraft.Trail[i * 3 + 1].ToString(),
					aircraft.Trail[i * 3 + 2].ToString()
				};
				pathArray.Add(pathArray.Count, coord);
			}

			foreach (string[] point in pathArray.Values) {
				googleMapUrl += point[0] + "," + point[1];
				if (point[0] != pathArray[pathArray.Count - 1][0]) {
					googleMapUrl += "|";
				}
			}

			foreach (string image in imageLinks) {
				if (image != null) {
					imageHTML += "<img style='margin: 5px;border: 2px solid;border-radius: 10px;' src='" + image + "' />";
				}
			}
			imageHTML += "<br>";

			for (int i = 1;i < transponderTypes.Length;i++) {
				if (aircraft.GetProperty("Trt") == i.ToString()) {
					transponderName = transponderTypes[i];
				}
			}

			Core.logFileSW.WriteLine(DateTime.Now.ToLongTimeString() + " | SENDING ALERT: " + Environment.NewLine + aircraft.ToString() + Environment.NewLine + Environment.NewLine);
			Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | SENDING    | " + aircraft.ICAO + " | Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightBlue);

			bool isAlternateStyle = false;
			foreach (string child in aircraft.GetPropertyKeys()) {
				//TODO ADD TO VRSPROPERTIES
				string parameter = "UNKNOWN_PARAMETER";
				if (child.ToString() == "CNum") {
					parameter = "Aircraft_Serial";
				}
				if (child.ToString() == "EngMount") {
					parameter = "Engine_Mount";
				}
				if (child.ToString() == "FSeen") {
					parameter = "First_Seen";
				}
				if (child.ToString() == "HasPic") {
					parameter = "Has_Picture";
				}
				if (child.ToString() == "Id") {
					parameter = "Id";
				}
				if (child.ToString() == "Lat") {
					parameter = "Latitude";
				}
				if (child.ToString() == "Long") {
					parameter = "Longitude";
				}
				if (child.ToString() == "PosTime") {
					parameter = "Position_Time";
				}
				if (child.ToString() == "ResetTrail") {
					parameter = "Reset_Trail";
				}
				if (child.ToString() == "Tisb") {
					parameter = "TIS-B";
				}
				if (child.ToString() == "Trak") {
					parameter = "Track";
				}
				if (child.ToString() == "TrkH") {
					parameter = "Is_Track_Heading";
				}
				if (child.ToString() == "Year") {
					parameter = "Year";
				}
				if (Settings.EmailContentConfig.PropertyList == Core.PropertyListType.Essentials && parameter != "UNKNOWN_PARAMETER")
					continue;
				bool non_essential = false;
				if (parameter == "UNKNOWN_PARAMETER") {
					foreach (Core.vrsProperty property in Core.vrsPropertyData.Keys) {
						if (Core.vrsPropertyData[property][2] == child.ToString()) {
							if (Settings.EmailContentConfig.PropertyList == Core.PropertyListType.Essentials && !Core.essentialProperties.Contains(property)) {
								non_essential = true;
								continue;
							}
							parameter = property.ToString();
						}
					}
					if (parameter == "UNKNOWN_PARAMETER")
						continue;
				}
				if (non_essential)
					continue;

				if (isAlternateStyle) {
					aircraftTable += "<tr style='background-color:#CCC'>";
				}
				else {
					aircraftTable += "<tr>";
				}
				aircraftTable += "<td style='padding: 3px;font-weight:bold;'>" + parameter + "</td>";
				aircraftTable += "<td style='padding: 3px'>" + aircraft.GetProperty(child) + "</td>";
				aircraftTable += "</tr>";
				isAlternateStyle = !isAlternateStyle;
			}
			aircraftTable += "</table>";

			message.Body =
				"<!DOCTYPE html PUBLIC ' -W3CDTD XHTML 1.0 TransitionalEN' 'http:www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><body>";
			if (isDetection) {
				message.Body += "<h1>Plane Alert - Last Contact</h1>";
			}
			else {
				message.Body += "<h1>Plane Alert - First Contact</h1>";
			}
			if (aircraft.GetProperty("Reg") != null && aircraft.GetProperty("Reg") != "") {
				airframesText = "<h3><a style='text-decoration: none;' href='http://www.airframes.org/reg/" + aircraft.GetProperty("Reg").Replace("-", "").ToUpper() + "'>Airframes.org Lookup</a></h3>";
			}
			string reportUrl = "";
			try {
				if (Settings.radarUrl[Settings.radarUrl.Length - 1] == "/"[0]) {
					reportUrl = Settings.radarUrl + "desktopReport.html?icao-Q=" + aircraft.ICAO;
				}
				else {
					reportUrl = Settings.radarUrl + "/desktopReport.html?icao-Q=" + aircraft.ICAO;
				}
			}
			catch {
				Core.UI.writeToConsole("ERROR: No radar url specified.", Color.Red);
				ThreadManager.Restart();
			}

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
				message.Body += "<h3><a style='text-decoration: none;' href='" + Settings.radarUrl + "'>Goto Radar</a></h3>";
			//Report url
			if (Settings.EmailContentConfig.ReportLink)
				message.Body += "<h3><a style='text-decoration: none;' href='" + reportUrl + "'>VRS Report Lookup</a></h3>";
			//Airframes.org url
			if (Settings.EmailContentConfig.AfLookup)
				message.Body += airframesText;

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
				message.Body += "<h3 style='margin: 0px'><a style='text-decoration: none' href=" + googleMapUrl + ">Open map</a></h3><br />" +
				"<img style='border: 2px solid;border-radius: 10px;' alt='Loading Map...' src='" + googleMapUrl + "' />";
			}
			message.Body += "</td></tr></table></body></html>";

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

			condition.increaseSentEmails();
			Stats.updateStats();

			Core.UI.writeToConsole(DateTime.Now.ToLongTimeString() + " | SENT       | " + aircraft.ICAO + " | Condition: " + condition.conditionName + " (" + emailPropertyInfo + ")", Color.LightBlue);
		}
	}
}
