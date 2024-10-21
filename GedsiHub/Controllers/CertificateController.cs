using Microsoft.AspNetCore.Mvc;
using GedsiHub.Models;
using GedsiHub.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using GedsiHub.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/certificate")]
public class CertificateController : ControllerBase
{
    private readonly CertificateService _certificateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CertificateController(CertificateService certificateService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _certificateService = certificateService;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("{userId}/{moduleId}")]
    public async Task<IActionResult> GetCertificate(string userId, int moduleId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var module = await _context.Modules.FindAsync(moduleId);
        if (module == null)
        {
            return NotFound("Module not found.");
        }

        var progress = await _context.UserProgresses
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId && up.IsCompleted);

        if (progress == null)
        {
            return BadRequest("Module not yet completed.");
        }

        try
        {
            var pdfBytes = await _certificateService.GenerateAndStoreCertificateAsync(userId, moduleId);
            return File(pdfBytes, "application/pdf", "certificate.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Will return "Certificate already issued" if applicable
        }
    }
}
