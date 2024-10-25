using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;

public class FeedbackController : Controller
{
	private readonly ApplicationDbContext _context;

	public FeedbackController(ApplicationDbContext context)
	{
		_context = context;
	}

	// ************** USER INTERFACE **************

	[Authorize(Roles = "Student, Employee")]  // Accessible by both Students and Employees
	public IActionResult Index()
	{
		return View();  // Views/Feedback/Index.cshtml
	}

	[Authorize(Roles = "Student, Employee")]  // Accessible by both Students and Employees
	public IActionResult Complaint()
	{
		return View();  // Views/Feedback/Complaint.cshtml
	}

	[Authorize(Roles = "Student, Employee")]  // Accessible by both Students and Employees
	[HttpPost]
	public async Task<IActionResult> SubmitComplaint(FeedbackComplaint model)
	{
		if (ModelState.IsValid)
		{
			var feedback = new Feedback
			{
				UserName = User.Identity.Name,
				Date = DateTime.Now,
				Type = model.TypeOfIssue,
				FeedbackType = "Complaint",
				Description = model.DetailedDescription
			};

			// Ensure the uploads directory exists
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);  // Create uploads directory if it doesn't exist
			}

			// Handle optional Evidence
			if (model.Evidence != null)
			{
				var filePath = Path.Combine(uploadPath, model.Evidence.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.Evidence.CopyToAsync(stream);
				}
			}

			_context.Feedbacks.Add(feedback);
			await _context.SaveChangesAsync();

			return RedirectToAction("Success");
		}

		return View("Complaint", model);
	}

	[Authorize(Roles = "Student, Employee")]  // Accessible by both Students and Employees
	public IActionResult Suggestion()
	{
		return View();  // Views/Feedback/Suggestion.cshtml
	}

	[Authorize(Roles = "Student, Employee")]  // Accessible by both Students and Employees
	[HttpPost]
	public async Task<IActionResult> SubmitSuggestion(FeedbackSuggestion model)
	{
		if (ModelState.IsValid)
		{
			var feedback = new Feedback
			{
				UserName = User.Identity.Name,
				Date = DateTime.Now,
				Type = model.TypeOfSuggestion,
				FeedbackType = "Suggestion",
				Description = model.DescriptionOfSuggestion
			};

			// Ensure the uploads directory exists
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);  // Create uploads directory if it doesn't exist
			}

			// Handle optional Attachment
			if (model.Attachment != null)
			{
				var filePath = Path.Combine(uploadPath, model.Attachment.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.Attachment.CopyToAsync(stream);
				}
			}

			_context.Feedbacks.Add(feedback);
			await _context.SaveChangesAsync();

			return RedirectToAction("Success");
		}

		return View("Suggestion", model);
	}

	public IActionResult Success()
	{
		return View();  // Views/Feedback/Success.cshtml
	}

	// ************** ADMIN INTERFACE **************

	[Authorize(Roles = "Admin")]  // Accessible by Admins only
	public IActionResult AdminIndex(string type = "Complaint")
	{
		var feedbacks = _context.Feedbacks
			.Where(f => f.FeedbackType == type && !f.IsResolved)
			.ToList();

		return View(feedbacks);  // Views/Feedback/AdminIndex.cshtml
	}

	[Authorize(Roles = "Admin")]  // Accessible by Admins only
	public IActionResult Details(int id)
	{
		var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
		if (feedback == null) return NotFound();

		return View(feedback);  // Views/Feedback/Details.cshtml
	}

	[Authorize(Roles = "Admin")]  // Accessible by Admins only
	public IActionResult MarkAsResolved(int id)
	{
		var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
		if (feedback == null) return NotFound();

		feedback.IsResolved = true;
		_context.SaveChanges();

		return RedirectToAction(nameof(AdminIndex));
	}

	[Authorize(Roles = "Admin")]  // Accessible by Admins only
	public IActionResult Delete(int id)
	{
		var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
		if (feedback == null) return NotFound();

		_context.Feedbacks.Remove(feedback);
		_context.SaveChanges();

		return RedirectToAction(nameof(AdminIndex));
	}
}
