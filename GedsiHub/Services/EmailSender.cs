using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailSender(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var apiKey = _configuration["EmailSettings:APIKey"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                // Prepare the email payload
                var emailPayload = new
                {
                    sender = new { email = fromEmail, name = fromName },
                    to = new[] { new { email, name = "Recipient" } },
                    subject,
                    htmlContent = htmlMessage
                };

                // Add API key to the request headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                // Send the email via Brevo API
                var response = await _httpClient.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", emailPayload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Email sent successfully to {email}.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Error: {errorContent}");
                    throw new Exception($"Failed to send email: {errorContent}");
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
                var apiKey = _configuration["EmailSettings:APIKey"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                // Prepare the email payload with attachment
                var emailPayload = new
                {
                    sender = new { email = fromEmail, name = fromName },
                    to = new[] { new { email, name = "Recipient" } },
                    subject,
                    htmlContent = htmlMessage,
                    attachment = new[]
                    {
                        new
                        {
                            content = Convert.ToBase64String(pdfBytes),
                            name = fileName
                        }
                    }
                };

                // Add API key to the request headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                // Send the email via Brevo API
                var response = await _httpClient.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", emailPayload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Email with attachment sent successfully to {email}.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send email with attachment. Status: {response.StatusCode}, Error: {errorContent}");
                    throw new Exception($"Failed to send email with attachment: {errorContent}");
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
