// File: Services/EmailSender.cs
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["EmailSettings:SMTPHost"],
                Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:FromEmail"],
                    _configuration["EmailSettings:FromEmailPassword"])
            };

            using (var mailMessage = new MailMessage(_configuration["EmailSettings:FromEmail"], email, subject, htmlMessage))
            {
                mailMessage.IsBodyHtml = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
