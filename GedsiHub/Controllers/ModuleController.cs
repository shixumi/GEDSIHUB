using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(ApplicationDbContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Reusable method to fetch a module by ID
        private async Task<Module> GetModuleByIdAsync(int id)
        {
            return await _context.Modules
                                 .Include(m => m.Lessons)
                                 .FirstOrDefaultAsync(m => m.ModuleId == id);
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var modules = await _context.Modules.Include(m => m.Lessons).ToListAsync();
            return View(modules);
        }

        // GET: Module Details
        public async Task<IActionResult> Details(int id)
        {
            var module = await GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Create a new Module
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create a new Module with improved error handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Status,Color")] Module module)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    module.CreatedDate = DateTime.UtcNow;
                    module.LastModifiedDate = DateTime.UtcNow;
                    _context.Add(module);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database error while creating module");
                    ModelState.AddModelError("", "Database error occurred. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating module");
                    ModelState.AddModelError("", "An error occurred while creating the module.");
                }
            }

            return View(module);
        }

        // GET: Edit a Module
        public async Task<IActionResult> Edit(int id)
        {
            var module = await GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Edit a Module with improved error handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Title,Description,Status,Color,CreatedDate,LastModifiedDate")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    module.LastModifiedDate = DateTime.UtcNow;
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.ModuleId))
                        return NotFound();

                    _logger.LogError("Concurrency error while editing module with ID {ModuleId}", module.ModuleId);
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database error while editing module");
                    ModelState.AddModelError("", "Database error occurred while updating. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while editing module");
                    ModelState.AddModelError("", "An error occurred while editing the module.");
                }
            }

            return View(module);
        }

        // DELETE: Module
        public async Task<IActionResult> Delete(int id)
        {
            var module = await GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module != null)
            {
                _context.Modules.Remove(module);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Publish a Module
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = ModuleStatus.Published;
            module.LastModifiedDate = DateTime.UtcNow;
            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Unpublish a Module
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = ModuleStatus.Unpublished;
            module.LastModifiedDate = DateTime.UtcNow;
            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Toggle status between Published and Unpublished
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = module.Status == ModuleStatus.Published ? ModuleStatus.Unpublished : ModuleStatus.Published;
            module.LastModifiedDate = DateTime.UtcNow;
            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Update the color of a module
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateColor(int id, string color)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
                return NotFound();

            // Remove manual validation and rely on model-level validation
            module.Color = color;
            module.LastModifiedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while updating color for module {ModuleId}", id);
                    TempData["Error"] = "An error occurred while updating the module color.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }
    }
}
