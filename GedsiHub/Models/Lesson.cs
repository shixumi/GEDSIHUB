using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Overview { get; set; } = string.Empty;

        public int LessonNumber { get; set; }

        [Required]
        public int ModuleId { get; set; } // Foreign Key to Module

        // Navigation Property
        public virtual Module? Module { get; set; } = null!;

        // Collection of LessonContent
        public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
    }
}
