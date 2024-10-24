using GedsiHub.Models;
using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int AdminCount { get; set; }
        public int NonAdminCount { get; set; }

        public List<ActivityLog> RecentLogs { get; set; } // Display latest activity logs on the dashboard
    }
}
