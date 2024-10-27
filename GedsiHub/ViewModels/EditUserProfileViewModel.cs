// ViewModels/EditUserProfileViewModel.cs

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GedsiHub.ViewModels
{
    public class EditUserProfileViewModel
    {
        // Common Fields
        [Display(Name = "Honorifics")]
        [StringLength(10)]
        public string Honorifics { get; set; }

        [Display(Name = "Lived Name")]
        [StringLength(50)]
        public string LivedName { get; set; }

        [Display(Name = "Pronouns")]
        [StringLength(30)]
        public string Pronouns { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sex")]
        [StringLength(10)]
        public string Sex { get; set; }

        [Display(Name = "Gender Identity")]
        [StringLength(30)]
        public string GenderIdentity { get; set; }

        // Student-Specific Fields
        [Display(Name = "Program")]
        [StringLength(255)]
        public string Program { get; set; } // e.g., Course Name

        [Display(Name = "Year")]
        [Range(1, 10, ErrorMessage = "Please enter a valid year.")]
        public int? Year { get; set; }

        [Display(Name = "Section")]
        [StringLength(10)]
        public string Section { get; set; }

        // Employee-Specific Fields
        [Display(Name = "Employee Type")]
        [StringLength(100)]
        public string EmployeeType { get; set; }

        [Display(Name = "Employment Status")]
        [StringLength(100)]
        public string EmploymentStatus { get; set; }

        [Display(Name = "Branch Office/Section/Unit")]
        [StringLength(100)]
        public string BranchOfficeSectionUnit { get; set; }

        [Display(Name = "Position")]
        [StringLength(100)]
        public string Position { get; set; }

        [Display(Name = "Sector")]
        [StringLength(100)]
        public string Sector { get; set; }

        // Profile Picture Upload
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        // Additional Fields (if any)
    }
}
