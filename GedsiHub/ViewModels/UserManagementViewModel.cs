using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    public class UserManagementViewModel
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public string SearchTerm { get; set; }
        public bool? IsActive { get; set; }  // Filter for active/inactive users
    }
}
