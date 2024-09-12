// File: Models/Question.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("question_tbl")]
    public class Question
    {
        [Key]
        [Column("question_id")]
        public int QuestionId { get; set; }

        [Column("question_name")]
        public string QuestionName { get; set; }

        [Column("question_type")]
        public string QuestionType { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("points")]
        public int Points { get; set; }

        [ForeignKey("Quiz")]
        [Column("quiz_id")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
