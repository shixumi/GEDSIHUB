using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GedsiHub.ViewModels
{
    public class DemographicReportViewModel : IValidatableObject
    {
        // Filters
        [DataType(DataType.Date)]
        [Display(Name = "Custom Start Date")]
        [Required(ErrorMessage = "Please enter a start date.")]
        public DateTime? CustomStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Custom End Date")]
        [Required(ErrorMessage = "Please enter an end date.")]
        public DateTime? CustomEndDate { get; set; }

        [Display(Name = "Campus")]
        public string Campus { get; set; }

        [Display(Name = "Age Group")]
        public string AgeGroup { get; set; }

        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Gender Identity")]
        public string GenderIdentity { get; set; }

        [Display(Name = "Type of User")]
        public string UserType { get; set; } // "student" or "employee"

        // Include in Report
        [Display(Name = "Include ID Number")]
        public bool IncludeIdNumber { get; set; }

        [Display(Name = "Include Name")]
        public bool IncludeName { get; set; }

        [Display(Name = "Include Webmail")]
        public bool IncludeWebmail { get; set; }

        [Display(Name = "Include Phone Number")]
        public bool IncludePhoneNumber { get; set; }

        [Display(Name = "Include Date of Birth")]
        public bool IncludeDateOfBirth { get; set; }

        [Display(Name = "Include Age")]
        public bool IncludeAge { get; set; }

        [Display(Name = "Include Sex")]
        public bool IncludeSex { get; set; }

        [Display(Name = "Include Gender Identity")]
        public bool IncludeGender { get; set; }

        [Display(Name = "Include Indigenous Community")]
        public bool IncludeIndigenousCommunity { get; set; } = false;

        [Display(Name = "Include Differently Abled")]
        public bool IncludeDifferentlyAbled { get; set; } = false;

        // Report Options
        [Required(ErrorMessage = "Please select a file format.")]
        [Display(Name = "File Format")]
        public string FileFormat { get; set; }

        [Display(Name = "Group By")]
        public string GroupBy { get; set; } = "None"; // Default value

        [Display(Name = "Sort By")]
        public string SortBy { get; set; } = "Name"; // Default value

        // Dropdown Lists
        public List<SelectListItem> DateRangeOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CampusOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AgeGroupOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SexOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> GenderIdentityOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UserTypeOptions { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public byte[] ReportFile { get; set; } = Array.Empty<byte>();

        [ValidateNever]
        public string ReportFileName { get; set; } = string.Empty;

        // Custom Validation Logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomStartDate.HasValue && CustomEndDate.HasValue)
            {
                if (CustomStartDate.Value > CustomEndDate.Value)
                {
                    yield return new ValidationResult(
                        "Start date cannot be later than the end date.",
                        new[] { nameof(CustomStartDate), nameof(CustomEndDate) });
                }

                if (CustomEndDate.Value > DateTime.UtcNow)
                {
                    yield return new ValidationResult(
                        "End date cannot be in the future.",
                        new[] { nameof(CustomEndDate) });
                }
            }
        }
    }
}
