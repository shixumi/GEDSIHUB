// Controllers/AnalyticsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GedsiHub.Models;
using GedsiHub.Data;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GedsiHub.Services;
using GedsiHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]

    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(ApplicationDbContext context, AnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        // ****************************** DASHBOARD VIEW ******************************

        // GET: Analytics/Dashboard
        // Displays the dashboard view with various metrics such as total learners and modules.
        // Controllers/AnalyticsController.cs
        [HttpGet("")]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(int? moduleId)
        {
            // Fetch basic data for the dashboard
            var viewModel = new AnalyticsDashboardViewModel
            {
                Modules = await _context.Modules.ToListAsync(),
                TotalLearners = await _analyticsService.GetTotalLearnersAsync(),
                StudentLearners = await _analyticsService.GetStudentLearnersAsync(),
                EmployeeLearners = await _analyticsService.GetEmployeeLearnersAsync(),
                TotalModules = await _analyticsService.GetTotalModulesAsync(),
                AIInsights = new Dictionary<string, string>() // Initially empty
            };

            return View(viewModel);
        }

        [HttpPost("GenerateAIInsights")]
        public async Task<IActionResult> GenerateAIInsights(int? moduleId)
        {
            var chartData = await _analyticsService.ConsolidateChartDataAsync(moduleId);
            var aiInsights = await _analyticsService.GenerateAIInsightsAsync(chartData);

            // Ensure all keys are present in AIInsights
            if (!aiInsights.ContainsKey("ModulePerformance"))
            {
                aiInsights["ModulePerformance"] = "No insights available for module performance.";
            }

            return Json(aiInsights);
        }


        // ****************************** USER DEMOGRAPHICS API ******************************

        // GET: Analytics/GetUserDemographics
        // API endpoint that returns user demographics data such as age, gender, and location breakdown.
        [HttpGet("GetUserDemographics")]
        public async Task<IActionResult> GetUserDemographics()
        {
            var demographics = await _analyticsService.GetUserDemographicsAsync();
            return Json(demographics);
        }

        // ****************************** MODULE PERFORMANCE API ******************************

        // GET: Analytics/GetModulePerformance/{moduleId}
        // API endpoint that returns performance metrics for a specific module, including quiz scores and completion rates.
        [HttpGet("GetModulePerformance")]
        public async Task<IActionResult> GetModulePerformance(int moduleId)
        {
            var completionRate = await _analyticsService.GetModuleCompletionRateAsync(moduleId);
            var certificatesIssued = await _analyticsService.GetCertificateIssuanceRateAsync(moduleId);
            var averageQuizScore = await _analyticsService.GetAverageQuizScoreAsync(moduleId);

            var performance = new
            {
                AverageQuizScore = averageQuizScore,
                CompletionRate = completionRate,
                CertificateIssuance = certificatesIssued
            };

            return Json(performance);
        }
        // ****************************** ACTIVE USERS API ******************************

        // GET: Analytics/GetCurrentActiveUsers
        // API endpoint that returns the count of currently active users in the system.
        [HttpGet("GetCurrentActiveUsers")]
        public async Task<IActionResult> GetCurrentActiveUsers()
        {
            var activeUsers = await _analyticsService.GetCurrentActiveUsersAsync();
            return Json(new { ActiveUsers = activeUsers });
        }

        // ****************************** LIVE USER PROGRESS API ******************************

        // GET: Analytics/GetLiveUserProgress
        // API endpoint that returns live progress data for a user in a specific module.
        [HttpGet("GetLiveUserProgress")]
        public async Task<IActionResult> GetLiveUserProgress(string userId, int moduleId)
        {
            var progress = await _analyticsService.GetLiveUserProgressAsync(userId, moduleId);
            return Json(progress);
        }

        // ****************************** TRACK MODULE TIME API ******************************

        // POST: Analytics/TrackModuleTime
        // API endpoint that tracks the time spent by a user on a specific module.
        [HttpPost("TrackModuleTime")]
        public async Task<IActionResult> TrackModuleTime([FromBody] ModuleTimeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var moduleActivity = new ModuleActivity
            {
                UserId = userId,
                ModuleId = dto.ModuleId,
                TimeSpent = TimeSpan.FromSeconds(dto.TimeSpent),
                ActivityDate = DateTime.UtcNow.Date
            };

            _context.ModuleActivities.Add(moduleActivity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ****************************** EMPLOYMENT STATUS BREAKDOWN API ******************************

        // GET: Analytics/GetEmploymentStatusBreakdown
        // API endpoint that returns the breakdown of users by employment status (e.g., full-time, part-time).
        [HttpGet("GetEmploymentStatusBreakdown")]
        public async Task<IActionResult> GetEmploymentStatusBreakdown()
        {
            var employmentStatus = await _analyticsService.GetEmploymentStatusBreakdownAsync();
            return Json(employmentStatus);
        }

        // ****************************** COURSE ASSOCIATIONS API ******************************

        // GET: Analytics/GetCourseAssociations
        // API endpoint that returns the associations between users and their enrolled courses.
        [HttpGet("GetCourseAssociations")]
        public async Task<IActionResult> GetCourseAssociations()
        {
            var courseAssociations = await _analyticsService.GetCourseAssociationsAsync();
            return Json(courseAssociations);
        }

        // ***************************** POST COUNT AND KEYWORDS API ****************************

        // GET: Analytics/GetPostCountByModule
        // API endpoint that returns the count of posts by module.
        [HttpGet("GetPostCountByModule")]
        public async Task<IActionResult> GetPostCountByModule()
        {
            try
            {
                var postCounts = await _analyticsService.GetPostCountByModuleAsync();
                return Ok(postCounts); // Returns 200 OK with JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: Analytics/GetCommonKeywords
        // API endpoint that returns the most common keywords used in posts.
        [HttpGet("GetCommonKeywords")]
        public async Task<IActionResult> GetCommonKeywords()
        {
            try
            {
                var commonKeywords = await _analyticsService.GetCommonKeywordsAsync();
                return Ok(commonKeywords); // Returns 200 OK with JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    // ****************************** COURSE ASSOCIATIONS API ******************************

    // GET: Analytics/GetCourseAssociations
    // API endpoint that returns the associations between users and their enrolled courses.

    // DTO for tracking time spent on a module
    public class ModuleTimeDto
    {
        [Required(ErrorMessage = "Module ID is required.")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Time spent is required.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Time spent must be greater than 0.")]
        public double TimeSpent { get; set; } // Time in seconds
    }



}
