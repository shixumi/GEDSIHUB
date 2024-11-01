// Controllers/DataController.cs
using GedsiHub.Models; // Correct reference
using GedsiHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    public class DataController : Controller
    {
        private readonly WatershedApiService _apiService;
        private readonly ILogger<DataController> _logger;

        public DataController(WatershedApiService apiService, ILogger<DataController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                LrsResponse lrsResponse = await _apiService.GetXAPIDataAsync();
                // Pass the LrsResponse object directly to the view
                return View("Index", lrsResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch xAPI data from Watershed.");
                return View("Error", ex.Message);
            }
        }
    }
}
