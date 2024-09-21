// Models/CollegeDepartment.cs

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GedsiHub.Models;

public class CollegeDepartment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CollegeDeptId { get; set; }

    [Required]
    [StringLength(255)]
    public string DepartmentName { get; set; }

    // Navigation property for related courses
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
