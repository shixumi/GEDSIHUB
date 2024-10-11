using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]  // This will map to /api/chatbot for the API calls
public class ChatbotController : Controller  // Use Controller instead of ControllerBase for view support
{
    // This action will return the Chat view when you navigate to /Chatbot
    [HttpGet("/Chatbot")]
    public IActionResult Index()
    {
        return View("Chat");  // Returns the Chat.cshtml view from Views/Chatbot/Chat.cshtml
    }

    // API to handle chatbot interactions via POST request
    [HttpPost("ask")]
    public IActionResult Ask([FromBody] string userQuestion)
    {
        if (string.IsNullOrEmpty(userQuestion))
        {
            return BadRequest(new { response = "Question cannot be empty." });
        }

        // Simple response for now (echo back the user question)
        var response = new { response = "You asked: " + userQuestion };
        return Ok(response);  // Return a valid JSON response for the API
    }
}
