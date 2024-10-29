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

        // Generate certificate PDF using DinkToPdf
        private byte[] GenerateCertificatePdf(CertificateData data)
        {
            // Get the image path from wwwroot
            var base64Image = GetBase64StringForImage("images/certificate_bg.png");

            var htmlContent = GenerateCertificateHtml(data, base64Image);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape, // Landscape mode for certificate
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(doc);
        }

        // Convert image to Base64 for embedding in HTML
        private string GetBase64StringForImage(string imgRelativePath)
        {
            // Resolve the physical path to the image in the wwwroot folder
            var imgFullPath = Path.Combine(_webHostEnvironment.WebRootPath, imgRelativePath);

            // Check if the file exists
            if (!File.Exists(imgFullPath))
            {
                throw new FileNotFoundException("The certificate background image was not found.", imgFullPath);
            }

            // Read the image as byte array and convert to Base64
            byte[] imageArray = System.IO.File.ReadAllBytes(imgFullPath);
            return Convert.ToBase64String(imageArray);
        }

        // HTML template for certificate

        private string GenerateCertificateHtml(CertificateData data, string base64Image)
        {
            return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    text-align: center;
                    position: relative;
                    height: 100%;
                    width: 100%;
                    background-image: url('data:image/png;base64,{base64Image}');
                    background-size: cover;
                    background-repeat: no-repeat;
                    background-position: center;
                }}
                .certificate-container {{
                    position: absolute;
                    top: 60%;  /* Adjusted top to move the text lower */
                    left: 20%; /* Center horizontally */
                    transform: translateX(-50%); /* Only translate horizontally */
                    width: 70%; /* Adjust width */
                    padding: 20px;
                    /* Transparent background */
                }}
                h1 {{
                    font-size: 35px;
                    color: #800080; /* Purple color for header */
                    margin: 0;
                }}
                .content {{
                    font-size: 20px;
                    color: #800080; /* Purple for content text */
                }}
                .content .name {{
                    font-size: 28px;
                    color: #800080;
                    font-weight: bold;
                    margin: 10px 0;
                }}
                .content .course {{
                    font-size: 22px;
                    font-weight: bold;
                    margin-bottom: 5px;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 18px;
                    color: #800080; /* Purple for footer text */
                    text-align: left;
                    display: flex;
                    justify-content: space-between;
                }}
                .footer .signature {{
                    padding-left: 40px;
                }}
                .footer .date {{
                    padding-right: 40px;
                }}
            </style>
        </head>
        <body>
            <div class='certificate-container'>
                <h1>Certificate of Completion</h1>
                <div class='content'>
                    <p>This certifies that</p>
                    <p class='name'>{data.FullName}</p>
                    <p>has successfully completed the module</p>
                    <p class='course'>{data.CourseTitle}</p>
                    <p>on {data.CompletionDate.ToString("MMMM dd, yyyy")}</p>
                </div>
                <div class='footer'>
                    <div class='signature'>
                        <p>Signature: _______________</p>
                    </div>
                    <div class='date'>
                        <p>Date: _______________</p>
                    </div>
                </div>
            </div>
        </body>
        </html>";
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
