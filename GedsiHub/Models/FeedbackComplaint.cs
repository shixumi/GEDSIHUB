// Models/FeedbackComplaint.cs
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models;
public class FeedbackComplaint
{
    [Required]
    [Display(Name = "Type of Issue")]
    public string TypeOfIssue { get; set; }

    [Required]
    [Display(Name = "Affected Area")]
    public string AffectedArea { get; set; }

    [Required]
    [Display(Name = "Detailed Description")]
    public string DetailedDescription { get; set; }

    public IFormFile Evidence { get; set; } // Optional file upload
}