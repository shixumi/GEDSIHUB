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

        // H5P Specific Properties
        [Required]
        public string H5PId { get; set; } // Unique identifier for the H5P content

        public string H5PMetadata { get; set; } // JSON or XML metadata for H5P

        // Foreign Key to Module
        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        public Module Module { get; set; } // Navigation property

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
