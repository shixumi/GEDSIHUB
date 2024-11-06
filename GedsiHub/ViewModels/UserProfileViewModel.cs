// ViewModels/UserProfileViewModel.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class UserProfileViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        // Common Fields
        [Display(Name = "User Type")]
        public string UserType { get; set; } // "Student" or "Employee"

        [Display(Name = "Honorifics")]
        public string? Honorifics { get; set; }

        [Display(Name = "Lived Name")]
        public string? LivedName { get; set; }

        [Display(Name = "Pronouns")]
        public string Pronouns { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Gender Identity")]
        public string GenderIdentity { get; set; }

        // Student-Specific Fields
        [Display(Name = "Program")]
        public string Program { get; set; } // e.g., Course Name

        [Display(Name = "Year")]
        public int? Year { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }

        // Employee-Specific Fields
        [Display(Name = "Employee Type")]
        public string EmployeeType { get; set; }

        [Display(Name = "Employment Status")]
        public string EmploymentStatus { get; set; }

        [Display(Name = "Branch Office/Section/Unit")]
        public string BranchOfficeSectionUnit { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        // Profile Picture
        [Display(Name = "Profile Picture")]
        public string ProfilePicturePath { get; set; } = "/images/User.png";
        public string College { get; set; }
        public bool IsMemberOfIndigenousCommunity { get; set; }
        public bool IsDisabled { get; set; }

        [Display(Name = "Date Joined")]
        public DateTime CreatedDate { get; set; }

        // Certificates
        public List<CertificateDto> Certificates { get; set; } = new List<CertificateDto>();

        // Recent Activities
        public List<RecentPostDto> RecentPosts { get; set; } = new List<RecentPostDto>();

        // Additional Fields (if any)
    }
}
