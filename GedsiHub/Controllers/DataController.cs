using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class DataController : Controller
{
    private readonly WatershedApiService _apiService;

    public DataController(WatershedApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            string xAPIData = await _apiService.GetXAPIDataAsync();
            // Process the xAPI data or pass it to the view
            return View("Index", xAPIData);
        }
        catch (Exception ex)
        {
            // Handle errors
            return View("Error", ex.Message);
        }
    }
}
