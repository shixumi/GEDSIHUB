namespace GedsiHub.Helpers
{
    public static class NotificationHelper
    {
        public static string GetIconForCategory(string category)
        {
            return category switch
            {
                "General" => "fas fa-info-circle",
                "Event" => "fas fa-calendar-alt",
                "Update" => "fas fa-sync-alt",
                "Alert" => "fas fa-exclamation-triangle",
                _ => "fas fa-bell",
            };
        }
    }
}
