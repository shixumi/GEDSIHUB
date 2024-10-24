using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access this controller
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display activity logs
        public async Task<IActionResult> Index()
        {
            var logs = await _context.ActivityLogs.ToListAsync();
            return View(logs);
        }
    }
}
