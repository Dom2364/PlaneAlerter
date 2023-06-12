using System;
using System.Collections.Generic;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Match information
	/// </summary>
	internal class Match
	{
		/// <summary>
		/// ICAO of aircraft matched
		/// </summary>
		public string Icao { get; }

		/// <summary>
		/// Time signal was lost
		/// </summary>
		public DateTime SignalLostTime { get; set; }

		/// <summary>
		/// Has signal been lost?
		/// </summary>
		public bool SignalLost { get; set; }

		/// <summary>
		/// Ignore if any other alerts match for this aircraft
		/// </summary>
		public bool IgnoreFollowing { get; set; }

		/// <summary>
		/// Conditions matched to
		/// </summary>
		public List<MatchedCondition> Conditions { get; }

		/// <summary>
		/// Information of aircraft matched with
		/// </summary>
		public Aircraft AircraftInfo { get; set; }

		/// <summary>
		/// Add a condition to the match
		/// </summary>
		/// <param name="id">ID of condition</param>
		/// <param name="condition">Condition</param>
		public void AddCondition(int id, Condition condition)
		{
			Conditions.Add(new MatchedCondition(id, condition));
			IgnoreFollowing = IgnoreFollowing || condition.IgnoreFollowing;
		}

		/// <summary>
		/// Create match
		/// </summary>
		/// <param name="icao">ICAO of aircraft matched</param>
		/// <param name="aircraftInfo">Aircraft information of matched aircraft</param>
		public Match(string icao, Aircraft aircraftInfo)
		{
			Icao = icao;
			Conditions = new List<MatchedCondition>();
			AircraftInfo = aircraftInfo;
		}
	}
}
