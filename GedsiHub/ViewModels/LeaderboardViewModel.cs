// ViewModels/LeaderboardViewModel.cs
using GedsiHub.Models;

namespace GedsiHub.ViewModels
{
    public class LeaderboardViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public double? TotalTimeSpent { get; set; } // Nullable to handle cases where time is not tracked
        public double TotalScore { get; set; }
    }

    // New ViewModel to handle different leaderboard scopes
    public class LeaderboardPageViewModel
    {
        public string SelectedModuleId { get; set; } // "Overall" or ModuleId as string
        public string SelectedModuleName { get; set; } // "Overall" or Module Title
        public List<LeaderboardViewModel> LeaderboardEntries { get; set; }
        public List<Module> PublishedModules { get; set; } // For module selection dropdown
    }
}
