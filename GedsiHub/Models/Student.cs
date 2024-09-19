// Student.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("student_tbl")] // Table for Student records
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Column("college", TypeName = "VARCHAR(100)")]
        public string? College { get; set; } // Choice field for College (CollegeChoices)

        [Column("college_dept_id", TypeName = "VARCHAR(30)")]
        public string? CollegeDeptId { get; set; }

        [Column("program", TypeName = "VARCHAR(100)")]
        public string? Program { get; set; } // Program or course

        [Column("year")]
        public int? Year { get; set; } // Year of study

        [Column("section", TypeName = "VARCHAR(10)")]
        public string? Section { get; set; } // Section


        // Navigation property for college department
        public virtual CollegeDepartment CollegeDepartment { get; set; }
    }
}
