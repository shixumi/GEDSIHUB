// Services/AnalyticsService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // User Engagement Metrics

        public async Task<TimeSpan> GetTotalTimeSpentAsync(string userId, int moduleId)
        {
            var totalSeconds = await _context.ModuleActivities
                .Where(ma => ma.UserId == userId && ma.ModuleId == moduleId)
                .SumAsync(ma => ma.TimeSpent.TotalSeconds);

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public async Task<int> GetNumberOfLoginsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.UserLogins.AsQueryable();

            query = query.Where(ul => ul.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(ul => ul.LoginTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ul => ul.LoginTime <= endDate.Value);

            return await query.CountAsync();
        }

        public async Task<int> GetActiveDaysAsync(string userId)
        {
            return await _context.UserLogins
                .Where(ul => ul.UserId == userId)
                .Select(ul => ul.LoginTime.Date)
                .Distinct()
                .CountAsync();
        }

        // Performance Metrics

        public async Task<double> GetAverageQuizScoreAsync(int moduleId)
        {
            var scores = await _context.UserEngagements
                .Where(ue => ue.ModuleId == moduleId && ue.QuizScore > 0)
                .Select(ue => ue.QuizScore)
                .ToListAsync();

            if (!scores.Any())
                return 0.0;

            return scores.Average();
        }

        public async Task<double> GetModuleCompletionRateAsync(int moduleId)
        {
            var totalUsers = await _context.Enrollments
                .Where(e => e.ModuleId == moduleId)
                .CountAsync();

            if (totalUsers == 0)
                return 0.0;

            var completedUsers = await _context.UserEngagements
                .Where(ue => ue.ModuleId == moduleId && ue.IsModuleCompleted)
                .CountAsync();

            return (double)completedUsers / totalUsers * 100;
        }

        public async Task<int> GetCertificateIssuanceRateAsync(int moduleId)
        {
            return await _context.Certificates
                .Where(c => c.ModuleId == moduleId)
                .CountAsync();
        }

        // User Demographics and Profiles

        public async Task<UserDemographicsDto> GetUserDemographicsAsync()
        {
            var demographics = new UserDemographicsDto
            {
                AgeDistribution = await _context.Users
                    .Where(u => u.DateOfBirth != null)
                    .Select(u => new AgeGroupDto
                    {
                        Year = u.DateOfBirth.Year,
                        Count = 1
                    })
                    .GroupBy(a => a.Year)
                    .Select(g => new AgeGroupDto
                    {
                        Year = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync(),
                GenderIdentity = await _context.Users
                    .GroupBy(u => u.GenderIdentity)
                    .Select(g => new GenderIdentityDto
                    {
                        Gender = g.Key ?? "Unspecified",
                        Count = g.Count()
                    })
                    .ToListAsync(),
                IndigenousMembership = await _context.Users
                    .GroupBy(u => u.IsMemberOfIndigenousCommunity)
                    .Select(g => new IndigenousMembershipDto
                    {
                        IsMember = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync(),
                EmploymentStatus = await _context.Employees
                    .GroupBy(e => e.EmploymentStatus)
                    .Select(g => new EmploymentStatusDto
                    {
                        Status = g.Key ?? "Unspecified",
                        Count = g.Count()
                    })
                    .ToListAsync(),
                CourseAssociations = await _context.Students
                    .GroupBy(s => s.CourseId)
                    .Select(g => new CourseAssociationDto
                    {
                        CourseId = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync()
            };

            return demographics;
        }

        // Real-Time Analytics

        public async Task<int> GetCurrentActiveUsersAsync()
        {
            return await _context.ActiveUsers.CountAsync();
        }

        public async Task<UserProgressDto> GetLiveUserProgressAsync(string userId, int moduleId)
        {
            var progress = await _context.UserProgresses
                .Where(up => up.UserId == userId && up.ModuleId == moduleId)
                .FirstOrDefaultAsync();

            if (progress == null)
                return null;

            return new UserProgressDto
            {
                ProgressPercentage = progress.ProgressPercentage,
                IsCompleted = progress.IsCompleted
            };
        }

        // Advanced Analytics

        public async Task<double> GetDropOffRateAsync(int moduleId)
        {
            // Placeholder implementation
            // Requires detailed tracking of user navigation within modules
            return 0.0;
        }

        public async Task<FeedbackAnalysisDto> GetFeedbackAnalysisAsync()
        {
            var feedbacks = await _context.UserFeedbacks.ToListAsync();

            // Simple sentiment analysis example (placeholder)
            var positive = feedbacks.Count(f => f.SatisfactionScore >= 4);
            var neutral = feedbacks.Count(f => f.SatisfactionScore == 3);
            var negative = feedbacks.Count(f => f.SatisfactionScore <= 2);

            return new FeedbackAnalysisDto
            {
                Positive = positive,
                Neutral = neutral,
                Negative = negative
            };
        }

        public async Task<UserSegmentationDto> GetUserSegmentationAsync()
        {
            // Example segmentation by Employment Status
            var segments = await _context.Employees
                .GroupBy(e => e.EmploymentStatus)
                .Select(g => new SegmentDto
                {
                    Segment = g.Key ?? "Unspecified",
                    Count = g.Count()
                })
                .ToListAsync();

            return new UserSegmentationDto
            {
                Segments = segments
            };
        }

        public async Task<CorrelationDto> GetGenderPerformanceCorrelationAsync()
        {
            // Placeholder for correlation logic
            return new CorrelationDto
            {
                CorrelationCoefficient = 0.0
            };
        }

        public async Task<UserSatisfactionDto> GetUserSatisfactionAsync(int moduleId)
        {
            var satisfaction = await _context.UserFeedbacks
                .Where(f => f.ModuleId == moduleId)
                .GroupBy(f => f.SatisfactionScore)
                .Select(g => new SatisfactionDto
                {
                    Score = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return new UserSatisfactionDto
            {
                SatisfactionScores = satisfaction
            };
        }

        // New Methods for Additional Charts

        /// <summary>
        /// Retrieves the breakdown of users based on their employment status.
        /// </summary>
        /// <returns>List of EmploymentStatusDto.</returns>
        public async Task<List<EmploymentStatusDto>> GetEmploymentStatusBreakdownAsync()
        {
            return await _context.Employees
                .GroupBy(e => e.EmploymentStatus)
                .Select(g => new EmploymentStatusDto
                {
                    Status = g.Key ?? "Unspecified",
                    Count = g.Count()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves the number of students associated with each course.
        /// </summary>
        /// <returns>List of CourseAssociationDto.</returns>
        public async Task<List<CourseAssociationDto>> GetCourseAssociationsAsync()
        {
            return await _context.Students
                .GroupBy(s => s.CourseId)
                .Select(g => new CourseAssociationDto
                {
                    CourseId = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves user satisfaction levels.
        /// </summary>
        /// <returns>List of SatisfactionDto.</returns>
        public async Task<List<SatisfactionDto>> GetUserSatisfactionLevelsAsync()
        {
            return await _context.UserFeedbacks
                .GroupBy(f => f.SatisfactionScore)
                .Select(g => new SatisfactionDto
                {
                    Score = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        // Method to get Total Learners
        public async Task<int> GetTotalLearnersAsync()
        {
            // Total Learners = Students + Employees
            var studentCount = await _context.Students.CountAsync();
            var employeeCount = await _context.Employees.CountAsync();
            return studentCount + employeeCount;
        }

        // Method to get Student Learners
        public async Task<int> GetStudentLearnersAsync()
        {
            return await _context.Students.CountAsync();
        }

        // Method to get Employee Learners
        public async Task<int> GetEmployeeLearnersAsync()
        {
            return await _context.Employees.CountAsync();
        }

        // Method to get Total Modules
        public async Task<int> GetTotalModulesAsync()
        {
            return await _context.Modules.CountAsync();
        }
    }

    // DTO Classes
    public class EmploymentStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class CourseAssociationDto
    {
        public int? CourseId { get; set; }
        public int Count { get; set; }
    }

    public class SatisfactionDto
    {
        public int Score { get; set; }
        public int Count { get; set; }
    }

    // Existing DTOs...
    public class UserDemographicsDto
    {
        public List<AgeGroupDto> AgeDistribution { get; set; }
        public List<GenderIdentityDto> GenderIdentity { get; set; }
        public List<IndigenousMembershipDto> IndigenousMembership { get; set; }
        public List<EmploymentStatusDto> EmploymentStatus { get; set; }
        public List<CourseAssociationDto> CourseAssociations { get; set; }
    }

    public class AgeGroupDto
    {
        public int Year { get; set; }
        public int Count { get; set; }
    }

    public class GenderIdentityDto
    {
        public string Gender { get; set; }
        public int Count { get; set; }
    }

    public class IndigenousMembershipDto
    {
        public bool IsMember { get; set; }
        public int Count { get; set; }
    }

    public class UserProgressDto
    {
        public decimal ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class FeedbackAnalysisDto
    {
        public int Positive { get; set; }
        public int Neutral { get; set; }
        public int Negative { get; set; }
    }

    public class UserSegmentationDto
    {
        public List<SegmentDto> Segments { get; set; }
    }

    public class SegmentDto
    {
        public string Segment { get; set; }
        public int Count { get; set; }
    }

    public class CorrelationDto
    {
        public double CorrelationCoefficient { get; set; }
    }

    public class UserSatisfactionDto
    {
        public List<SatisfactionDto> SatisfactionScores { get; set; }
    }
}
