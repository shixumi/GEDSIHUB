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

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("emp_type", TypeName = "VARCHAR(20)")]
        public string EmployeeType { get; set; } = string.Empty;

        [Required]
        [Column("sector", TypeName = "VARCHAR(100)")]
        public string Sector { get; set; } = string.Empty;

        [Required]
        [Column("emp_loc", TypeName = "VARCHAR(100)")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Column("position", TypeName = "VARCHAR(100)")]
        public string Position { get; set; } = string.Empty;
    }
}
