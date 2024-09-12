// File: Models/Quiz.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("quiz_tbl")]
    public class Quiz
    {
        [Key]
        [Column("quiz_id")]
        public int QuizId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("instructions")]
        public string Instructions { get; set; }

        [ForeignKey("Module")]
        [Column("module_id")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Question> Questions { get; set; }
    }
}
