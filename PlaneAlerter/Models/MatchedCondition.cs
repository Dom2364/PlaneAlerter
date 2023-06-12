namespace PlaneAlerter.Models
{
	/// <summary>
	/// Matched condition information
	/// </summary>
	internal class MatchedCondition
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
		/// Create condition match
		/// </summary>
		/// <param name="conditionId">ID of condition</param>
		/// <param name="c">Condition matched with</param>
		public MatchedCondition(int conditionId, Condition c)
		{
			Condition = c;
			ConditionId = conditionId;
		}
	}
}
