// ViewModels/CertificateDto.cs
namespace GedsiHub.ViewModels
{
    public class CertificateDto
    {
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Status { get; set; } // e.g., "Completed", "In Progress"
        public string CertificateImagePath { get; set; } // Path to the certificate image
    }
}
