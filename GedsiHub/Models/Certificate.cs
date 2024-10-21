using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        public string CertificateUrl { get; set; } = string.Empty; // URL for the generated certificate (optional if storing PDF in filesystem or DB)

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow; // Issue date of the certificate

        // Foreign key to the ApplicationUser
        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        // Foreign key to the Module
        [Required]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }
    }
}
