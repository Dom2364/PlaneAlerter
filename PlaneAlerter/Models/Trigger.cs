using PlaneAlerter.Enums;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Trigger information
	/// </summary>
	internal class Trigger
	{
		/// <summary>
		/// Property to be checked
		/// </summary>
		public VrsProperty Property;

		/// <summary>
		/// Value to be checked against
		/// </summary>
		public string Value;

		/// <summary>
		/// Type of comparison
		/// </summary>
		public string ComparisonType;

		/// <summary>
		/// Trigger constructor
		/// </summary>
		/// <param name="property">Property to check</param>
		/// <param name="value">Value to be checked against</param>
		/// <param name="comparisonType">Type of comparison</param>
		public Trigger(VrsProperty property, string value, string comparisonType)
		{
			Property = property;
			Value = value;
			ComparisonType = comparisonType;
		}
	}
}
