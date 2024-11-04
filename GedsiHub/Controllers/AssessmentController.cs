using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AssessmentController> _logger;

        public AssessmentController(ApplicationDbContext context, ILogger<AssessmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ****************************** CREATE ASSESSMENT ******************************
        [Authorize(Roles = "Admin")]
        // GET: Assessment/Create/{moduleId}
        public IActionResult Create(int moduleId)
        {
            _logger.LogInformation($"Creating assessment for ModuleId: {moduleId}");

            // Load the module and its assessment
            var module = _context.Modules.Include(m => m.Assessment).FirstOrDefault(m => m.ModuleId == moduleId);
            if (module == null)
            {
                _logger.LogWarning($"Module with ID {moduleId} not found.");
                return NotFound();
            }

            if (module.Assessment != null)
            {
                _logger.LogInformation($"Assessment already exists for ModuleId: {moduleId}, redirecting to edit.");
                return RedirectToAction("Edit", new { id = module.Assessment.AssessmentId });
            }

            // Create a new assessment and pass the module object
            var assessment = new Assessment
            {
                ModuleId = moduleId,
                Module = module
            };

            return View(assessment);
        }

        // POST: Assessment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,H5PEmbedCode,ModuleId")] Assessment assessment)
        {
            _logger.LogInformation("Creating new assessment.");

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid. Logging validation errors...");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError($"Validation Error: {error.ErrorMessage}");
                }
                return View(assessment);
            }

            // Retrieve the module by ModuleId and assign it to the assessment
            var module = await _context.Modules.FindAsync(assessment.ModuleId);
            if (module == null)
            {
                _logger.LogError($"Module with ID {assessment.ModuleId} not found.");
                ModelState.AddModelError("ModuleId", "Invalid Module.");
                return View(assessment);
            }

            assessment.Module = module;

            try
            {
                _logger.LogInformation($"Saving new assessment for ModuleId: {assessment.ModuleId}");
                _context.Assessments.Add(assessment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Assessment created successfully for ModuleId: {assessment.ModuleId} with AssessmentId: {assessment.AssessmentId}");
                return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving assessment: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the assessment. Please try again.");
                return View(assessment);
            }
        }

        // ****************************** SHOW ASSESSMENT ******************************
        // GET: Assessment/Details/{id}
        [Authorize(Roles = "Student, Employee, Admin")]
        public async Task<IActionResult> Details(int id)
        {
            // Load the assessment and its associated module
            var assessment = await _context.Assessments
                .Include(a => a.Module)
                .FirstOrDefaultAsync(a => a.AssessmentId == id);

            if (assessment == null)
            {
                _logger.LogWarning($"Details: Assessment with ID {id} not found.");
                return NotFound();
            }

            // Pass the module information to the view using ViewBag
            ViewBag.ModuleId = assessment.ModuleId;
            ViewBag.ModuleTitle = assessment.Module?.Title ?? "Unknown Module";
            ViewBag.AssessmentId = assessment.AssessmentId;

            return View(assessment);
        }

        // ****************************** EDIT ASSESSMENT ******************************
        [Authorize(Roles = "Admin")]
        // GET: Assessment/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                _logger.LogWarning("Edit assessment: Assessment ID is null.");
                return BadRequest();
            }

            var assessment = await _context.Assessments
                .Include(a => a.Module)
                .FirstOrDefaultAsync(a => a.AssessmentId == id.Value);

            if (assessment == null)
            {
                _logger.LogWarning($"Edit assessment: Assessment with ID {id} not found.");
                return NotFound();
            }

            ViewBag.ModuleTitle = assessment.Module?.Title ?? "Unknown Module";

            return View(assessment);
        }

        // POST: Assessment/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AssessmentId,Title,Description,H5PEmbedCode,ModuleId,CreatedDate,LastModifiedDate")] Assessment assessment)
        {
            if (id != assessment.AssessmentId)
            {
                _logger.LogWarning($"Edit assessment: Assessment ID mismatch (id: {id}, AssessmentId: {assessment.AssessmentId}).");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid when editing assessment.");
                return View(assessment);
            }

            // Ensure the module is valid before proceeding
            var module = await _context.Modules.FindAsync(assessment.ModuleId);
            if (module == null)
            {
                _logger.LogError($"Module with ID {assessment.ModuleId} not found.");
                ModelState.AddModelError("ModuleId", "Invalid Module.");
                return View(assessment);
            }

            assessment.Module = module;

            try
            {
                _logger.LogInformation($"Updating assessment with ID {id} for ModuleId: {assessment.ModuleId}");
                _context.Update(assessment);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Assessment with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AssessmentExists(assessment.AssessmentId))
                {
                    _logger.LogWarning($"Edit assessment: Assessment with ID {assessment.AssessmentId} no longer exists.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Concurrency error while updating assessment: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating assessment: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the assessment. Please try again.");
                return View(assessment);
            }

            return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
        }

        // ****************************** DELETE ASSESSMENT ******************************
        [Authorize(Roles = "Admin")]
        // GET: Assessment/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete assessment: Assessment ID is null.");
                return BadRequest();
            }

            var assessment = await _context.Assessments
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.AssessmentId == id);

            if (assessment == null)
            {
                _logger.LogWarning($"Delete assessment: Assessment with ID {id} not found.");
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessment/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment != null)
            {
                _logger.LogInformation($"Deleting assessment with ID {id}.");
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Assessment with ID {id} deleted successfully.");
                return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
            }

            _logger.LogWarning($"Delete assessment: Assessment with ID {id} not found during deletion.");
            return NotFound();
        }

        // ****************************** SUBMIT ASSESSMENT ******************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student, Employee")]
        public async Task<IActionResult> Submit(int assessmentId)
        {
            _logger.LogInformation($"User attempting to submit assessment with ID {assessmentId}.");

            // Get the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Submit assessment: User not authenticated.");
                return Unauthorized("User not authenticated.");
            }

            // Get the assessment
            var assessment = await _context.Assessments
                                           .Include(a => a.Module)
                                           .FirstOrDefaultAsync(a => a.AssessmentId == assessmentId);
            if (assessment == null)
            {
                _logger.LogWarning($"Submit assessment: Assessment with ID {assessmentId} not found.");
                return NotFound("Assessment not found.");
            }

            // Get or create the UserProgress entry for the module
            var userProgress = await _context.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == assessment.ModuleId);

            if (userProgress == null)
            {
                userProgress = new UserProgress
                {
                    UserId = userId,
                    ModuleId = assessment.ModuleId,
                    ProgressPercentage = 100,
                    IsCompleted = true,
                    CompletedLessonIds = "" // Assuming assessment completes the module regardless of lesson completions
                };
                _context.UserProgresses.Add(userProgress);
            }
            else
            {
                userProgress.IsCompleted = true;
                userProgress.ProgressPercentage = 100;
                // Optionally, update CompletedLessonIds if needed
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User {userId} completed assessment for ModuleId {assessment.ModuleId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Submit assessment: Error updating UserProgress - {ex.Message}");
                return StatusCode(500, "An error occurred while updating your progress.");
            }

            // Redirect to /Api/Certificate/userID/ModuleId
            var redirectUrl = $"/Api/Certificate/{userId}/{assessment.ModuleId}";
            return Redirect(redirectUrl);
        }

        // ****************************** HELPER METHOD ******************************
        private bool AssessmentExists(int id)
        {
            var exists = _context.Assessments.Any(e => e.AssessmentId == id);
            _logger.LogInformation($"Checking if assessment with ID {id} exists: {exists}");
            return exists;
        }
    }
}
