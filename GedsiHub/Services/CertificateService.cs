using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class CertificateService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSender _emailSender;

        // Constructor with optional EmailSender for migrations
        public CertificateService(ApplicationDbContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // Method to generate and store certificate
        public async Task<byte[]> GenerateAndStoreCertificateAsync(string userId, int moduleId)
        {
            // Fetch user and module details
            var user = await _context.Users.FindAsync(userId);
            var module = await _context.Modules.FindAsync(moduleId);

            if (user == null || module == null)
            {
                throw new Exception("User or module not found.");
            }

            // Check if a certificate has already been issued
            var existingCertificate = await _context.Certificates
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ModuleId == moduleId);

            // For testing, comment out this block to allow re-issuing of certificates
            // if (existingCertificate != null)
            // {
            //     throw new Exception("Certificate already issued for this module.");
            // }

            // Generate the PDF certificate
            var certificateData = new CertificateData
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                CourseTitle = module.Title,
                CompletionDate = DateTime.UtcNow
            };

            var pdfBytes = GenerateCertificatePdf(certificateData);

            // Save the certificate in the database
            var certificate = new Certificate
            {
                UserId = userId,
                ModuleId = moduleId,
                CertificateUrl = "", // If storing URL (otherwise, save file in storage)
                IssueDate = DateTime.UtcNow
            };

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            // After the certificate has been generated and saved, send the email
            if (!string.IsNullOrEmpty(user.Email))
            {
                await SendCertificateEmail(user.Email, "Your Certificate of Completion", pdfBytes, "certificate.pdf");
            }

            // Return the generated PDF bytes
            return pdfBytes;
        }

        // Method to send certificate via email
        public async Task SendCertificateEmail(string email, string subject, byte[] pdfBytes, string fileName)
        {
            if (_emailSender != null)
            {
                // Log the email sending attempt for debugging purposes
                Console.WriteLine($"Sending email to {email} with subject {subject}");

                try
                {
                    await _emailSender.SendEmailWithAttachmentAsync(email, subject, "Here is your certificate.", pdfBytes, fileName);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during the email sending process
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Email sender service is not available.");
            }
        }

        // Method to generate the PDF certificate
        private byte[] GenerateCertificatePdf(CertificateData data)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape()); // Set the page to landscape orientation
                    page.Margin(2, Unit.Centimetre); // Page margin
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    // Defining the content within a bordered section
                    page.Content().Padding(20).Column(column =>
                    {
                        column.Spacing(20);

                        // Border around the whole content section
                        column.Item().Border(2).BorderColor(Colors.Black).Padding(20).Column(contentColumn =>
                        {
                            contentColumn.Spacing(20);

                            // Header section
                            contentColumn.Item().AlignCenter().Text("Certificate of Completion")
                                .FontSize(40)
                                .SemiBold()
                                .FontColor(Color.FromHex("#007BFF")); // Blue color

                            // Certificate details
                            contentColumn.Item().AlignCenter().Text($"This is to certify that")
                                .FontSize(24)
                                .FontColor(Colors.Black);

                            contentColumn.Item().AlignCenter().Text($"{data.FullName}")
                                .Bold()
                                .FontSize(36)
                                .FontColor(Color.FromHex("#007BFF")); // Blue color

                            contentColumn.Item().AlignCenter().Text($"has successfully completed the module")
                                .FontSize(24)
                                .FontColor(Colors.Black);

                            contentColumn.Item().AlignCenter().Text($"{data.CourseTitle}")
                                .Bold()
                                .FontSize(32)
                                .FontColor(Color.FromHex("#007BFF")); // Blue color

                            contentColumn.Item().AlignCenter().Text($"on {data.CompletionDate.ToString("MMMM dd, yyyy")}")
                                .FontSize(20)
                                .FontColor(Colors.Black);

                            // Footer with signature and date placeholders
                            contentColumn.Item().Height(40); // Space before the footer
                            contentColumn.Item().Row(row =>
                            {
                                row.RelativeColumn().Text("Signature: _______________").FontSize(20);
                                row.RelativeColumn().AlignRight().Text("Date: _______________").FontSize(20);
                            });
                        });
                    });

                    // Bottom Footer (e.g., Organization name)
                    page.Footer().AlignCenter().Text("GEDSI Hub")
                        .FontSize(16)
                        .FontColor(Color.FromHex("#808080")); // Gray color
                });
            });

            return document.GeneratePdf();
        }
    }

    // CertificateData DTO used to pass certificate details
    public class CertificateData
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CourseTitle { get; set; }
        public DateTime CompletionDate { get; set; }

        public string FullName => string.IsNullOrEmpty(MiddleName)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {MiddleName} {LastName}";
    }
}
