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

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("student_Lname")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("student_Fname")]
        public string FirstName { get; set; } = string.Empty;

        [Column("student_Mname")]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [Column("college_dept_id")]
        public string CollegeDeptId { get; set; }

        [Column("year_section")]
        public string YearSection { get; set; } = string.Empty;

        // Navigation property
        public virtual CollegeDepartment CollegeDepartment { get; set; }
    }
}
