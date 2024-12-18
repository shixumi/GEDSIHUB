// GedsiHub.ViewModels.DemographicReportViewModel.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GedsiHub.ViewModels
{
    public class DemographicReportViewModel
    {
        // Custom Date Range
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Please enter a start date.")]
        public DateTime? CustomStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Please enter an end date.")]
        public DateTime? CustomEndDate { get; set; }

        // Filters
        [Display(Name = "Campus")]
        [Required(ErrorMessage = "The Campus field is required.")]
        public string Campus { get; set; }

        [Display(Name = "Age Group")]
        [Required(ErrorMessage = "The Age Group field is required.")]
        public string AgeGroup { get; set; }

        [Display(Name = "Sex")]
        [Required(ErrorMessage = "The Sex field is required.")]
        public string Sex { get; set; }

        [Display(Name = "Gender Identity")]
        [Required(ErrorMessage = "The Gender Identity field is required.")]
        public string GenderIdentity { get; set; }

        [Display(Name = "Type of User")]
        [Required(ErrorMessage = "The Type of User field is required.")]
        public string UserType { get; set; }

        // Metrics to Include
        public bool IncludeIdNumber { get; set; }
        public bool IncludeName { get; set; }
        public bool IncludeWebmail { get; set; }
        public bool IncludePhoneNumber { get; set; }
        public bool IncludeDateOfBirth { get; set; }
        public bool IncludeAge { get; set; }
        public bool IncludeSex { get; set; }
        public bool IncludeGender { get; set; }
        public bool IncludeIndigenousCommunity { get; set; }
        public bool IncludeDifferentlyAbled { get; set; }

        // Report Options
        [Display(Name = "Group By")]
        public string GroupBy { get; set; }

        [Display(Name = "Sort By")]
        public string SortBy { get; set; }

        [Display(Name = "File Format")]
        [Required(ErrorMessage = "Please select a file format.")]
        public string FileFormat { get; set; }

        // Dropdown Options
        public List<SelectListItem> CampusOptions { get; set; }
        public List<SelectListItem> AgeGroupOptions { get; set; }
        public List<SelectListItem> SexOptions { get; set; }
        public List<SelectListItem> GenderIdentityOptions { get; set; }
        public List<SelectListItem> UserTypeOptions { get; set; }

        // New Dropdown Options for Group By and Sort By
        public List<SelectListItem> GroupByOptions { get; set; }
        public List<SelectListItem> SortByOptions { get; set; }

        public DemographicReportViewModel()
        {
            // Initialize lists to prevent null references
            CampusOptions = new List<SelectListItem>();
            AgeGroupOptions = new List<SelectListItem>();
            SexOptions = new List<SelectListItem>();
            GenderIdentityOptions = new List<SelectListItem>();
            UserTypeOptions = new List<SelectListItem>();
            GroupByOptions = new List<SelectListItem>();
            SortByOptions = new List<SelectListItem>();
        }
    }
}
