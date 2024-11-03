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
    public IActionResult Index()
    {
        var complaints = _context.Feedbacks
            .Where(f => f.FeedbackType == "Complaint" && !f.IsResolved)
            .ToList();

        var suggestions = _context.Feedbacks
            .Where(f => f.FeedbackType == "Suggestion" && !f.IsResolved)
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
        return RedirectToAction("Success");
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
            AffectedArea = feedback.AffectedArea, // Populate this as needed
            EvidencePath = feedback.EvidencePath // Ensure this property is available in your Feedback model
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
