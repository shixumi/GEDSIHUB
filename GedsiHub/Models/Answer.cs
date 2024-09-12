// File: Models/Answer.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("answer_tbl")]
    public class Answer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Question")]
        [Column("question_id")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("is_correct")]
        public bool IsCorrect { get; set; }
    }
}
