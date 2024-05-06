using Microsoft.AspNetCore.SignalR;

namespace Chatapp.Core.WebSocket.Providers
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(x => x.Type == "id")?.Value;
        }
    }
}
