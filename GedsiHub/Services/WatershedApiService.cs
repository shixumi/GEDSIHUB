// Services/WatershedApiService.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Add this line
using GedsiHub.Models; // Ensure this is included
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GedsiHub.Services
{
    public class WatershedApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _orgId;
        private readonly ILogger<WatershedApiService> _logger;

        // Constructor to initialize the API credentials and organization ID
        public WatershedApiService(HttpClient httpClient, IConfiguration configuration, ILogger<WatershedApiService> logger)
        {
            _httpClient = httpClient;
            _orgId = configuration["LRS:OrganizationId"]; // Add OrganizationId to configuration
            _logger = logger;

            var wsKey = configuration["LRS:Key"];
            var wsSecret = configuration["LRS:Secret"];
            var byteArray = Encoding.ASCII.GetBytes($"{wsKey}:{wsSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Method to make the API call
        public async Task<LrsResponse> GetXAPIDataAsync()
        {
            string url = $"https://watershedlrs.com/api/organizations/{_orgId}/query/export?type=json";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LrsResponse>(json);
                }
                else
                {
                    _logger.LogError($"API call failed with status code: {response.StatusCode}");
                    throw new Exception($"API call failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch xAPI data from Watershed.");
                throw new Exception("An error occurred while fetching xAPI data.", ex);
            }
        }
    }
}
