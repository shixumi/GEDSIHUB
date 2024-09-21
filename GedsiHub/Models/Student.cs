// Models/Student.cs

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

        [ForeignKey("CollegeDepartment")] // Foreign key for CollegeDepartment
        [Column("college_dept_id")]
        public int? CollegeDeptId { get; set; } // Foreign key for College Department
        public virtual CollegeDepartment CollegeDepartment { get; set; } // Navigation property

        [ForeignKey("Course")] // Foreign key for Course
        [Column("course_id")]
        public int? CourseId { get; set; } // Foreign key for the Course
        public virtual Course Course { get; set; } // Navigation property

        [Column("year")]
        public int? Year { get; set; } // Year of study

        [Column("section", TypeName = "VARCHAR(10)")]
        public string? Section { get; set; } // Section
    }
}
