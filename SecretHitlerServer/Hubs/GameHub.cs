using Microsoft.AspNetCore.SignalR;
using SecretHitlerShared;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task PlayerConnected(Player connectingPlayer)
        {
            // Add the player to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, connectingPlayer.LobbyCode);
            // Set the connection ID of the player
            await this.ConnectPlayer(connectingPlayer);
        }

        public async Task ConnectPlayer(Player connectingPlayer)
        {
            // Notify other players that a player has connected
            await Clients.OthersInGroup(connectingPlayer.LobbyCode).SendAsync(ServerCallbacks.PlayerConnectedName, connectingPlayer);
        }

        public async Task SetPrimaryPlayer(Player primaryPlayer)
        {
            await Clients.OthersInGroup(primaryPlayer.LobbyCode).SendAsync(ServerCallbacks.SetPrimaryPlayerName, primaryPlayer);
        }

        public async Task PlayerDisconnected(Player disconnectedPlayer)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, disconnectedPlayer.LobbyCode);
        }

        public async Task DisconnectPlayer(Player disconnectingPlayer, string? message = null)
        {
            // Notify other players that a player has disconnected
            await Clients.Client(disconnectingPlayer.ConnectionId).SendAsync(ServerCallbacks.DisconnectPlayerName, disconnectingPlayer, message);
        }

        public async Task StartGame(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.StartGameName);
        }

        public async Task ClearAllPlayers(string lobbyCode)
        {
            await Clients.OthersInGroup(lobbyCode).SendAsync(ServerCallbacks.ClearAllPlayersName);
        }

        public async Task EndGame(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.EndGameName);
        }

        public async Task SessionStarted(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.SessionStartedName);
        }

        public async Task SendChatMessage(string lobbyCode, Player player, string message)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.ChatMessageName, player, message);
        }

        public async Task SendElectionVote(string lobbyCode, Player player, bool vote)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.ElectionVoteName, player, vote);
        }
    }
}
