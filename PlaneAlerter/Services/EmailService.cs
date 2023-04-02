using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;

namespace PlaneAlerter.Services {
	internal interface IEmailService
	{
		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="message">Message to send</param>
		/// <param name="emailAddress">Email address to send to</param>
		void SendEmail(MailMessage message, string emailAddress);
	}

	/// <summary>
	/// Class for email operations
	/// </summary>
	internal class EmailService : IEmailService
	{
        private readonly ISettingsManagerService _settingsManagerService;
        private readonly ILoggerWithQueue _logger;

		public EmailService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
            _logger = logger;
		}

		/// <summary>
		/// Send alert email
		/// </summary>
		/// <param name="message">Message to send</param>
		/// <param name="emailAddress">Email address to send to</param>
		public void SendEmail(MailMessage message, string emailAddress) {
            using var mailClient = new SmtpClient(_settingsManagerService.Settings.SMTPHost)
            {
	            Port = _settingsManagerService.Settings.SMTPPort,
	            Credentials = new NetworkCredential(_settingsManagerService.Settings.SMTPUser,
		            _settingsManagerService.Settings.SMTPPassword),
	            EnableSsl = _settingsManagerService.Settings.SMTPSSL
            };

            //Set sender email to the one set in settings
			try
			{
				message.From = new MailAddress(_settingsManagerService.Settings.SenderEmail, "PlaneAlerter Alerts");
			}
			catch
			{
				_logger.Log(
					"ERROR: Email to send from is invalid (" + _settingsManagerService.Settings.SenderEmail + ")",
					Color.Red);
				return;
			}

			//Add email to message receiver list
			try
			{
				message.To.Clear();
				message.To.Add(emailAddress);
			}
			catch
			{
				_logger.Log("ERROR: Email to send to is invalid (" + emailAddress + ")", Color.Red);
				return;
			}


			//Send the alert
			try {
				mailClient.SendAsync(message, null);
			}
			catch (SmtpException e) {
				if (e.InnerException != null) {
					_logger.Log("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}

				_logger.Log("SMTP ERROR: " + e.Message, Color.Red);
			}
			catch (InvalidOperationException e) {
				if (e.InnerException != null) {
					_logger.Log("SMTP ERROR: " + e.Message + " (" + e.InnerException.Message + ")", Color.Red);
					return;
				}

				_logger.Log("SMTP ERROR: " + e.Message, Color.Red);
			}
		}
	}
}
