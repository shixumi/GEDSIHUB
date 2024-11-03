using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;

[Authorize]
public class FeedbackController : Controller
{
	private readonly ApplicationDbContext _context;

	public FeedbackController(ApplicationDbContext context)
	{
		_context = context;
	}

    // ************** USER INTERFACE **************

    [Authorize]
    public IActionResult Index(string tab = "complaints", string status = "unresolved")
    {
        // Determine whether to show resolved or unresolved feedback
        bool showResolved = status == "resolved";
        ViewBag.ShowResolved = showResolved; // Set whether to show resolved feedback
        ViewBag.ActiveTab = tab; // Set the active tab

        // Fetch complaints and suggestions based on the resolved status
        var complaints = _context.Feedbacks
            .Where(f => f.FeedbackType == "Complaint" && f.IsResolved == showResolved)
            .ToList();

        var suggestions = _context.Feedbacks
            .Where(f => f.FeedbackType == "Suggestion" && f.IsResolved == showResolved)
            .ToList();

        // Pass both lists to the view via a ViewModel
        var model = new FeedbackAdminViewModel
        {
            Complaints = complaints,
            Suggestions = suggestions
        };

        return View(model);
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
        if (!ModelState.IsValid)
        {
            // If the model state is invalid, return to the view and show validation errors
            return View("Complaint", model);
        }

        // Map properties from FeedbackComplaint to Feedback
        var feedback = new Feedback
        {
            UserName = User.Identity.Name,
            Date = DateTime.Now,
            Type = model.TypeOfIssue,
            FeedbackType = "Complaint",
            Description = model.DetailedDescription,
            AffectedArea = model.AffectedArea, // Ensure this is correctly set
            IsResolved = false // Default to false, since the complaint is new
        };

        // Handle optional Evidence (file upload)
        if (model.Evidence != null && model.Evidence.Length > 0)
        {
            // Define the upload path
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Create a unique file name to prevent overwriting existing files
            var uniqueFileName = $"{Guid.NewGuid()}_{model.Evidence.FileName}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Evidence.CopyToAsync(stream);
            }

            // Save the file path in the EvidencePath property
            feedback.EvidencePath = $"/uploads/{uniqueFileName}"; // Ensure EvidencePath is set in the Feedback model
        }

        // Add the feedback to the database and save changes
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        // Redirect to a success page or confirmation view
        return RedirectToAction("Success", new { type = "Complaint" });
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
        if (!ModelState.IsValid)
        {
            // If the model state is invalid, return to the view and show validation errors
            return View("Suggestion", model);
        }

        // Map properties from FeedbackSuggestion to Feedback
        var feedback = new Feedback
        {
            UserName = User.Identity.Name,
            Date = DateTime.Now,
            Type = model.TypeOfSuggestion,
            FeedbackType = "Suggestion",
            Description = model.DescriptionOfSuggestion,
            IsResolved = false // Default to false, since the suggestion is new
        };

        // Handle optional Attachment (file upload)
        if (model.Attachment != null && model.Attachment.Length > 0)
        {
            // Define the upload path
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Create a unique file name to prevent overwriting existing files
            var uniqueFileName = $"{Guid.NewGuid()}_{model.Attachment.FileName}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Attachment.CopyToAsync(stream);
            }

            // Save the file path in the EvidencePath property
            feedback.EvidencePath = $"/uploads/{uniqueFileName}"; // Set EvidencePath
        }

        // Add the feedback to the database and save changes
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        // Redirect to a success page or confirmation view
        return RedirectToAction("Success", new { type = "Suggestion" });
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

    [Authorize(Roles = "Admin")]
    public IActionResult Details(int id)
    {
        var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
        if (feedback == null) return NotFound();

        // Find the next feedback item
        var nextFeedback = _context.Feedbacks
            .Where(f => f.FeedbackType == feedback.FeedbackType && f.Id > id)
            .OrderBy(f => f.Id)
            .FirstOrDefault();

        // Map the Feedback object to the FeedbackDetailsViewModel
        var viewModel = new FeedbackDetailsViewModel
        {
            Id = feedback.Id,
            UserName = feedback.UserName,
            Date = feedback.Date,
            Type = feedback.Type,
            FeedbackType = feedback.FeedbackType,
            Description = feedback.Description,
            IsResolved = feedback.IsResolved,
            AffectedArea = feedback.AffectedArea,
            EvidencePath = feedback.EvidencePath,
            NextFeedbackId = nextFeedback?.Id
        };

        return View(viewModel);  // Pass the ViewModel to the view
    }

    [Authorize(Roles = "Admin")]
	public IActionResult MarkAsResolved(int id)
	{
		var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
		if (feedback == null) return NotFound();

		feedback.IsResolved = true;
		_context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

	[Authorize(Roles = "Admin")]
	public IActionResult Delete(int id)
	{
		var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
		if (feedback == null) return NotFound();

		_context.Feedbacks.Remove(feedback);
		_context.SaveChanges();

		return RedirectToAction(nameof(Index));
	}
}
