using System;
using System.Collections.Generic;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Class to store aircraft information
	/// </summary>
	internal class Aircraft
	{
		/// <summary>
		/// List of property values retrieved from last aircraftlist.json
		/// </summary>
		private readonly Dictionary<string, string?> _properties = new Dictionary<string, string?>();

		/// <summary>
		/// ICAO hex of aircraft
		/// </summary>
		public string Icao { get; set; }

		/// <summary>
		/// Position track from last aircraftlist.json
		/// </summary>
		public double[] Trail { get; set; } = Array.Empty<double>();

		/// <summary>
		/// Get property value from property list
		/// </summary>
		/// <param name="key">Property name</param>
		/// <returns>Value of property</returns>
		public string? GetProperty(string key)
		{
			return _properties.ContainsKey(key) ? _properties[key] : null;
		}

		/// <summary>
		/// Set property value
		/// </summary>
		/// <param name="key">Property name</param>
		/// <param name="value">Value</param>
		public void SetProperty(string key, string? value)
		{
			if (_properties.ContainsKey(key))
				_properties[key] = value;
		}

		/// <summary>
		/// Add a property to the list
		/// </summary>
		/// <param name="key">Key of property</param>
		/// <param name="value">Value of property</param>
		public void AddProperty(string key, string value)
		{
			//Add if property is not position trail or stops list
			//Arrays can't be used for conditions or displaying so theyre just ignored
			if (key != "Cot" && key != "Stops")
				_properties.Add(key, value);
		}

		/// <summary>
		/// Get list of keys from property list
		/// </summary>
		/// <returns>List of property keys</returns>
		public Dictionary<string, string?>.KeyCollection GetPropertyKeys()
		{
			return _properties.Keys;
		}

		/// <summary>
		/// ToJSON
		/// </summary>
		public string ToJson()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(_properties);
		}

		public override string ToString()
		{
			return $"Aircraft: {GetProperty("Icao")} | {GetProperty("Reg")} | {GetProperty("Type")} | {GetProperty("Call")}";
		}

		/// <summary>
		/// Aircraft class constructor
		/// </summary>
		/// <param name="icao">ICAO of aircraft</param>
		public Aircraft(string icao)
		{
			Icao = icao;
		}
	}
}
