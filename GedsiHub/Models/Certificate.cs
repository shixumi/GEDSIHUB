// Certificate.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("certificate_tbl")]
    public class Certificate
    {
        [Key]
        [Column("certificate_id")]
        public int CertificateId { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Module")]
        [Column("module_id")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Column("issue_date", TypeName = "TIMESTAMP")]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    }
}
