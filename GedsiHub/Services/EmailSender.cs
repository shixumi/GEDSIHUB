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
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:FromEmail"],
                        _configuration["EmailSettings:FromEmailPassword"])
                };

                using (var mailMessage = new MailMessage(_configuration["EmailSettings:FromEmail"], email, subject, htmlMessage))
                {
                    mailMessage.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine($"Email sent to {email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {email}: {ex.Message}");
                throw;
            }
        }

        // Method to send email with PDF attachment
        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, byte[] pdfBytes, string fileName)
        {
            try
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

                    // Create attachment from the byte array and add it to the email
                    var attachment = new Attachment(new MemoryStream(pdfBytes), fileName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);

                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine($"Email with attachment sent to {email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email with attachment to {email}: {ex.Message}");
                throw;
            }
        }
    }
}
