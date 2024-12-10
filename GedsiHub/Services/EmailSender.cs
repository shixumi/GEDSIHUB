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
                // Initialize SMTP client with settings from appsettings.json
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:SMTPHost"],
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    EnableSsl = _configuration.GetValue<bool>("EmailSettings:EnableSsl"),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SMTPEmail"], // SMTP email for authentication
                        _configuration["EmailSettings:SMTPPassword"] // SMTP password (API key) for authentication
                    )
                };

                // Prepare the email
                using (var mailMessage = new MailMessage(
                    _configuration["EmailSettings:FromEmail"], // The validated sender email
                    email,                                     // The recipient email
                    subject,                                   // The email subject
                    htmlMessage))                              // The email body
                {
                    mailMessage.IsBodyHtml = true;

                    // Send the email
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

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, byte[] pdfBytes, string fileName)
        {
            try
            {
                // Initialize SMTP client with settings from appsettings.json
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:SMTPHost"],
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    EnableSsl = _configuration.GetValue<bool>("EmailSettings:EnableSsl"),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SMTPEmail"], // SMTP email for authentication
                        _configuration["EmailSettings:SMTPPassword"] // SMTP password (API key) for authentication
                    )
                };

                // Prepare the email with attachment
                using (var mailMessage = new MailMessage(
                    _configuration["EmailSettings:FromEmail"], // The validated sender email
                    email,                                     // The recipient email
                    subject,                                   // The email subject
                    htmlMessage))                              // The email body
                {
                    mailMessage.IsBodyHtml = true;

                    // Attach the PDF file
                    var attachment = new Attachment(new MemoryStream(pdfBytes), fileName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);

                    // Send the email
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