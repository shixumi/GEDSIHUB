using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class WatershedApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private readonly string _wsKey;
    private readonly string _wsSecret;
    private readonly string _orgId;

    // Constructor to initialize the API credentials and organization ID
    public WatershedApiService(string wsKey, string wsSecret, string orgId)
    {
        _wsKey = wsKey;
        _wsSecret = wsSecret;
        _orgId = orgId;

        // Configure HttpClient (this is done once per instance)
        var byteArray = Encoding.ASCII.GetBytes($"{_wsKey}:{_wsSecret}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // Method to make the API call
    public async Task<string> GetXAPIDataAsync()
    {
        string url = $"https://watershedlrs.com/api/organizations/{_orgId}/query/export?type=json";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"API call failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (logging, re-throwing, etc.)
            throw new Exception("An error occurred while fetching xAPI data.", ex);
        }
    }
}
