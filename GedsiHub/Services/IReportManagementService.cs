using GedsiHub.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReportManagementService
{
    // Fetching reports
    Task<List<ReportedPostViewModel>> GetPostReportsAsync();
    Task<List<ReportedCommentViewModel>> GetCommentReportsAsync();

    // Dismissing reports
    Task DismissPostReportAsync(int reportId);
    Task DismissCommentReportAsync(int reportId);

    // Deleting reported content
    Task DeleteReportedPostAsync(int postId);
    Task DeleteReportedCommentAsync(int commentId);
}
