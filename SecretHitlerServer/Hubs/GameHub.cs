﻿using Microsoft.AspNetCore.SignalR;
using SecretHitlerShared;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task PlayerConnected(PlayerShared connectingPlayer)
        {
            // Add the player to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, connectingPlayer.LobbyCode);
            await this.ConnectPlayer(connectingPlayer);
        }

        public async Task ConnectPlayer(PlayerShared connectingPlayer)
        {
            // Notify other players that a player has connected
            await Clients.Group(connectingPlayer.LobbyCode).SendAsync(ServerCallbacks.PlayerConnectedName, connectingPlayer);
        }

        //public async Task PlayerDisconnected(PlayerShared disconnectingPlayer, string? message = null)
        //{
        //    // Remove the player from the group
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, disconnectingPlayer.LobbyCode);
        //    await this.DisconnectPlayer(disconnectingPlayer, message);
        //}

        public async Task DisconnectPlayer(PlayerShared disconnectingPlayer, string? message = null)
        {
            // Notify other players that a player has disconnected
            await Clients.Group(disconnectingPlayer.LobbyCode).SendAsync(ServerCallbacks.DisconnectPlayerName, disconnectingPlayer, message);
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
