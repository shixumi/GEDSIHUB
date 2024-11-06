using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    public class NotificationUser
    {
        public int Id { get; set; }

        [Required]
        public int NotificationId { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsRead { get; set; }

        // Navigation properties
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
