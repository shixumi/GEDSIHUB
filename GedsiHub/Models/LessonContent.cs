using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class LessonContent
    {
        [Key]
        public int ContentId { get; set; }

        [Required]
        public string ContentType { get; set; } = string.Empty; // e.g., Video, Text, PDF, H5P

        [Required]
        public string ContentPath { get; set; } = string.Empty; // Path to media or H5P

        public int LessonId { get; set; } // Foreign Key to Lesson

        // Navigation Property
        public virtual Lesson? Lesson { get; set; } = null!;

        // Optional: H5P Specific Properties
        public string? H5PId { get; set; } // ID or reference for H5P content

        [Column(TypeName = "TEXT")]
        public string? H5PMetadata { get; set; } // JSON or XML metadata related to H5P content
    }
}
