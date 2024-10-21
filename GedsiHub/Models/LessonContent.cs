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
        public ContentTypeEnum ContentType { get; set; } // Enum to specify content type

        // Properties for Text Content
        public string? TextContent { get; set; } // Stores HTML-formatted text

        // Properties for Image Content
        public string? ImageUrl { get; set; } // URL of the image

        // Properties for H5P Content
        [DataType(DataType.Html)]
        public string? H5PEmbedCode { get; set; } // Stores the iframe embed code

        public int PositionInt { get; set; } // Determines the order of content within the lesson

        // Foreign Key to Lesson
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }

        // Navigation Property
        public virtual Lesson Lesson { get; set; } = null!;
    }

    public enum ContentTypeEnum
    {
        Text = 1,
        Image = 2,
        H5P = 3
    }
}
