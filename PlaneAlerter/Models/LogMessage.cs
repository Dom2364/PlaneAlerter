using System.Drawing;

namespace PlaneAlerter.Models
{
	internal class LogMessage
	{
		public string Message { get; set; }
		public Color? Color { get; set; }

		public LogMessage(string message, Color? color)
		{
			Message = message;
			Color = color;
		}
	}
}
