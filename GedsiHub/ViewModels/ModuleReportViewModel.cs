// GedsiHub.ViewModels.ModuleReportViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GedsiHub.ViewModels
{
    public class ModuleReportViewModel
    {
        [Display(Name = "Select Module")]
        [Required(ErrorMessage = "Please select a module.")]
        public int? SelectedModuleId { get; set; }

        // Metrics to Include
        public bool IncludeCompletionRate { get; set; }
        public bool IncludeAverageQuizScore { get; set; }
        public bool IncludeCertificatesIssued { get; set; }

        // Report Options
        [Display(Name = "File Format")]
        [Required(ErrorMessage = "Please select a file format.")]
        public string FileFormat { get; set; }

        // Dropdown Options
        [BindNever]
        public List<SelectListItem> ModuleOptions { get; set; }
    }
}
