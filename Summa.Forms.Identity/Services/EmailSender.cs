using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Summa.Forms.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                var credential = new NetworkCredential
                {
                    UserName = "summaforms@gmail.com",
                    Password = "29OHA3Npr5pl"
                };
                
                client.Credentials = credential;
                client.Port = 587;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress("summaforms@gmail.com");
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    emailMessage.IsBodyHtml = true;
                    client.Send(emailMessage);
                }
            }

            await Task.CompletedTask;
        }
    }
}