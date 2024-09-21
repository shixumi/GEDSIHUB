// Models/FAQ.cs
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } // e.g., "Account", "Courses", "Technical Support"

        [Required]
        [StringLength(200)]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
