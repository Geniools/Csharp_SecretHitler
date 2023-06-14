﻿using Microsoft.AspNetCore.SignalR.Client;
using SecretHitlerShared;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        // Connection to the server
        public HubConnection HubConnection { get; private set; }

        // Events
        public event Action<Player> PlayerConnected;
        public event Action<string> PlayerDisconnected;
        public event Action GameStarted;
        public event Action ClearAllPlayers;

        // Other properties
        internal Player CurrentPlayer { get; set; }

        public SignalRService(string hubName, string baseUrl = "http://localhost", int portNr = 80)
        {
            // Android can't connect to localhost (for some reason)
            if (baseUrl.Equals("http://localhost") && DeviceInfo.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }

            if (portNr != 80 && portNr != 443)
            {
                baseUrl += $":{portNr}";
            }

            // Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/{hubName}")
                .Build();
        }

        private async Task StartConnection()
        {
            // This function must contain all event handlers
            // Handle the PlayerConnected event
            this.HubConnection.On<Player>(ServerCallbacks.PlayerConnectedName, connectedPlayer =>
            {
                this.PlayerConnected?.Invoke(connectedPlayer);
            });

            this.HubConnection.On<string>(ServerCallbacks.DisconnectPlayerName, (message) =>
            {
                this.PlayerDisconnected?.Invoke(message);
            });

            // Handle the GameStarted event
            this.HubConnection.On(ServerCallbacks.StartGameName, () =>
            {
                this.GameStarted?.Invoke();
            });

            this.HubConnection.On(ServerCallbacks.ClearAllPlayersName, () =>
            {
                this.ClearAllPlayers?.Invoke();
            });

            this.HubConnection.On<string>(ServerCallbacks.GetConnectionIdName, (connectionId) =>
            {
                this.CurrentPlayer.ConnectionId = connectionId;
            });

            //Start the connection
            await this.HubConnection.StartAsync();

            // Get the connection id
            await this.HubConnection.InvokeAsync<string>(ServerCallbacks.GetConnectionIdName);
        }
        
        internal async Task ConnectPlayer(Player player)
        {
            this.CurrentPlayer = player;
            await this.StartConnection();
            await this.HubConnection.SendAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        internal async Task DisconnectPlayer()
        {
            await this.HubConnection.SendAsync(ServerCallbacks.DisconnectPlayerName, this.CurrentPlayer);
        }

        internal async Task StartOnlineGame(List<Player> connectedPlayers)
        {
            // Clear all players from other clients
            await this.HubConnection.SendAsync(ServerCallbacks.ClearAllPlayersName, this.CurrentPlayer.LobbyCode);

            // Notify all players of the other connected players
            foreach (Player player in connectedPlayers)
            {
                await this.HubConnection.SendAsync(ServerCallbacks.ConnectPlayerName, player);
            }

            if (!string.IsNullOrEmpty(this.CurrentPlayer.LobbyCode))
            {
                await this.HubConnection.SendAsync(ServerCallbacks.StartGameName, this.CurrentPlayer.LobbyCode);
            }
        }
    }
}
