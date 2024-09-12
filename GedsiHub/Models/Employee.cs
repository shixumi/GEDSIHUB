// File: Models/Employee.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("employee_tbl")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column("emp_type")]
        public string EmployeeType { get; set; }

        [Column("sector")]
        public string Sector { get; set; }

        [Column("emp_loc")]
        public string Location { get; set; }

        [Column("position")]
        public string Position { get; set; }
    }
}
