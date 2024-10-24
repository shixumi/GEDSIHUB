namespace GedsiHub.ViewModels
{
    public class DeleteUserViewModel
    {
        public string Id { get; set; }      // Must match the property used in asp-for="Id"
        public string UserName { get; set; } // For display in the heading
    }
}
