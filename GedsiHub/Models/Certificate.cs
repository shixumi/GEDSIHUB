// File: Models/Certificate.cs
namespace GedsiHub.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }
        public int ModuleId { get; set; }
        public required Module Module { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
    }
}
