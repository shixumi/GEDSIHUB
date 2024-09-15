// Question.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public required string Text { get; set; }

        [Required]
        public required string QuestionType { get; set; } // e.g., Text, Multiple-Choice

        [Required]
        public int ModuleId { get; set; }

        // Navigation Property
        public required Module Module { get; set; }

        // **Added Navigation Property**
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
