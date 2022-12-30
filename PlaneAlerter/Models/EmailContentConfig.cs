using PlaneAlerter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Email content config
	/// </summary>
	internal class EmailContentConfig
	{
		public bool ReceiverName { get; set; }
		public bool TransponderType { get; set; }
		public bool RadarLink { get; set; }
		public bool ReportLink { get; set; }
		public bool AfLookup { get; set; }
		public bool AircraftPhotos { get; set; }
		public bool Map { get; set; }
		public bool TwitterOptimised { get; set; }
		public bool KMLfile { get; set; }
		public PropertyListType PropertyList { get; set; }
	}
}
