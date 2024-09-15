// CourseContent.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class CourseContent
    {
        [Key]
        public int ContentId { get; set; }

        [Required]
        public required string ContentType { get; set; } // e.g., Video, PDF, Image, H5P

        [Required]
        public required string ContentPath { get; set; } // Path to the content file or H5P package

        [Required]
        public int ModuleId { get; set; }

        // Navigation Property
        public required Module Module { get; set; }

        // H5P Specific Properties
        [Column("h5p_id")]
        public string? H5PId { get; set; } // ID or reference for H5P content

        [Column("h5p_metadata", TypeName = "TEXT")]
        public string? H5PMetadata { get; set; } // JSON or XML metadata related to H5P content
    }
}
