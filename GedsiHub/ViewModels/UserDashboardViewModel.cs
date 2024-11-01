// ViewModels/UserDashboardViewModel.cs
using System.Collections.Generic;
using GedsiHub.Models;

namespace GedsiHub.ViewModels
{
    public class UserDashboardViewModel
    {
        public int ModulesToDoCount { get; set; } 
        public int CompletedModulesCount { get; set; }
        public int TotalModules { get; set; }
        public int CurrentStreak { get; set; }
    }
}
