using GedsiHub.ViewModels;
using System.Collections.Generic;

namespace GedsiHub.ViewModels;

public class UserManagementViewModel
{
    public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    public string SearchTerm { get; set; }
    public bool? IsActive { get; set; }

    // Pagination properties
    public int TotalUsers { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
