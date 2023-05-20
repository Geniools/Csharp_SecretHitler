using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessageToAll(string username, string lobbyCode)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, lobbyCode);
        }
    }
}
