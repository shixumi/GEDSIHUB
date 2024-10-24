using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    public class BulkDeleteViewModel
    {
        public List<string> UserIds { get; set; } = new List<string>(); // IDs of users to delete
    }
}
