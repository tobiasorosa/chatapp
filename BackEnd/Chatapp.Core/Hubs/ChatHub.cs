using Microsoft.AspNetCore.SignalR;

namespace Chatapp.Core.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        Clients.All.SendAsync("ReceiveMessage", user, message);
        // todo enviar para cliente por id
    }
}
