// ViewModels/AnalyticsDashboardViewModel.cs

using GedsiHub.Models;
using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    public class AnalyticsDashboardViewModel
    {
        // Existing Properties
        public List<Module> Modules { get; set; } = new List<Module>();

        // New Metrics
        public int TotalLearners { get; set; }
        public int StudentLearners { get; set; }
        public int EmployeeLearners { get; set; }
        public int TotalModules { get; set; }

        // Additional properties for existing analytics (if any)
        // e.g., UserDemographics, FeedbackAnalysis, etc.
    }
}
