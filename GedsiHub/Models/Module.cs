// File: Models/Module.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("module_tbl")]
    public class Module
    {
        [Key]
        [Column("module_id")]
        public int ModuleId { get; set; }

        [Column("module_no")]
        public int ModuleNumber { get; set; }

        [Column("module_name")]
        public string ModuleName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<UserProgress> UserProgresses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}
