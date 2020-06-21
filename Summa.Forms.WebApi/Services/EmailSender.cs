using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Summa.Forms.WebApi.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;
		private readonly NetworkCredential _credential;
		private readonly SmtpClient _client;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
			_credential = new NetworkCredential
			{
				UserName = _configuration["Mail:UserName"],
				Password = _configuration["Mail:Password"]
			};
			_client = new SmtpClient(_configuration["Mail:Host"])
			{
				Credentials = _credential,
				Port = Convert.ToInt32(_configuration["Mail:Port"]),
				EnableSsl = true
			};
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			using var emailMessage = new MailMessage
			{
				To = {new MailAddress(email)},
				From = new MailAddress(_configuration["Mail:UserName"]),
				Subject = subject,
				Body = message,
				IsBodyHtml = true
			};

			await _client.SendMailAsync(emailMessage);
		}
	}
}