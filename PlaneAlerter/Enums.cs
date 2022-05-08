using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneAlerter {
	/// <summary>
	/// Enumerations used in VRS properties
	/// </summary>
	internal static class Enums {
        public static bool TryGetConvertedValue(string propertykey, string value, out string convertedvalue) {
            switch (propertykey) {
                case "Trt":
                    convertedvalue = TransponderTypes[Convert.ToInt32(value)];
                    return true;
                case "Species":
                    convertedvalue = Species[Convert.ToInt32(value)];
                    return true;
                case "EngType":
                    convertedvalue = EngineType[Convert.ToInt32(value)];
                    return true;
                case "EngMount":
                    convertedvalue = EnginePlacement[Convert.ToInt32(value)];
                    return true;
                case "WTC":
                    convertedvalue = WakeTurbulenceCategory[Convert.ToInt32(value)];
                    return true;
                case "AltT":
                    convertedvalue = AltitudeType[Convert.ToInt32(value)];
                    return true;
                case "SpdTyp":
                    convertedvalue = SpeedType[Convert.ToInt32(value)];
                    return true;
                case "VsiT":
                    convertedvalue = VerticalSpeedType[Convert.ToInt32(value)];
                    return true;
            }
            convertedvalue = null;
            return false;
        }

        private static Dictionary<int, string> TransponderTypes = new Dictionary<int, string> {
            {0, "Unknown" },
            {1, "Mode S" },
            {2, "ADS-B" },
            {3, "ADS-B v0" },
            {4, "ADS-B v1" },
            {5, "ADS-B v2" },
        };

        private static Dictionary<int, string> Species = new Dictionary<int, string> {
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

        private static Dictionary<int, string> EngineType = new Dictionary<int, string> {
            { 0, "None" },
            { 1, "Piston" },
            { 2, "Turbo" },
            { 3, "Jet" },
            { 4, "Electric" },
            { 5, "Rocket" },
        };

        private static Dictionary<int, string> EnginePlacement = new Dictionary<int, string> {
            { 0, "Unknown" },
            { 1, "Aft Mounted" },
            { 2, "Wing Buried" },
            { 3, "Fuselage Buried" },
            { 4, "Nose Mounted" },
            { 5, "Wing Mounted" },
        };

        private static Dictionary<int, string> WakeTurbulenceCategory = new Dictionary<int, string> {
            { 0, "None" },
            { 1, "Light" },
            { 2, "Medium" },
            { 3, "Heavy" },
        };

        private static Dictionary<int, string> AltitudeType = new Dictionary<int, string> {
            { 0, "Standard Pressure Altitude" },
            { 1, "Indicated Altitude" }
        };

        private static Dictionary<int, string> SpeedType = new Dictionary<int, string> {
            { 0, "Ground Speed" },
            { 1, "Ground Speed, Reversing" },
            { 2, "Indicated Air speed" },
            { 3, "True Air Speed" },
        };

        private static Dictionary<int, string> VerticalSpeedType = new Dictionary<int, string> {
            { 0, "Barometric Vertical Speed" },
            { 1, "Geometric Vertical Speed" },
        };
    }
}
