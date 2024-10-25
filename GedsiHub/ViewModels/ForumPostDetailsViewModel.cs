// ForumPostDetailsViewModel.cs
using GedsiHub.Models;

namespace GedsiHub.ViewModels
{
    public class ForumPostDetailsViewModel
    {
        public ForumPost ForumPost { get; set; }  // The forum post
        public ForumCommentViewModel CommentViewModel { get; set; }  // ViewModel for the comment form
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Flair { get; set; }
    }
}

