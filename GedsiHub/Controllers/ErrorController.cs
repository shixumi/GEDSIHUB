using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class ErrorController : Controller
{
    [Route("Error/403")]
    public IActionResult AccessDenied()
    {
        return View("AccessDenied"); // Make sure you have AccessDenied.cshtml in Views/Error/
    }

    [Route("Error/404")]
    public IActionResult NotFound()
    {
        return View("NotFound"); // Make sure you have NotFound.cshtml in Views/Error/
    }
}
