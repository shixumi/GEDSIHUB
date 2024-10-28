using System;
using System.Collections.Generic;

namespace GedsiHub.ViewModels
{
    public class AdminReportsViewModel
    {
        public List<ReportedPostViewModel> ReportedPosts { get; set; } = new List<ReportedPostViewModel>();
        public List<ReportedCommentViewModel> ReportedComments { get; set; } = new List<ReportedCommentViewModel>();
    }

    public class ReportedPostViewModel
    {
        public int ReportId { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string ReportedByName { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReportedCommentViewModel
    {
        public int ReportId { get; set; }
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public string ReportedByName { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; } // To link to the post details
    }
}
