using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }       // User ID

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }    // Email address
        public bool IsAdmin { get; set; }    // Admin status
        public bool IsActive { get; set; }   // Active status

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } // First Name of the user

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }  // Last Name of the user
    }
}
