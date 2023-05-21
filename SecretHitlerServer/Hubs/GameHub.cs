using Microsoft.AspNetCore.SignalR;
using SecretHitlerShared;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessageToAll(string username, string lobbyCode)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, lobbyCode);
        }

        public async Task PlayerConnected(Player player)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, player.Username);
        }
    }
}
