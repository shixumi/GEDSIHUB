// File: Models/Quiz.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int ModuleId { get; set; }

        // Navigation Properties
        public required Module Module { get; set; }
        public required ICollection<Question> Questions { get; set; } = new List<Question>(); // Initialized to prevent null reference
    }
}
