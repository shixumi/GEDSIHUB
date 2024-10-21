using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssessmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Assessment/Create
        public IActionResult Create(int moduleId)
        {
            ViewBag.ModuleId = moduleId;
            ViewBag.ModuleTitle = _context.Modules.Find(moduleId)?.Title;
            return View();
        }

        // POST: Assessment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,H5PId,H5PMetadata,ModuleId")] Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
            }

            ViewBag.ModuleId = assessment.ModuleId;
            ViewBag.ModuleTitle = _context.Modules.Find(assessment.ModuleId)?.Title;
            return View(assessment);
        }

        // GET: Assessment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Assessment ID is required.");
            }

            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }

            ViewBag.ModuleId = assessment.ModuleId;
            ViewBag.ModuleTitle = _context.Modules.Find(assessment.ModuleId)?.Title;
            return View(assessment);
        }

        // POST: Assessment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssessmentId,Title,Description,H5PId,H5PMetadata,ModuleId,CreatedDate,LastModifiedDate")] Assessment assessment)
        {
            if (id != assessment.AssessmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    assessment.LastModifiedDate = DateTime.UtcNow;
                    _context.Update(assessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentExists(assessment.AssessmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Module", new { id = assessment.ModuleId });
            }

            ViewBag.ModuleId = assessment.ModuleId;
            ViewBag.ModuleTitle = _context.Modules.Find(assessment.ModuleId)?.Title;
            return View(assessment);
        }

        // GET: Assessment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Assessment ID is required.");
            }

            var assessment = await _context.Assessments
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.AssessmentId == id);

            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment != null)
            {
                int moduleId = assessment.ModuleId;
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Module", new { id = moduleId });
            }
            return NotFound();
        }

        private bool AssessmentExists(int id)
        {
            return _context.Assessments.Any(e => e.AssessmentId == id);
        }
    }
}
