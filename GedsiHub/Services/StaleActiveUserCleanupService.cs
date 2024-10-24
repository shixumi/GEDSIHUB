// Services/StaleActiveUserCleanupService.cs

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GedsiHub.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GedsiHub.Services
{
    public class StaleActiveUserCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StaleActiveUserCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _staleThreshold = TimeSpan.FromMinutes(60); // Define as needed

        public StaleActiveUserCleanupService(IServiceProvider serviceProvider, ILogger<StaleActiveUserCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StaleActiveUserCleanupService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_cleanupInterval, stoppingToken);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var cutoffTime = DateTime.UtcNow - _staleThreshold;
                        var staleActiveUsers = await context.ActiveUsers
                            .Where(au => au.LastActive < cutoffTime)
                            .ToListAsync(stoppingToken);

                        if (staleActiveUsers.Any())
                        {
                            context.ActiveUsers.RemoveRange(staleActiveUsers);
                            await context.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation($"Removed {staleActiveUsers.Count} stale ActiveUser entries.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during stale ActiveUser cleanup.");
                }
            }

            _logger.LogInformation("StaleActiveUserCleanupService is stopping.");
        }
    }
}
