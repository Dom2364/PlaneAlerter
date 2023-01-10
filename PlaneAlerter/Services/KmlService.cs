using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IKmlService
	{
		/// <summary>
		/// Generate a KML
		/// </summary>
		string GenerateTrailKml(Aircraft aircraft);
	}

	internal class KmlService : IKmlService
	{
		/// <summary>
		/// Generate a KML
		/// </summary>
		public string GenerateTrailKml(Aircraft aircraft)
		{
			var kmlData = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
	<Document>
		<name>{aircraft.GetProperty("Reg")}</name>
		<Placemark>
			<name>Flight path of {aircraft.GetProperty("Reg")}</name>
			<Style>
				<LineStyle>
					<color>7fffffff</color>
					<width>2</width>
				</LineStyle>
				<PolyStyle>
					<color>7fffffff</color>
				</PolyStyle>
			</Style>
			<LineString>
				<extrude>1</extrude>
				<altitudeMode>absolute</altitudeMode>
				<coordinates>";

			//Process aircraft trail
			for (var i = 0; i < aircraft.Trail.Length / 4; i++)
			{
				//Get coordinate
				var coordinate = new[]
				{
					aircraft.Trail[i * 4]?.ToString("#.####"),
					aircraft.Trail[i * 4 + 1]?.ToString("#.####"),
					((aircraft.Trail[i * 4 + 3] ?? 0) * 0.3048).ToString("#.####") // Convert feet to metres for KML
				};

				//Add coordinate to KML
				kmlData += coordinate[1] + "," + coordinate[0] + "," + coordinate[2] + " ";
			}

			//Add rest of KML
			kmlData += @"</coordinates>
			</LineString>
		</Placemark>
	</Document>
</kml>";

			return kmlData;
		}
	}
}
