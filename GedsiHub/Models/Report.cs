// File: Models/Report.cs
namespace GedsiHub.Models
{
    public class Report
    {
        public int Id { get; set; }
        public required string ReportType { get; set; } // e.g., CSV, PDF
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public required string Data { get; set; } // JSON string or any format
    }
}
