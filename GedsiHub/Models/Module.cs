using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>(); // Added Lessons
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
