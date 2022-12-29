using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneAlerter.Enums
{
	/// <summary>
	/// List of VRS properties
	/// </summary>
	/// //ADD NEW ITEMS TO END
	public enum VrsProperty
	{
		Time_Tracked,
		Receiver,
		Icao,
		Invalid_Icao,
		Registration,
		Altitude,
		Altitude_AMSL,
		Altitude_Type,
		Target_Altitude,
		Air_Pressure,
		Callsign,
		Callsign_Inaccurate,
		Speed,
		Speed_Type,
		Vertical_Speed,
		Vertical_Speed_Type,
		Aircraft_Model,
		Aircraft_Model_Icao,
		Departure_Airport,
		Arrival_Airport,
		Operator,
		Operator_Icao_Code,
		Squawk,
		Is_In_Emergency,
		Distance,
		Wake_Turbulence_Category,
		Engines,
		Engine_Type,
		Species,
		Is_Military,
		Registered_Country,
		Flight_Count,
		Message_Count,
		Is_On_Ground,
		User_Tag,
		Is_Interesting,
		Transponder_Type,
		Has_Signal_Level,
		Signal_Level,
		Mlat,
		Manufacturer,
		Bearing,
		Engine_Mount,
		Year,
		Serial,
		Is_Tisb,
		Track,
		Is_Track_Heading,
		Latitude,
		Longitude,
		Is_Ferry_Flight,
		Is_Charter_Flight,
		Ident,
		Target_Heading,
		Notes
	}
}
