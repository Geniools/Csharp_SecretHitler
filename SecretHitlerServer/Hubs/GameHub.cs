﻿using Microsoft.AspNetCore.SignalR;
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
            connectingPlayer.ConnectionId = Context.ConnectionId;
            await this.ConnectPlayer(connectingPlayer);
        }

        public async Task ConnectPlayer(Player connectingPlayer)
        {
            // Notify other players that a player has connected
            await Clients.OthersInGroup(connectingPlayer.LobbyCode).SendAsync(ServerCallbacks.PlayerConnectedName, connectingPlayer);
        }

        // Get the connection ID of the player
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task DisconnectPlayer(Player disconnectingPlayer, string? message = null)
        {
            // Notify other players that a player has disconnected
            //await Clients.Group(disconnectingPlayer.LobbyCode).SendAsync(ServerCallbacks.DisconnectPlayerName, disconnectingPlayer, message);
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
