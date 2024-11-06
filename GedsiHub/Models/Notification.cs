using System;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } // Notification title

        [Required]
        public string Message { get; set; } // Detailed message

        public DateTime CreatedAt { get; set; }

        [Required]
        public string Category { get; set; } // Category of the notification

        public bool IsImportant { get; set; } // Indicates if the notification is important

        // IconClass is determined automatically based on Category
        public string IconClass { get; set; } // e.g., "fas fa-info-circle"

        // Specifies the target audience
        [Required]
        public string TargetAudience { get; set; } // "AllUsers", "Students", "Employees", "Admins"
    }
}
