using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace GedsiHub.ViewModels
{
    public class FeedbackAdminViewModel
    {
        public List<Feedback> Complaints { get; set; }
        public List<Feedback> Suggestions { get; set; }
    }
}
