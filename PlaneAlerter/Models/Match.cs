using System;
using System.Collections.Generic;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Match information
	/// </summary>
	public class Match
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
		/// Add a condition to the match
		/// </summary>
		/// <param name="id">ID of condition</param>
		/// <param name="match">Condition</param>
		/// <param name="aircraftInfo">Aircraft information when matched</param>
		public void AddCondition(int id, Condition match, Aircraft aircraftInfo)
		{
			Conditions.Add(new MatchedCondition(id, match, aircraftInfo));
			IgnoreFollowing = match.IgnoreFollowing;
		}

		/// <summary>
		/// Remove condition from match
		/// </summary>
		/// <param name="match"></param>
		public void RemoveCondition(MatchedCondition match)
		{
			Conditions.Remove(match);
		}

		/// <summary>
		/// Create match
		/// </summary>
		/// <param name="icao">ICAO of aircraft matched</param>
		public Match(string icao)
		{
			Icao = icao;
			Conditions = new List<MatchedCondition>();
		}
	}
}
