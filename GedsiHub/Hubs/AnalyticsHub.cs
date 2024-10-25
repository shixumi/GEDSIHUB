// Hubs/AnalyticsHub.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GedsiHub.Hubs
{
    [Authorize]
    public class AnalyticsHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AnalyticsHub> _logger;

        public AnalyticsHub(ApplicationDbContext context, ILogger<AnalyticsHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"User connected: {userId} with ConnectionId: {Context.ConnectionId}");

                var activeUser = new ActiveUser
                {
                    UserId = userId,
                    ConnectionId = Context.ConnectionId,
                    LastActive = DateTime.UtcNow
                };
                _context.ActiveUsers.Add(activeUser);
                await _context.SaveChangesAsync();

                var activeUsersCount = await _context.ActiveUsers
                    .Select(au => au.UserId)
                    .Distinct()
                    .CountAsync();

                await Clients.All.SendAsync("UpdateActiveUsers", activeUsersCount);
            }
            else
            {
                _logger.LogWarning("UserIdentifier is null or empty during connection.");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"User disconnected: {userId} with ConnectionId: {Context.ConnectionId}");

                var activeUser = await _context.ActiveUsers
                    .FirstOrDefaultAsync(au => au.UserId == userId && au.ConnectionId == Context.ConnectionId);
                if (activeUser != null)
                {
                    _context.ActiveUsers.Remove(activeUser);
                    await _context.SaveChangesAsync();

                    var activeUsersCount = await _context.ActiveUsers
                        .Select(au => au.UserId)
                        .Distinct()
                        .CountAsync();

                    await Clients.All.SendAsync("UpdateActiveUsers", activeUsersCount);
                }
                else
                {
                    _logger.LogWarning($"ActiveUser not found for UserId: {userId} and ConnectionId: {Context.ConnectionId}");
                }
            }
            else
            {
                _logger.LogWarning("UserIdentifier is null or empty during disconnection.");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Heartbeat()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                var activeUser = await _context.ActiveUsers
                    .FirstOrDefaultAsync(au => au.UserId == userId && au.ConnectionId == Context.ConnectionId);

                if (activeUser != null)
                {
                    activeUser.LastActive = DateTime.UtcNow;
                    _context.ActiveUsers.Update(activeUser);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Heartbeat received from UserId: {userId}, ConnectionId: {Context.ConnectionId}");
                }
                else
                {
                    _logger.LogWarning($"ActiveUser not found for Heartbeat from UserId: {userId}, ConnectionId: {Context.ConnectionId}");
                }
            }
        }
    }
}
