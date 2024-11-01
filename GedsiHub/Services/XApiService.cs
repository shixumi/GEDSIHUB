// Services/XApiService.cs
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using GedsiHub.Hubs;
using Microsoft.EntityFrameworkCore;

public class XApiService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<XApiService> _logger;
    private readonly IHubContext<AnalyticsHub> _hubContext;

    public XApiService(ApplicationDbContext context, ILogger<XApiService> logger, IHubContext<AnalyticsHub> hubContext)
    {
        _context = context;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task ProcessAndStoreStatementAsync(string xApiJson)
    {
        string userId = null;
        int moduleId = 0;
        string verb = null;

        try
        {
            JObject xApiStatement = JObject.Parse(xApiJson);

            // Map to XApiStatement model
            var statement = new XApiStatement
            {
                Timestamp = xApiStatement["timestamp"]?.Value<DateTime>() ?? DateTime.UtcNow,
                ActorName = xApiStatement["actor"]?["name"]?.ToString(),
                ActorMbox = xApiStatement["actor"]?["mbox"]?.ToString(),
                ActorAccountHomePage = xApiStatement["actor"]?["account"]?["homePage"]?.ToString(),
                ActorAccountName = xApiStatement["actor"]?["account"]?["name"]?.ToString(),
                VerbId = xApiStatement["verb"]?["id"]?.ToString(),
                VerbDisplay = xApiStatement["verb"]?["display"]?["en-US"]?.ToString(),
                ObjectId = xApiStatement["object"]?["id"]?.ToString(),
                ObjectDefinitionName = xApiStatement["object"]?["definition"]?["name"]?["en-US"]?.ToString(),
                ObjectDefinitionDescription = xApiStatement["object"]?["definition"]?["description"]?["en-US"]?.ToString(),
                ResultScore = xApiStatement["result"]?["score"]?["raw"]?.Value<double>(),
                ResultSuccess = xApiStatement["result"]?["success"]?.Value<bool>(),
                ContextRegistration = xApiStatement["context"]?["registration"]?.ToString(),
                AdditionalData = xApiStatement.ToString()
            };

            _context.XApiStatements.Add(statement);
            await _context.SaveChangesAsync();

            // Map to UserActivity based on verb
            userId = xApiStatement["actor"]?["account"]?["name"]?.ToString();
            moduleId = ExtractModuleIdFromObjectId(statement.ObjectId); // Implement this method based on your Object ID structure
            verb = statement.VerbDisplay.ToLower();

            if (userId != null && moduleId > 0)
            {
                var userActivity = new UserActivity
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    ActivityType = verb,
                    Score = statement.ResultScore,
                    Success = statement.ResultSuccess,
                    Timestamp = statement.Timestamp
                };

                _context.UserActivities.Add(userActivity);
                await _context.SaveChangesAsync();

                // Handle certificate issuance if necessary
                if (verb.Contains("completed") && statement.ResultSuccess == true)
                {
                    var certificate = new Certificate
                    {
                        UserId = userId,
                        ModuleId = moduleId,
                        IssueDate = DateTime.UtcNow // Corrected property name
                    };

                    _context.Certificates.Add(certificate);
                }

                await _context.SaveChangesAsync();
                // After storing data, notify dashboards
                await _hubContext.Clients.All.SendAsync("ReceiveAnalyticsUpdate", new { ModuleId = moduleId, UserId = userId, ActivityType = verb });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process and store xAPI statement.");
            // Handle exception as needed
        }
    }

    private int ExtractModuleIdFromObjectId(string objectId)
    {
        // Implement logic to extract ModuleId from objectId
        // This depends on how you structure your xAPI object IDs
        // Example: "https://yourlms.com/modules/123" => 123
        if (Uri.TryCreate(objectId, UriKind.Absolute, out Uri uri))
        {
            var segments = uri.Segments;
            if (segments.Length >= 2 && int.TryParse(segments[segments.Length - 1], out int moduleId))
            {
                return moduleId;
            }
        }
        return 0;
    }
}
