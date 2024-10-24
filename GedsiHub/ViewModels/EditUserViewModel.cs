namespace GedsiHub.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }      // Must match the property used in asp-for="Id"
        public string UserName { get; set; } // Must match the property used in asp-for="UserName"
        public string Email { get; set; }    // Must match the property used in asp-for="Email"
        public bool IsAdmin { get; set; }    // Must match the property used in asp-for="IsAdmin"
        public bool IsActive { get; set; }   // Must match the property used in asp-for="IsActive"
    }
}
