// Models/SuggestionFeedback.cs
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models;
public class FeedbackSuggestion
{
    [Required]
    [Display(Name = "Type of Suggestion")]
    public string TypeOfSuggestion { get; set; }

    [Required]
    [Display(Name = "Description of Suggestion")]
    public string DescriptionOfSuggestion { get; set; }

    public IFormFile Attachment { get; set; } // Optional file upload
}