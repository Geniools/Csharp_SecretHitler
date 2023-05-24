using Microsoft.AspNetCore.SignalR;
using SecretHitlerShared;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task PlayerConnected(PlayerShared player)
        {
            // Add the player to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, player.LobbyCode);

            // Notify other players that a player has connected
            await Clients.Group(player.LobbyCode).SendAsync("PlayerConnected", player.Username);
        }
    }
}
