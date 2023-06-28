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
            await Clients.Group(primaryPlayer.LobbyCode).SendAsync(ServerCallbacks.SetPrimaryPlayerName, primaryPlayer);
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

        public async Task VotingBallot(Player votingPlayer, string primaryPlayerConnectionId, BallotType ballotCard)
        {
            await Clients.Client(primaryPlayerConnectionId).SendAsync(ServerCallbacks.VotingBallotName, votingPlayer, ballotCard);
        }

        public async Task ChancellorVoting(Player player)
        {
            await Clients.Group(player.LobbyCode).SendAsync(ServerCallbacks.ChancellorVotingName, player);
        }

        public async Task ChancellorSelected(Player currentChancellor)
        {
            await Clients.Group(currentChancellor.LobbyCode).SendAsync(ServerCallbacks.ChancellorSelectedName, currentChancellor);
        }

        public async Task PlayerSelectionStatus(string lobbyCode, PlayerSelectionStatus status)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.PlayerSelectionStatusName, status);
        }


        public async Task PresidentSelected(Player currentPresident)
        {
            await Clients.Group(currentPresident.LobbyCode).SendAsync(ServerCallbacks.PresidentSelectedName, currentPresident);
        }

        public async Task SendElectionVote(string lobbyCode, Player player, bool vote)
        {
            await Clients.Group(lobbyCode).SendAsync(ServerCallbacks.ElectionVoteName, player, vote);
        }
    }
}
