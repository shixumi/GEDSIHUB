// Middleware/XApiEnrichmentMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using GedsiHub.Services;

public class XApiEnrichmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<XApiEnrichmentMiddleware> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _lrsEndpoint;
    private readonly string _lrsKey;
    private readonly string _lrsSecret;

    public XApiEnrichmentMiddleware(RequestDelegate next, ILogger<XApiEnrichmentMiddleware> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _logger = logger;
        _lrsEndpoint = configuration["LRS:Endpoint"];
        _lrsKey = configuration["LRS:Key"];
        _lrsSecret = configuration["LRS:Secret"];
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Equals("/xapi/submit", StringComparison.OrdinalIgnoreCase) && context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Intercepting xAPI statement for enrichment.");

            try
            {
                // Resolve XApiService from the scoped HttpContext
                var xApiService = context.RequestServices.GetRequiredService<XApiService>();

                // Read the incoming xAPI statement
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
                string body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                // Parse the xAPI statement
                JObject xApiStatement = JObject.Parse(body);

                // Extract userID from the authenticated user
                string userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.LogWarning("Authenticated user email not found. Cannot enrich xAPI statement with mbox.");
                }

                string homePage = "gedsihub.com"; // Define your homePage accordingly

                // Append userID to the xAPI statement's actor
                xApiStatement["actor"] = new JObject
                {
                    { "objectType", "Agent" },
                    { "name", context.User.Identity.Name },
                    { "mbox", $"mailto:{userEmail}" },
                    { "account", new JObject
                        {
                            { "homePage", homePage },
                            { "name", userEmail }
                        }
                    }
                };

                // Forward the enriched statement to the LRS
                var httpClient = _httpClientFactory.CreateClient();
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_lrsKey}:{_lrsSecret}"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var content = new StringContent(xApiStatement.ToString(), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(_lrsEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("xAPI statement forwarded to LRS successfully.");
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync("xAPI statement received and processed.");
                }
                else
                {
                    _logger.LogError($"Failed to forward xAPI statement to LRS. Status Code: {response.StatusCode}");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Failed to process xAPI statement.");
                }

                // Since we've handled the request, we can return a response
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing xAPI statement.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An internal server error occurred.");
                return;
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
