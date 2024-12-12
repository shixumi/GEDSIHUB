// ViewModels/AnalyticsDashboardViewModel.cs

using GedsiHub.Controllers;
using GedsiHub.Models;
using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    // ViewModels/AnalyticsDashboardViewModel.cs
    public class AnalyticsDashboardViewModel
    {
        public List<Module> Modules { get; set; }
        public List<ModuleMetricsViewModel> ModuleMetrics { get; set; }
        public int TotalLearners { get; set; }
        public int StudentLearners { get; set; }
        public int EmployeeLearners { get; set; }
        public int TotalModules { get; set; }
        public Dictionary<string, string> AIInsights { get; set; } = new Dictionary<string, string>();
    }
}