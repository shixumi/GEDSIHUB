using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GedsiHub.ViewModels
{
    public class EditUserProfileViewModel
    {
        [Display(Name = "Honorifics")]
        [StringLength(10)]
        public string? Honorifics { get; set; }

        [Display(Name = "Lived Name")]
        [StringLength(50)]
        public string? LivedName { get; set; }

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

        [Display(Name = "Are you a member of any Indigenous cultural community?")]
        public bool IsMemberOfIndigenousCommunity { get; set; }

        [Display(Name = "Do you identify yourself as a differently abled person?")]
        public bool IsDisabled { get; set; }

        // Profile Picture Upload (Optional, with Validation)
        [Display(Name = "Profile Picture")]
        [MaxFileSize(2 * 1024 * 1024)] // 2 MB limit
        public IFormFile? ProfilePicture { get; set; }
        public string ProfilePicturePath { get; set; } = "/images/User.png";
    }
}

// Custom file size validation
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    public MaxFileSizeAttribute(int maxFileSize) => _maxFileSize = maxFileSize;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null && file.Length > _maxFileSize)
        {
            return new ValidationResult($"Maximum allowed file size is {(_maxFileSize / (1024 * 1024))} MB.");
        }

        return ValidationResult.Success;
    }
}
