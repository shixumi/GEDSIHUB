using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Models.ViewModels;
using GedsiHub.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using GedsiHub.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var notifications = await _context.NotificationUsers
                .Include(nu => nu.Notification)
                .Where(nu => nu.UserId == userId)
                .OrderByDescending(nu => nu.Notification.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var notificationUser = await _context.NotificationUsers
                .Include(nu => nu.Notification)
                .FirstOrDefaultAsync(nu => nu.NotificationId == id && nu.UserId == userId);

            if (notificationUser == null)
            {
                return NotFound();
            }

            // Mark as read if not already
            if (!notificationUser.IsRead)
            {
                notificationUser.IsRead = true;
                _context.Update(notificationUser);
                await _context.SaveChangesAsync();
            }

            return View(notificationUser.Notification);
        }

        // GET: Notifications/GetUnreadCount
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = _userManager.GetUserId(User);
            var count = await _context.NotificationUsers
                .CountAsync(nu => nu.UserId == userId && !nu.IsRead);
            return Json(count);
        }

        // GET: Notifications/GetRecentNotifications
        public async Task<IActionResult> GetRecentNotifications()
        {
            var userId = _userManager.GetUserId(User);

            var notifications = await _context.NotificationUsers
                .Include(nu => nu.Notification)
                .Where(nu => nu.UserId == userId)
                .OrderByDescending(nu => nu.Notification.CreatedAt)
                .Take(5)
                .ToListAsync();

            return PartialView("_RecentNotificationsPartial", notifications);
        }

        // GET: Notifications/AdminCreate
        [Authorize(Roles = "Admin")]
        public IActionResult AdminCreate()
        {
            // Prepare categories and target audiences for the view
            ViewData["Categories"] = new List<string> { "General", "Event", "Update", "Alert" };
            ViewData["TargetAudiences"] = new List<string> { "AllUsers", "Student", "Employee", "Admin" };
            return View();
        }

        // POST: Notifications/AdminCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminCreate(NotificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Automatically determine the IconClass based on the category
                var iconClass = NotificationHelper.GetIconForCategory(model.Category);

                var notification = new Notification
                {
                    Title = model.Title,
                    Message = model.Message,
                    CreatedAt = DateTime.Now,
                    Category = model.Category,
                    IconClass = iconClass,
                    TargetAudience = model.TargetAudience,
                    IsImportant = model.IsImportant
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync(); // Save to get the NotificationId

                // Get the list of users based on the TargetAudience
                IList<ApplicationUser> users;

                if (notification.TargetAudience == "AllUsers")
                {
                    users = _userManager.Users.ToList();
                }
                else
                {
                    users = await _userManager.GetUsersInRoleAsync(notification.TargetAudience);
                }

                // Create NotificationUser entries
                foreach (var user in users)
                {
                    var notificationUser = new NotificationUser
                    {
                        NotificationId = notification.Id,
                        UserId = user.Id,
                        IsRead = false
                    };
                    _context.NotificationUsers.Add(notificationUser);
                }

                await _context.SaveChangesAsync();

                // Send notification via SignalR to specific users
                await _hubContext.Clients.Users(users.Select(u => u.Id)).SendAsync("ReceiveNotification");

                return RedirectToAction(nameof(Index));
            }

            // Re-populate ViewData
            ViewData["Categories"] = new List<string> { "General", "Event", "Update", "Alert" };
            ViewData["TargetAudiences"] = new List<string> { "AllUsers", "Student", "Employee", "Admin" };
            return View(model);
        }
    }
}
