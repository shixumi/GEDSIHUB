using DinkToPdf;
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

        // Constructor with IWebHostEnvironment for image path resolution
        public CertificateService(ApplicationDbContext context, IConverter converter, EmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _converter = converter;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;  // Store the web host environment
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
            //var existingCertificate = await _context.Certificates
            //    .FirstOrDefaultAsync(c => c.UserId == userId && c.ModuleId == moduleId);

            //if (existingCertificate != null)
            //{
            //    throw new Exception("Certificate already issued for this module.");
            //}

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
                CertificateUrl = "",
                IssueDate = DateTime.UtcNow
            };

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            // Send the email after certificate generation
            if (!string.IsNullOrEmpty(user.Email))
            {
                await SendCertificateEmail(user.Email, "Your Certificate of Completion", pdfBytes, "certificate.pdf");
            }

            return pdfBytes;
        }

        // Generate certificate PDF using DinkToPdf with provided HTML
        private byte[] GenerateCertificatePdf(CertificateData data)
        {
            // Resolve image paths from wwwroot
            var pupLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "PUPLogo.png");
            var gadoLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "GADO_Logo_w-Border.png");
            var badgePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "badge.png");

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
                    @page {{
                        size: 11in 8.5in; /* Landscape size */
                        margin: 0; /* No margin for full use of the page */
                    }}
                    body {{
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        background-color: #f0f0f0;
                        font-family: 'Poppins', serif;
                    }}
                    .certificate {{
                        width: 11in;
                        height: 8.5in;
                        padding-top: 40px;
                        border: 25px solid transparent;
                        border-image: radial-gradient(circle, #001f83, #1a2b44) 1;
                        background-color: #f5f4ec;
                        text-align: center;
                        position: relative;
                        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
                        overflow: hidden;
                    }}
                    .logo-container {{
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        margin-top: 30px;
                    }}
                    .logo {{
                        width: 80px;
                        display: block;
                        margin: 0 10px;
                    }}
                    h1 {{
                        font-size: 50px;
                        margin-bottom: 0;
                        font-family: 'Cursive', serif;
                        color: #1a2b44;
                        letter-spacing: 1px;
                        font-weight: 700;
                    }}
                    h2 {{
                        font-size: 30px;
                        padding-top: 50px;
                        color: #000000;
                        font-family: 'Cursive', serif;
                        font-weight: 100;
                    }}
                    .recipient {{
                        font-size: 55px;
                        margin: 20px 0;
                        font-family: 'Cursive', serif;
                        font-style: italic;
                    }}
                    .underline {{
                        width: 70%;
                        height: 3px;
                        background-color: #1a2b44;
                        margin: 10px auto;
                    }}
                    .content {{
                        font-size: 25px;
                        line-height: 1.5;
                        margin: 20px 0;
                        color: #000000;
                        font-family: 'Cursive', serif;
                        font-style: italic;
                    }}
                    .signature {{
                        display: flex;
                        justify-content: space-between;
                        margin-top: 100px;
                        padding: 0 15%;
                        font-family: 'Cursive', sans-serif;
                        position: relative;
                    }}
                    .signature div {{
                        text-align: center;
                    }}
                    .signature div span {{
                        display: block;
                        margin-top: 10px;
                        font-weight: bold;
                    }}
                    .badge {{
                        width: 150px;
                        position: absolute;
                        left: 50%;
                        transform: translateX(-50%);
                        top: calc(75% - 30px);
                        display: block;
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
                    <div class='signature'>
                        <div>
                            <span>___________________________</span>
                            Signature
                        </div>
                        <div>
                            <span>___________________________</span>
                            Date
                        </div>
                    </div>
                    <img src='data:image/png;base64,{badgeBase64}' alt='Badge' class='badge'>
                </div>
            </body>
            </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape, // Landscape mode for certificate
                    PaperSize = PaperKind.Letter,
                    Margins = new MarginSettings { Top = 0, Bottom = 0, Left = 0, Right = 0 }
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(doc);
        }

        // Method to send certificate via email
        public async Task SendCertificateEmail(string email, string subject, byte[] pdfBytes, string fileName)
        {
            if (_emailSender != null)
            {
                Console.WriteLine($"Sending email to {email} with subject {subject}");

                try
                {
                    await _emailSender.SendEmailWithAttachmentAsync(email, subject, "Here is your certificate.", pdfBytes, fileName);
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
