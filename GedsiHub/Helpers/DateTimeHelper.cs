namespace GedsiHub.Helpers
{
    public static class DateTimeHelper
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return timeSpan.Seconds == 1 ? "1 second ago" : $"{timeSpan.Seconds} seconds ago";
            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes == 1 ? "1 minute ago" : $"{timeSpan.Minutes} minutes ago";
            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours == 1 ? "1 hour ago" : $"{timeSpan.Hours} hours ago";
            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days == 1 ? "1 day ago" : $"{timeSpan.Days} days ago";
            if (timeSpan <= TimeSpan.FromDays(365))
            {
                int months = timeSpan.Days / 30;
                return months == 1 ? "1 month ago" : $"{months} months ago";
            }

            int years = timeSpan.Days / 365;
            return years == 1 ? "1 year ago" : $"{years} years ago";
        }

    }
}
