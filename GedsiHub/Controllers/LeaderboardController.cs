// Controllers/LeaderboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GedsiHub.Data;
using GedsiHub.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GedsiHub.ViewModels;
using GedsiHub.Models;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin,Student,Employee")]
    public class LeaderboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AnalyticsService _analyticsService;

        public LeaderboardController(ApplicationDbContext context, AnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        // GET: Leaderboard
        public async Task<IActionResult> Index(string scope = "Overall", int? moduleId = null)
        {
            LeaderboardPageViewModel pageViewModel = new LeaderboardPageViewModel();

            if (scope == "Module" && moduleId.HasValue)
            {
                // Fetch module details
                var module = await _context.Modules
                    .FirstOrDefaultAsync(m => m.ModuleId == moduleId.Value && m.Status == ModuleStatus.Published);

                if (module == null)
                {
                    TempData["Error"] = "Selected module does not exist or is unpublished.";
                    return RedirectToAction("Index");
                }

                pageViewModel.SelectedModuleId = moduleId.Value.ToString();
                pageViewModel.SelectedModuleName = module.Title;
            }
            else
            {
                scope = "Overall";
                pageViewModel.SelectedModuleId = "Overall";
                pageViewModel.SelectedModuleName = "Overall Leaderboard";
            }

            // Fetch leaderboard entries based on scope from AnalyticsService
            var leaderboardEntries = await _analyticsService.GetLeaderboardAsync(scope, moduleId);
            pageViewModel.LeaderboardEntries = leaderboardEntries;

            // Fetch published modules for the sidebar
            pageViewModel.PublishedModules = await _analyticsService.GetPublishedModulesAsync();

            return View(pageViewModel);
        }

    }
}
