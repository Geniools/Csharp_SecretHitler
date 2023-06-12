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
            await this.ConnectPlayer(player);
        }

        public async Task ConnectPlayer(PlayerShared player)
        {
            // Notify other players that a player has connected
            await Clients.Group(player.LobbyCode).SendAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        public async Task StartGame(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.StartGameName);
        }

        public async Task EndGame(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.EndGameName);
        }

        public async Task SessionStarted(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.SessionStartedName);
        }

        public async Task SendChatMessage(string lobbyCode, string username, string message)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.ChatMessageName, username, message);
        }

        public async Task SendElectionVote(string lobbyCode, string username, bool vote)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.ElectionVoteName, username, vote);
        }
    }
}
