﻿using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var modules = await _context.Modules.ToListAsync();
            return View(modules);
        }

        // GET: Module Details
        public async Task<IActionResult> Details(int id)
        {
            var module = await _context.Modules
                .Include(m => m.Lessons)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

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

        // POST: Create a new Module
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Module module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(module);
        }

        // GET: Edit a Module
        public async Task<IActionResult> Edit(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: Edit a Module
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Title,Description,CreatedDate,LastModifiedDate")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.ModuleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(module);
        }

        // GET: Delete a Module
        public async Task<IActionResult> Delete(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: Delete a Module
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

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }
    }
}
