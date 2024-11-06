using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models.ViewModels
{
    public class NotificationViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string TargetAudience { get; set; } // "AllUsers", "Students", "Employees", "Admins"

        public bool IsImportant { get; set; } // Indicates if the notification is important
    }
}
