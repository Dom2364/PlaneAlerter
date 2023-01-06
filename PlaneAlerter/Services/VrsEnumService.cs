using System.Collections.Generic;

namespace PlaneAlerter.Services
{
	internal interface IVrsEnumService
	{
		bool TryToString(string propertyKey, string? value, out string? convertedValue);
	}

	/// <summary>
    /// Enumerations used in VRS properties
    /// </summary>
    internal class VrsEnumService : IVrsEnumService
	{
        public bool TryToString(string propertyKey, string? value, out string? convertedValue)
        {
	        if (value == null)
	        {
                convertedValue = null;
                return false;
	        }

            if (!int.TryParse(value, out var result))
            {
                convertedValue = null;
                return false;
            }

            switch (propertyKey)
            {
                case "Trt":
                    convertedValue = TransponderTypes[result];
                    return true;
                case "Species":
                    convertedValue = Species[result];
                    return true;
                case "EngType":
                    convertedValue = EngineType[result];
                    return true;
                case "EngMount":
                    convertedValue = EnginePlacement[result];
                    return true;
                case "WTC":
                    convertedValue = WakeTurbulenceCategory[result];
                    return true;
                case "AltT":
                    convertedValue = AltitudeType[result];
                    return true;
                case "SpdTyp":
                    convertedValue = SpeedType[result];
                    return true;
                case "VsiT":
                    convertedValue = VerticalSpeedType[result];
                    return true;
            }

            convertedValue = null;
            return false;
        }

        private static readonly Dictionary<int, string> TransponderTypes = new()
        {
            {0, "Unknown" },
            {1, "Mode S" },
            {2, "ADS-B" },
            {3, "ADS-B v0" },
            {4, "ADS-B v1" },
            {5, "ADS-B v2" },
        };

        private static readonly Dictionary<int, string> Species = new()
        {
            { 0, "None" },
            { 1, "Land Plane" },
            { 2, "Sea Plane" },
            { 3, "Amphibian" },
            { 4, "Helicopter" },
            { 5, "Gyrocopter" },
            { 6, "Tiltwing" },
            { 7, "Ground Vehicle" },
            { 8, "Tower" },
        };

        private static readonly Dictionary<int, string> EngineType = new()
        {
            { 0, "None" },
            { 1, "Piston" },
            { 2, "Turbo" },
            { 3, "Jet" },
            { 4, "Electric" },
            { 5, "Rocket" },
        };

        private static readonly Dictionary<int, string> EnginePlacement = new()
        {
            { 0, "Unknown" },
            { 1, "Aft Mounted" },
            { 2, "Wing Buried" },
            { 3, "Fuselage Buried" },
            { 4, "Nose Mounted" },
            { 5, "Wing Mounted" },
        };

        private static readonly Dictionary<int, string> WakeTurbulenceCategory = new()
        {
            { 0, "None" },
            { 1, "Light" },
            { 2, "Medium" },
            { 3, "Heavy" },
        };

        private static readonly Dictionary<int, string> AltitudeType = new()
        {
            { 0, "Standard Pressure Altitude" },
            { 1, "Indicated Altitude" }
        };

        private static readonly Dictionary<int, string> SpeedType = new()
        {
            { 0, "Ground Speed" },
            { 1, "Ground Speed, Reversing" },
            { 2, "Indicated Air speed" },
            { 3, "True Air Speed" },
        };

        private static readonly Dictionary<int, string> VerticalSpeedType = new()
        {
            { 0, "Barometric Vertical Speed" },
            { 1, "Geometric Vertical Speed" },
        };
    }
}
