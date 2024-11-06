using Azure.Communication.Email;
using Azure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly EmailClient _emailClient;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            // Initialize the EmailClient using the ACS connection string from the configuration
            var connectionString = _configuration["EmailSettings:ACSConnectionString"];

            // Debugging: Check if the connection string is null or empty
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("ACSConnectionString is null or empty. Check your appsettings.json.");
            }

            _emailClient = new EmailClient(connectionString);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // Create the email content
                var emailContent = new EmailContent(subject)
                {
                    Html = htmlMessage
                };

                // Create the email recipients
                var emailRecipients = new EmailRecipients(new[] { new EmailAddress(email) });

                // Create the email message
                var emailMessage = new EmailMessage(
                    senderAddress: _configuration["EmailSettings:FromEmail"],
                    recipients: emailRecipients,
                    content: emailContent
                );

                // Send the email
                var operation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
                Console.WriteLine($"Email sent to {email}. Operation ID: {operation.Id}");
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
                // Create the email content
                var emailContent = new EmailContent(subject)
                {
                    Html = htmlMessage
                };

                // Create the email recipients
                var emailRecipients = new EmailRecipients(new[] { new EmailAddress(email) });

                // Create the email message
                var emailMessage = new EmailMessage(
                    senderAddress: _configuration["EmailSettings:FromEmail"],
                    recipients: emailRecipients,
                    content: emailContent
                );

                // Add the PDF attachment using BinaryData
                var attachment = new EmailAttachment(
                    name: fileName,
                    contentType: "application/pdf",
                    content: BinaryData.FromBytes(pdfBytes)
                );
                emailMessage.Attachments.Add(attachment);

                // Send the email
                var operation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
                Console.WriteLine($"Email with attachment sent to {email}. Operation ID: {operation.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email with attachment to {email}: {ex.Message}");
                throw;
            }
        }
    }
}
