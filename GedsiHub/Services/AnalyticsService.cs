﻿// Services/AnalyticsService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models; // Ensure this using directive is present
using Microsoft.EntityFrameworkCore;
using GedsiHub.ViewModels;
using GedsiHub.Models.Quiz;

namespace GedsiHub.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AnalyticsService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _lrsEndpoint;
        private readonly string _lrsUsername;
        private readonly string _lrsPassword;

        public AnalyticsService(ApplicationDbContext context, ILogger<AnalyticsService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;

            _lrsEndpoint = configuration["LRS:Endpoint"]; // e.g., https://your-lrs-endpoint.com/xapi/statements
            _lrsUsername = configuration["LRS:Username"];
            _lrsPassword = configuration["LRS:Password"];

            _httpClient = new HttpClient();
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_lrsUsername}:{_lrsPassword}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        // Leaderboard

        // Single Method: Get Leaderboard
        public async Task<List<LeaderboardViewModel>> GetLeaderboardAsync(string scope, int? moduleId = null)
        {
            // Step 1: Fetch ExamIDs if filtering by module
            List<int> examIds = new List<int>();
            if (scope == "Module" && moduleId.HasValue)
            {
                examIds = await _context.Exams
                    .Where(e => e.ModuleId == moduleId.Value)
                    .Select(e => e.ExamID)
                    .ToListAsync();

                if (!examIds.Any()) // Return empty if no exams for this module
                    return new List<LeaderboardViewModel>();
            }

            // Step 2: Calculate each user’s percentage score based on correct answers
            var scoresQuery = _context.QuizResults
                .Where(qr => !examIds.Any() || examIds.Contains(qr.ExamID))
                .GroupBy(qr => qr.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    CorrectAnswers = g.Count(qr => qr.IsCorrect),
                    TotalQuestions = g.Count()  // Total attempts made
                })
                .Select(score => new
                {
                    score.UserId,
                    TotalScore = score.TotalQuestions > 0 ? Math.Round(((double)score.CorrectAnswers / score.TotalQuestions) * 100, 2) : 0 // Round to 2 decimals
                });

            // Step 3: Join with the Users table to get usernames and construct the LeaderboardViewModel
            var leaderboardEntries = await scoresQuery
                .Join(_context.Users,
                      score => score.UserId,
                      user => user.Id,
                      (score, user) => new LeaderboardViewModel
                      {
                          UserId = score.UserId,
                          UserName = user.UserName,
                          TotalScore = score.TotalScore // Score as a percentage (already rounded)
                      })
                .OrderByDescending(l => l.TotalScore)
                .Take(10) // Top 10 leaderboard entries
                .ToListAsync();

            return leaderboardEntries;
        }


        // Helper Method to Extract User ID from mbox
        private string ExtractUserIdFromMbox(string mbox)
        {
            // Assuming mbox is in the format 'mailto:user@example.com'
            if (mbox.StartsWith("mailto:"))
            {
                return mbox.Substring(7).ToLower();
            }
            return mbox.ToLower();
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
                        CourseName = g.FirstOrDefault().Course.CourseName,
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
                ProgressPercentage = (double)progress.ProgressPercentage, // Explicit conversion
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

        // New Method: Get Completion Rate per Module
        public async Task<double> GetModuleCompletionRateAsync(int moduleId)
        {
            var totalUsers = await _context.UserProgresses
                .Where(up => up.ModuleId == moduleId)
                .Select(up => up.UserId)
                .Distinct()
                .CountAsync();

            if (totalUsers == 0)
                return 0.0;

            var completedUsers = await _context.UserProgresses
                .Where(up => up.ModuleId == moduleId && up.IsCompleted)
                .Select(up => up.UserId)
                .Distinct()
                .CountAsync();

            return ((double)completedUsers / totalUsers) * 100;
        }

        // New Method: Get Number of Certificates Issued per Module
        public async Task<int> GetCertificateIssuanceRateAsync(int moduleId)
        {
            return await _context.Certificates
                .Where(c => c.ModuleId == moduleId)
                .CountAsync();
        }

        // New Method: Get Average Quiz Score per Module
        public async Task<double> GetAverageQuizScoreAsync(int moduleId)
        {
            _logger.LogInformation("Fetching exams for Module ID: {ModuleId}", moduleId);

            var examIds = await _context.Exams
                .Where(e => e.ModuleId == moduleId)
                .Select(e => e.ExamID)
                .ToListAsync();

            _logger.LogInformation("Found {ExamCount} exams for Module ID: {ModuleId}", examIds.Count, moduleId);

            if (!examIds.Any())
                return 0.0;

            var quizResults = await _context.QuizResults
                .Where(qr => examIds.Contains(qr.ExamID))
                .ToListAsync();

            _logger.LogInformation("Found {QuizResultCount} quiz results for Module ID: {ModuleId}", quizResults.Count, moduleId);

            if (!quizResults.Any())
                return 0.0;

            // Calculate the average score per user
            var userScores = quizResults
                .GroupBy(qr => qr.UserId)
                .Select(g =>
                {
                    var totalQuestions = g.Count();
                    var correctAnswers = g.Count(qr => qr.IsCorrect);
                    return (double)correctAnswers / totalQuestions * 100;
                })
                .ToList();

            var averageScore = userScores.Any() ? userScores.Average() : 0.0;

            _logger.LogInformation("Average quiz score for Module ID: {ModuleId} is {AverageScore}", moduleId, averageScore);

            return averageScore;
        }

        // New Method: Get the Count of Modules to Do for a User
        public async Task<int> GetModulesToDoCountAsync(string userId)
        {
            var completedModuleIds = await _context.UserActivities
                .Where(ua => ua.UserId == userId && ua.ActivityType.Contains("completed") && ua.Success == true)
                .Select(ua => ua.ModuleId)
                .Distinct()
                .ToListAsync();

            var modulesToDoCount = await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published && !completedModuleIds.Contains(m.ModuleId))
                .CountAsync();

            return modulesToDoCount;
        }

        // User Dashboard
        public async Task<UserDashboardViewModel> GetUserDashboardAsync(string userId)
        {
            var completedModules = await _context.UserProgresses
                .Where(up => up.UserId == userId && up.IsCompleted)
                .Select(up => up.ModuleId)
                .Distinct()
                .ToListAsync();

            var totalModules = await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published)
                .CountAsync();

            var modulesToDoCount = totalModules - completedModules.Count;

            var streak = await _context.UserProgresses
                .Where(up => up.UserId == userId)
                .SumAsync(up => up.StreakCount);

            return new UserDashboardViewModel
            {
                ModulesToDoCount = modulesToDoCount,
                CompletedModulesCount = completedModules.Count,
                TotalModules = totalModules,
                CurrentStreak = streak
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
                    CourseName = g.FirstOrDefault().Course.CourseName, // Fetch CourseName
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

        // New Method: Get Published Modules
        public async Task<List<Module>> GetPublishedModulesAsync()
        {
            return await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published)
                .OrderBy(m => m.PositionInt)
                .ToListAsync();
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
        public string CourseName { get; set; }
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
        public double ProgressPercentage { get; set; } // Ensured double type
        public bool IsCompleted { get; set; }
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
}
