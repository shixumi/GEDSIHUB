using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:SMTPHost"],
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SMTPEmail"],
                        _configuration["EmailSettings:SMTPPassword"]
                    )
                };

                using (var mailMessage = new MailMessage(
                    _configuration["EmailSettings:FromEmail"],
                    email,
                    subject,
                    htmlMessage))
                {
                    mailMessage.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception($"Error sending email: {ex.InnerException.Message}", ex.InnerException);
                }
                throw new Exception($"Error sending email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, byte[] pdfBytes, string fileName)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:SMTPHost"],
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SMTPEmail"],
                        _configuration["EmailSettings:SMTPPassword"]
                    )
                };

                using (var mailMessage = new MailMessage(
                    _configuration["EmailSettings:FromEmail"],
                    email,
                    subject,
                    htmlMessage))
                {
                    mailMessage.IsBodyHtml = true;

                    var attachment = new Attachment(new MemoryStream(pdfBytes), fileName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception($"Error sending email with attachment: {ex.InnerException.Message}", ex.InnerException);
                }
                throw new Exception($"Error sending email with attachment: {ex.Message}", ex);
            }
        }
    }
}
