// File: Models/Student.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("student_tbl")]
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column("student_Lname")]
        public string LastName { get; set; }

        [Column("student_Fname")]
        public string FirstName { get; set; }

        [Column("student_Mname")]
        public string MiddleName { get; set; }

        [Column("college_dept_id")]
        public string CollegeDeptId { get; set; }

        [Column("year_section")]
        public string YearSection { get; set; }

        // Navigation property
        public virtual CollegeDepartment CollegeDepartment { get; set; }
    }
}
