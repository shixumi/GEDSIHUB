// ViewModels/RecentPostDto.cs

using System;

namespace GedsiHub.ViewModels
{
    public class RecentPostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ContentSnippet { get; set; } = string.Empty; // Shortened content
        public string ImagePath { get; set; } = string.Empty;
        public string Flair { get; set; } = "No Flair";
        public string RelativeCreatedAt { get; set; } = string.Empty; // e.g., "2 hours ago"
    }
}
