// Services/CustomUserIdProvider.cs

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GedsiHub.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Use the user's unique identifier (e.g., ClaimTypes.NameIdentifier)
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
