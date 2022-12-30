using System;

namespace PlaneAlerter.Services {
	internal interface IStatsService
	{
		event EventHandler StatsChanged;

		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		int TotalAlertsSent { get; set; }

		/// <summary>
		/// Time planealerter started
		/// </summary>
		DateTime TimeStarted { get; }
	}

	/// <summary>
	/// Class for statistic operations
	/// </summary>
	internal class StatsService : IStatsService
	{
		public event EventHandler StatsChanged;

		/// <summary>
		/// Counter for total alerts sent
		/// </summary>
		private int _totalAlertsSent = 0;
		public int TotalAlertsSent {
			get => _totalAlertsSent;
			set
			{
				_totalAlertsSent = value;
				StatsChanged?.Invoke(this, EventArgs.Empty);
			} }

		/// <summary>
		/// Time planealerter started
		/// </summary>
		public DateTime TimeStarted { get; set; } = DateTime.Now;
	}
}
