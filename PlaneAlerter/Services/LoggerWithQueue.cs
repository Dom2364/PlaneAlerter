using System;
using System.Collections.Generic;
using System.Drawing;
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
		private readonly Queue<LogMessage> _queuedLogs;

		public LoggerWithQueue()
		{
			_queuedLogs = new Queue<LogMessage>();
		}

		public event EventHandler NewLogMessage;

		public IEnumerable<LogMessage> DequeueLogsForUi(int maxCount = 10)
		{
			for (var i = 0; i < maxCount && _queuedLogs.Count > 0; i++)
			{
				yield return _queuedLogs.Dequeue();
			}
		}

		public void Log(string message, Color? color = null)
		{
			_queuedLogs.Enqueue(new LogMessage(message, color));
			NewLogMessage?.Invoke(this, EventArgs.Empty);
		}

		public void LogWithTimeAndAircraft(Aircraft aircraft, string action, string message, Color? color = null)
		{
			Log(DateTime.Now.ToLongTimeString() + $" | {action.PadRight(10)} | {aircraft.Icao} - {aircraft.GetProperty("Reg")} - {aircraft.GetProperty("Call")} | " + message, color);
		}
	}
}
