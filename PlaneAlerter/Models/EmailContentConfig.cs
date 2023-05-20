using PlaneAlerter.Enums;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Email content config
	/// </summary>
	internal class EmailContentConfig
	{
		public EmailContentConfig(
			bool afLookup,
			bool aircraftPhotos,
			bool map,
			bool kmlFile,
			PropertyListType propertyList,
			bool radarLink,
			bool reportLink,
			bool receiverName,
			bool transponderType,
			bool twitterOptimised)
		{
			AfLookup = afLookup;
			AircraftPhotos = aircraftPhotos;
			Map = map;
			KmlFile = kmlFile;
			PropertyList = propertyList;
			RadarLink = radarLink;
			ReportLink = reportLink;
			ReceiverName = receiverName;
			TransponderType = transponderType;
			TwitterOptimised = twitterOptimised;
		}

		public bool ReceiverName { get; }
		public bool TransponderType { get; }
		public bool RadarLink { get; }
		public bool ReportLink { get; }
		public bool AfLookup { get; }
		public bool AircraftPhotos { get; }
		public bool Map { get; }
		public bool TwitterOptimised { get; }
		public bool KmlFile { get; }
		public PropertyListType PropertyList { get; }
	}
}
