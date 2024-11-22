using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportManagementService : IReportManagementService
{
    private readonly ApplicationDbContext _context;

    public ReportManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Fetch all post reports
    public async Task<List<ReportedPostViewModel>> GetPostReportsAsync()
    {
        return await _context.ForumPostReports
            .Include(r => r.ForumPost)
            .Include(r => r.User)
            .Select(r => new ReportedPostViewModel
            {
                ReportId = r.ReportId,
                PostId = r.PostId,
                PostTitle = r.ForumPost.Title,
                ReportedByName = $"{r.User.FirstName} {r.User.LastName}",
                Reason = r.Reason,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    // Fetch all comment reports
    public async Task<List<ReportedCommentViewModel>> GetCommentReportsAsync()
    {
        return await _context.ForumCommentReports
            .Include(r => r.ForumComment)
            .Include(r => r.User)
            .Select(r => new ReportedCommentViewModel
            {
                ReportId = r.ReportId,
                CommentId = r.CommentId,
                CommentContent = r.ForumComment.Content,
                ReportedByName = $"{r.User.FirstName} {r.User.LastName}",
                Reason = r.Reason,
                CreatedAt = r.CreatedAt,
                PostId = r.ForumComment.PostId
            })
            .ToListAsync();
    }

    // Dismiss a specific post report
    public async Task DismissPostReportAsync(int reportId)
    {
        var report = await _context.ForumPostReports.FindAsync(reportId);
        if (report != null)
        {
            _context.ForumPostReports.Remove(report);
            await _context.SaveChangesAsync();
        }
    }

    // Dismiss a specific comment report
    public async Task DismissCommentReportAsync(int reportId)
    {
        var report = await _context.ForumCommentReports.FindAsync(reportId);
        if (report != null)
        {
            _context.ForumCommentReports.Remove(report);
            await _context.SaveChangesAsync();
        }
    }

    // Delete a reported post and associated reports
    public async Task DeleteReportedPostAsync(int postId)
    {
        var post = await _context.ForumPosts
            .Include(p => p.PostReports) // Use the correct navigation property
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post != null)
        {
            _context.ForumPostReports.RemoveRange(post.PostReports); // Use the correct navigation property
            _context.ForumPosts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    // Delete a reported comment and associated reports
    public async Task DeleteReportedCommentAsync(int commentId)
    {
        var comment = await _context.ForumComments
            .Include(c => c.CommentReports) // Use the correct navigation property
            .FirstOrDefaultAsync(c => c.CommentId == commentId);

        if (comment != null)
        {
            _context.ForumCommentReports.RemoveRange(comment.CommentReports); // Use the correct navigation property
            _context.ForumComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
