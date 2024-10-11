using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : Controller
{
    private readonly ApplicationDbContext _context;

    public ChatbotController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Chatbot
    [HttpGet("/Chatbot")]
    public IActionResult Index()
    {
        return View("Chat"); // Return the Chat.cshtml view
    }

    // POST: /api/chatbot/ask
    [HttpPost("ask")]
    public IActionResult Ask([FromBody] string userQuestion)
    {
        if (string.IsNullOrEmpty(userQuestion))
        {
            return BadRequest(new { response = "Question cannot be empty." });
        }

        // First check if there's a relevant FAQ
        var faqAnswer = FindFAQ(userQuestion);
        if (faqAnswer != null)
        {
            return Ok(new { response = faqAnswer });
        }

        // If no FAQ found, check for a relevant module
        var recommendedModule = FindRelevantModule(userQuestion);
        if (recommendedModule != null)
        {
            var response = $"I recommend checking out the module '{recommendedModule.Title}', which covers relevant topics.";
            return Ok(new { response });
        }

        // Default response if nothing matches
        var defaultResponse = "Sorry, I couldn't find any relevant information. Please try rephrasing your question.";
        return Ok(new { response = defaultResponse });
    }

    // Search for an FAQ match based on the user's question
    private string FindFAQ(string question)
    {
        var keywords = question.Split(' ').Select(k => k.ToLower()).ToList();
        var relevantFAQ = _context.FAQs
            .FirstOrDefault(f => keywords.Any(k => f.Question.ToLower().Contains(k)));

        return relevantFAQ?.Answer; // Return the FAQ answer if found, otherwise null
    }

    // Search for a relevant module based on the user's question
    private Module FindRelevantModule(string question)
    {
        var keywords = question.Split(' ').Select(k => k.ToLower()).ToList();
        var relevantModule = _context.Modules
            .FirstOrDefault(m => keywords.Any(k => m.Title.ToLower().Contains(k) || m.Description.ToLower().Contains(k)));

        return relevantModule; // Return the first matching module or null
    }
}
