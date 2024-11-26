using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class UserLessonProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }

        public DateTime CompletedOn { get; set; }
    }
}
