using Microsoft.AspNetCore.Mvc;
using System;

namespace GedsiHub.ViewModels
{
    public class FeedbackDetailsViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string FeedbackType { get; set; }
        public string Description { get; set; }
        public bool IsResolved { get; set; }
        public string AffectedArea { get; set; } // New property for the affected area
        public string EvidencePath { get; set; } // New property for the evidence path
    }
}
