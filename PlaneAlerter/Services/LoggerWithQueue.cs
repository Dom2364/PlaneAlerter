using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface ILoggerWithQueue
	{
		event EventHandler NewLogMessage;
		IEnumerable<LogMessage> DequeueLogsForUi(int maxCount = 10);
		void Log(string message, Color? color = null);
		void LogWithTimeAndAircraft(Aircraft aircraft, string action, string message, Color? color = null);
	}

	internal class LoggerWithQueue : ILoggerWithQueue
	{
		private readonly Queue<LogMessage> _queuedLogs = new();

		public event EventHandler? NewLogMessage;

		public IEnumerable<LogMessage> DequeueLogsForUi(int maxCount = 10)
		{
			for (var i = 0; i < maxCount && _queuedLogs.Count > 0; i++)
			{
				yield return _queuedLogs.Dequeue();
			}
		}

		public void Log(string message, Color? color = null)
		{
			var messageWithTimestamp = $"{DateTime.Now.ToString("d", CultureInfo.InstalledUICulture)} {DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InstalledUICulture)} | {message}";
            
			_queuedLogs.Enqueue(new LogMessage(messageWithTimestamp, color));
			NewLogMessage?.Invoke(this, EventArgs.Empty);
		}

		public void LogWithTimeAndAircraft(Aircraft aircraft, string action, string message, Color? color = null)
		{
			Log($"{action.PadRight(10)} | {aircraft.Icao} - {aircraft.GetProperty("Reg")} - {aircraft.GetProperty("Call")} | " + message, color);
		}
	}
}
