// File: Models/CourseContent.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class CourseContent
    {
        [Key]
        public int ContentId { get; set; }

        [Required]
        public required string ContentType { get; set; } // e.g., Video, PDF, Image

        [Required]
        public required string ContentPath { get; set; } // Path to the content file

        [Required]
        public int ModuleId { get; set; }

        // Navigation Property
        public required Module Module { get; set; }
    }
}
