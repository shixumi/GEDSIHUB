using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class Assessment
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Html)]
        [Display(Name = "H5P Embed Code")]
        public string H5PEmbedCode { get; set; }

        // Foreign Key to Module
        public int ModuleId { get; set; }

        // Navigation Property (Remove [Required] if present)
        public Module Module { get; set; }

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
