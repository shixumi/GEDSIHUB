// GedsiHub.ViewModels.CombinedReportViewModel.cs
namespace GedsiHub.ViewModels
{
    public class CombinedReportViewModel
    {
        public DemographicReportViewModel DemographicReport { get; set; }
        public ModuleReportViewModel ModuleReport { get; set; }

        public CombinedReportViewModel()
        {
            DemographicReport = new DemographicReportViewModel();
            ModuleReport = new ModuleReportViewModel();
        }
    }
}
