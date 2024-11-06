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
        // Validate that the user exists
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Check if the module exists
        var module = await _context.Modules.FindAsync(moduleId);
        if (module == null)
        {
            return NotFound("Module not found.");
        }

        // Check for existing certificate
        var existingCertificate = await _context.Certificates
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ModuleId == moduleId);

        if (existingCertificate == null)
        {
            return BadRequest("No certificate issued for this module."); // Change this to your logic
        }

        // If an existing certificate is found, retrieve its PDF bytes
        var pdfBytes = await _certificateService.GetCertificateBytesAsync(userId, moduleId);
        if (pdfBytes == null)
        {
            return NotFound("Certificate not found.");
        }

        // Return the PDF to download
        return File(pdfBytes, "application/pdf", "certificate.pdf");
    }

}
