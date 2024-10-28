using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // GET: Assessment/Create/{moduleId}
        // Displays the form to create a new assessment for a specified module.
        // If an assessment already exists for the module, it redirects to the edit page.
        public IActionResult Create(int moduleId)
        {
            _logger.LogInformation($"Creating assessment for ModuleId: {moduleId}");

            var module = _context.Modules.Include(m => m.Assessment).FirstOrDefault(m => m.ModuleId == moduleId);
            if (module == null)
            {
                _logger.LogWarning($"Module with ID {moduleId} not found.");
                return NotFound();
            }

            if (module.Assessment != null)
            {
                _logger.LogInformation($"Assessment already exists for ModuleId: {moduleId}, redirecting to edit.");
                // Redirect to Edit if Assessment already exists
                return RedirectToAction("Edit", new { id = module.Assessment.AssessmentId });
            }

            var assessment = new Assessment
            {
                ModuleId = moduleId
            };

            return View(assessment);
        }

        // POST: Assessment/Create
        // Handles the submission of the form to create a new assessment.
        // Validates the input and saves the new assessment to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving assessment: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the assessment. Please try again.");
                return View(assessment);
            }

            return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
        }

        // ****************************** EDIT ASSESSMENT ******************************

        // GET: Assessment/Edit/{id}
        // Displays the form to edit an existing assessment by its ID.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit assessment: Assessment ID is null.");
                return BadRequest();
            }

            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
            {
                _logger.LogWarning($"Edit assessment: Assessment with ID {id} not found.");
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessment/Edit/{id}
        // Handles the submission of the form to edit an assessment.
        // Validates the input and updates the assessment in the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssessmentId,Title,Description,H5PEmbedCode,ModuleId,CreatedDate,LastModifiedDate")] Assessment assessment)
        {
            if (id != assessment.AssessmentId)
            {
                _logger.LogWarning($"Edit assessment: Assessment ID mismatch (id: {id}, AssessmentId: {assessment.AssessmentId}).");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid when editing assessment.");
                return View(assessment);
            }

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

        // GET: Assessment/Delete/{id}
        // Displays the confirmation page to delete an assessment by its ID.
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
        // Handles the confirmation and deletion of an assessment.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        // ****************************** HELPER METHOD ******************************

        // Helper method to check if an assessment exists by ID.
        private bool AssessmentExists(int id)
        {
            var exists = _context.Assessments.Any(e => e.AssessmentId == id);
            _logger.LogInformation($"Checking if assessment with ID {id} exists: {exists}");
            return exists;
        }
    }
}
