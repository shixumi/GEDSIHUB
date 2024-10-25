// Controllers/AnalyticsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GedsiHub.Models;
using GedsiHub.Data;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GedsiHub.Services;
using GedsiHub.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]  // Only Admins have access to the AnalyticsController
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

        // Endpoint to display the dashboard view
        [HttpGet("")]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var modules = await _context.Modules.ToListAsync();
            var viewModel = new AnalyticsDashboardViewModel
            {
                Modules = modules
            };
            return View(viewModel);
        }

        // API endpoint to get user demographics data
        [HttpGet("GetUserDemographics")]
        public async Task<IActionResult> GetUserDemographics()
        {
            var demographics = await _analyticsService.GetUserDemographicsAsync();
            return Json(demographics);
        }

        // API endpoint to get module performance metrics
        [HttpGet("GetModulePerformance")]
        public async Task<IActionResult> GetModulePerformance(int moduleId)
        {
            var averageQuizScore = await _analyticsService.GetAverageQuizScoreAsync(moduleId);
            var completionRate = await _analyticsService.GetModuleCompletionRateAsync(moduleId);
            var certificateIssuance = await _analyticsService.GetCertificateIssuanceRateAsync(moduleId);

            var performance = new
            {
                AverageQuizScore = averageQuizScore,
                CompletionRate = completionRate,
                CertificateIssuance = certificateIssuance
            };

            return Json(performance);
        }

        // API endpoint to get the count of current active users
        [HttpGet("GetCurrentActiveUsers")]
        public async Task<IActionResult> GetCurrentActiveUsers()
        {
            var activeUsers = await _analyticsService.GetCurrentActiveUsersAsync();
            return Json(new { ActiveUsers = activeUsers });
        }

        // API endpoint to get live progress data for a user in a specific module
        [HttpGet("GetLiveUserProgress")]
        public async Task<IActionResult> GetLiveUserProgress(string userId, int moduleId)
        {
            var progress = await _analyticsService.GetLiveUserProgressAsync(userId, moduleId);
            return Json(progress);
        }

        // API endpoint to get feedback analysis data
        [HttpGet("GetFeedbackAnalysis")]
        public async Task<IActionResult> GetFeedbackAnalysis()
        {
            var feedbackAnalysis = await _analyticsService.GetFeedbackAnalysisAsync();
            return Json(feedbackAnalysis);
        }

        // API endpoint to track time spent on a module
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

        // API endpoint to get employment status breakdown data
        [HttpGet("GetEmploymentStatusBreakdown")]
        public async Task<IActionResult> GetEmploymentStatusBreakdown()
        {
            var employmentStatus = await _analyticsService.GetEmploymentStatusBreakdownAsync();
            return Json(employmentStatus);
        }

        // API endpoint to get course associations data
        [HttpGet("GetCourseAssociations")]
        public async Task<IActionResult> GetCourseAssociations()
        {
            var courseAssociations = await _analyticsService.GetCourseAssociationsAsync();
            return Json(courseAssociations);
        }

        // API endpoint to get user satisfaction levels data
        [HttpGet("GetUserSatisfactionLevels")]
        public async Task<IActionResult> GetUserSatisfactionLevels()
        {
            var satisfactionLevels = await _analyticsService.GetUserSatisfactionLevelsAsync();
            return Json(satisfactionLevels);
        }
    }

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
