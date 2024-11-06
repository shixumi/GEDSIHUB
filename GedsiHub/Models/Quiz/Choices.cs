using GedsiHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GedsiHub.Models.Quiz
{
    public class Choice : BaseEntity
    {
        [Key]
        public int ChoiceID { get; set; }
        public int QuestionID { get; set; }

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }
        public string DisplayText { get; set; }
    }
}