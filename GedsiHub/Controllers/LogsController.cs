using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ****************************** LOG MANAGEMENT ******************************

        // GET: Display activity logs
        // Retrieves all activity logs and displays them in a view.
        // This action is restricted to Admins only.
        public async Task<IActionResult> Index()
        {
            // Fetch all logs from the database.
            var logs = await _context.ActivityLogs
                                     .OrderByDescending(l => l.Timestamp)
                                     .ToListAsync();

            return View(logs);
        }
    }
}
