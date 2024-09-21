// Models/Courses

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    [Required]
    [StringLength(255)]
    public string CourseName { get; set; }

    [Required]
    public int CollegeDeptId { get; set; }

    // Navigation property to reference CollegeDepartment
    [ForeignKey("CollegeDeptId")]
    public virtual CollegeDepartment CollegeDepartment { get; set; }
}
