using System;

namespace GedsiHub.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string AdminUser { get; set; } // The admin who performed the action
        public string Action { get; set; }    // The action taken (e.g., user edited, user deleted)
        public DateTime Timestamp { get; set; } // When the action occurred
    }
}
