using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace GedsiHub.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatbotController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Chat");
        }

        // Fetch available modules
        [HttpGet("api/chatbot/modules")]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _context.Modules
                .Select(m => new { m.Title })  // Select only the title for simplicity
                .ToListAsync();

            return Ok(new { modules });
        }

        // Fetch FAQs
        [HttpGet("api/chatbot/faqs")]
        public async Task<IActionResult> GetFAQs()
        {
            var faqs = await _context.FAQs
                .Select(f => new { f.Id, f.Question, f.Answer })  // Return both Question and Answer
                .ToListAsync();

            return Ok(new { faqs });
        }

        // Fetch FAQ answer by ID
        [HttpGet("api/chatbot/faq/{id}")]
        public async Task<IActionResult> GetFaqAnswer(int id)
        {
            var faq = await _context.FAQs
                .Where(f => f.Id == id)
                .Select(f => new { f.Answer })
                .FirstOrDefaultAsync();

            if (faq == null)
            {
                return NotFound();
            }

            return Ok(faq);
        }

        // Fetch module details by ID
        [HttpGet("api/chatbot/module/{id}")]
        public async Task<IActionResult> GetModuleDetails(int id)
        {
            var module = await _context.Modules
                .Where(m => m.ModuleId == id)
                .Select(m => new { m.Description })
                .FirstOrDefaultAsync();

            if (module == null)
            {
                return NotFound();
            }

            return Ok(new { details = module.Description });
        }

        // Fetch Contact Info
        [HttpGet("api/chatbot/contact")]
        public async Task<IActionResult> GetContact()
        {
            var contact = await _context.ContactInfos
                .Select(c => new
                {
                    c.SupportEmail,
                    c.PhoneNumber,
                    c.Facebook,
                    c.TikTok,
                    c.Instagram,
                    c.X,
                    c.Website
                })
                .FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            // Return the contact info with camelCase property names to match the frontend expectations
            return Ok(new
            {
                contactInfo = new
                {
                    supportEmail = contact.SupportEmail,
                    phoneNumber = contact.PhoneNumber,
                    facebook = contact.Facebook,
                    tikTok = contact.TikTok,
                    instagram = contact.Instagram,
                    x = contact.X,
                    website = contact.Website
                }
            });
        }
    }
}
