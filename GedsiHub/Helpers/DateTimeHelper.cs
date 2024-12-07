namespace GedsiHub.Helpers
{
    public static class DateTimeHelper
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            var now = DateTime.UtcNow;
            var isFuture = dateTime > now;
            var timeSpan = isFuture ? dateTime - now : now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return isFuture
                    ? "in a few seconds"
                    : timeSpan.Seconds == 1 ? "1 second ago" : $"{timeSpan.Seconds} seconds ago";
            if (timeSpan <= TimeSpan.FromMinutes(60))
                return isFuture
                    ? $"in {timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s" : "")}"
                    : timeSpan.Minutes == 1 ? "1 minute ago" : $"{timeSpan.Minutes} minutes ago";
            if (timeSpan <= TimeSpan.FromHours(24))
                return isFuture
                    ? $"in {timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s" : "")}"
                    : timeSpan.Hours == 1 ? "1 hour ago" : $"{timeSpan.Hours} hours ago";
            if (timeSpan <= TimeSpan.FromDays(30))
                return isFuture
                    ? $"in {timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}"
                    : timeSpan.Days == 1 ? "1 day ago" : $"{timeSpan.Days} days ago";
            if (timeSpan <= TimeSpan.FromDays(365))
            {
                int months = timeSpan.Days / 30;
                return isFuture
                    ? $"in {months} month{(months > 1 ? "s" : "")}"
                    : months == 1 ? "1 month ago" : $"{months} months ago";
            }

            int years = timeSpan.Days / 365;
            return isFuture
                ? $"in {years} year{(years > 1 ? "s" : "")}"
                : years == 1 ? "1 year ago" : $"{years} years ago";
        }
    }
}
