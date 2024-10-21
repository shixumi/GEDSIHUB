// Models/Feedback.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models;
public class Feedback
{
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Type { get; set; } // Type of Issue or Suggestion

    [Required]
    public string FeedbackType { get; set; } // "Complaint" or "Suggestion"

    public string Description { get; set; } // Detailed Description or Suggestion

    public bool IsResolved { get; set; } // Status
}
