using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class LessonContent
    {
        [Key]
        public int ContentId { get; set; }

        // Properties for Text Content
        public string? TextContent { get; set; } // Stores HTML-formatted text

        // Properties for Image Content
        public string? ImageUrl { get; set; } // URL of the image

        // Properties for H5P Content
        [DataType(DataType.Html)]
        public string? H5PEmbedCode { get; set; } // Stores the iframe embed code

        [Range(0, int.MaxValue, ErrorMessage = "Position must be a non-negative number.")]
        public int PositionInt { get; set; }

        // Foreign Key to Lesson
        [ForeignKey("Lesson")]
        [Required]
        public int LessonId { get; set; }

        // Navigation Property
        public virtual Lesson? Lesson { get; set; }
    }
}
