namespace PlaneAlerter.Models
{
	/// <summary>
	/// Matched condition information
	/// </summary>
	public class MatchedCondition
	{
		/// <summary>
		/// Condition matched with
		/// </summary>
		public Condition Condition { get; }

		/// <summary>
		/// ID of condition matched with
		/// </summary>
		public int ConditionId { get; }

		/// <summary>
		/// Information of aircraft matched with
		/// </summary>
		public Aircraft AircraftInfo { get; set; }

		/// <summary>
		/// Create condition match
		/// </summary>
		/// <param name="conditionId">ID of condition</param>
		/// <param name="c">Condition matched with</param>
		/// <param name="aircraftInfo">Aircraft information of matched aircraft</param>
		public MatchedCondition(int conditionId, Condition c, Aircraft aircraftInfo)
		{
			Condition = c;
			AircraftInfo = aircraftInfo;
			ConditionId = conditionId;
		}
	}
}
