﻿using DinkToPdf;
using DinkToPdf.Contracts;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class CertificateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;
        private readonly EmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;  // For accessing wwwroot
        private readonly ILogger<CertificateService> _logger;

        // Constructor with IWebHostEnvironment for image path resolution
        public CertificateService(ApplicationDbContext context, IConverter converter, EmailSender emailSender, IWebHostEnvironment webHostEnvironment, ILogger<CertificateService> logger)
        {
            _context = context;
            _converter = converter;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;  // Store the web host environment
            _logger = logger;
        }

        // Method to generate and store certificate
        public async Task<byte[]> GenerateAndStoreCertificateAsync(string userId, int moduleId)
        {
            // Fetch user and module details
            var user = await _context.Users.FindAsync(userId);
            var module = await _context.Modules.FindAsync(moduleId);

            _logger.LogInformation($"Generating certificate for UserID: {userId}, ModuleID: {moduleId}");

            if (user == null || module == null)
            {
                throw new Exception("User or module not found.");
            }

            // Check if a certificate has already been issued
            var existingCertificate = await _context.Certificates
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ModuleId == moduleId);

            if (existingCertificate != null)
            {
                throw new Exception("Certificate already issued for this module.");
            }

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

            // Store the certificate in the database
            var certificate = new Certificate
            {
                UserId = userId,
                ModuleId = moduleId,
                IssueDate = DateTime.UtcNow
            };

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            // Now store the PDF bytes in the CertificateFiles table
            var certificateFile = new CertificateFile
            {
                CertificateId = certificate.CertificateId, // Link back to the certificate
                PdfBytes = pdfBytes // Store the generated PDF bytes
            };

            _context.CertificateFiles.Add(certificateFile);
            await _context.SaveChangesAsync();

            // Generate the email attachment file name dynamically
            var emailFileName = $"{module.Title} - Certificate.pdf";

            // Optionally, send the certificate email with a personalized message
            if (!string.IsNullOrEmpty(user.Email))
            {
                var emailBody = $"Dear {certificateData.FullName},<br><br>" +
                                $"Congratulations on completing the <b>{certificateData.CourseTitle}</b> module! " +
                                $"Your certificate is attached to this email.<br><br>" +
                                $"Best regards,<br>The Team.";

                await SendCertificateEmail(user.Email, "Your Certificate of Completion", pdfBytes, emailFileName, emailBody);
            }
            return pdfBytes;
        }



        // Generate certificate PDF using DinkToPdf with provided HTML
        private byte[] GenerateCertificatePdf(CertificateData data)
        {
            // Resolve image paths from wwwroot
            var pupLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "PUPLogo.png");
            var gadoLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "GADO_Logo_w-Border.png");
            var badgePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "certificate-footer.png");

            // Check if the images exist
            if (!File.Exists(pupLogoPath) || !File.Exists(gadoLogoPath) || !File.Exists(badgePath))
            {
                throw new FileNotFoundException("One or more images for the certificate were not found.");
            }

            // Convert images to Base64 for embedding in HTML
            string pupLogoBase64 = Convert.ToBase64String(File.ReadAllBytes(pupLogoPath));
            string gadoLogoBase64 = Convert.ToBase64String(File.ReadAllBytes(gadoLogoPath));
            string badgeBase64 = Convert.ToBase64String(File.ReadAllBytes(badgePath));

            // Populate the HTML content with the provided data and embedded images
            string htmlContent = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Certificate of Completion</title>
    <style>
        html {{          
            height: 100%;
            background-color: #f5f4ec;
            border: 20px solid #1a2b44;
        }}
        .certificate {{
            width: 100%;
            padding: 40px;
            background-color: #f5f4ec;
            text-align: center;
            position: relative;
            box-sizing: border-box;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }}
        .logo-container {{
            display: flex;
            justify-content: center;
            align-items: center;
            margin-top: 10px;
        }}
        .logo {{
            width: 100px;
            margin: 0 15px;
        }}
        h1 {{
            font-size: 55px;
            margin-bottom: 5px;
            font-family: 'Cursive', serif;
            color: #1a2b44;
            letter-spacing: 1px;
            font-weight: 700;
        }}
        h2 {{
            font-size: 35px;
            padding-top: 25px;
            color: #000000;
            font-family: 'Cursive', serif;
            font-weight: 100;
        }}
        .recipient {{
            font-size: 60px;
            margin: 10px 0;
            font-family: 'Cursive', serif;
            font-style: italic;
        }}
        .underline {{
            width: 60%;
            height: 2px;
            background-color: #1a2b44;
            margin: 10px auto;
        }}
        .content {{
            font-size: 25px;
            line-height: 1.4;
            margin: 10px 0;
            color: #000000;
            font-family: 'Cursive', serif;
            font-style: italic;
        }}
        .certificate-footer {{
            width: 100%;
            height: auto;
            margin-top: 40px;
            
        }}
     

    </style>
</head>
<body>
    <div class='certificate'>
        <div class='logo-container'>
            <img src='data:image/png;base64,{pupLogoBase64}' alt='PUP Logo' class='logo'>
            <img src='data:image/png;base64,{gadoLogoBase64}' alt='GADO Logo' class='logo'>
        </div>
        <h1>CERTIFICATE OF COMPLETION</h1>
        <h2>This certificate is proudly awarded to</h2>
        <div class='recipient'>{data.FullName}</div>
        <div class='underline'></div>
        <div class='content'>
            For the successful completion of the <strong>{data.CourseTitle}</strong> module.<br>
            Completed on <strong>{data.CompletionDate:MMMM dd, yyyy}</strong>.
        </div>
        
        <div class='signature-container'>
        <img src='data:image/png;base64,{badgeBase64}' alt='Badge' class='certificate-footer'>
           
        </div>
    </div>
</body>
</html>";






            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.Letter,

                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8", LoadImages = true },
                        UseLocalLinks = true
                    }
                }
            };

            return _converter.Convert(doc);
        }

        // Method to send certificate via email
        public async Task SendCertificateEmail(string email, string subject, byte[] pdfBytes, string fileName, string emailBody)
        {
            Console.WriteLine($"Email sending is temporarily disabled. Would have sent email to {email} with subject {subject}");
            if (_emailSender != null)
            {
                Console.WriteLine($"Sending email to {email} with subject {subject}");
                try
                {
                    await _emailSender.SendEmailWithAttachmentAsync(email, subject, emailBody, pdfBytes, fileName);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Email sender service is not available.");
            }
        }

        public async Task<byte[]> GetCertificateBytesAsync(string userId, int moduleId)
        {
            // Check if a certificate exists for the user and module
            var existingCertificate = await _context.Certificates
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ModuleId == moduleId);

            if (existingCertificate == null)
            {
                // If no certificate exists, return null or throw an exception
                return null; // This could be changed to throw an exception if preferred
            }

            // Retrieve the certificate bytes from the CertificateFiles table
            var certificateFile = await _context.CertificateFiles
                .FirstOrDefaultAsync(cf => cf.CertificateId == existingCertificate.CertificateId);

            return certificateFile?.PdfBytes; // Return the PDF bytes, or null if not found
        }


    }

    // CertificateData DTO for passing certificate details
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