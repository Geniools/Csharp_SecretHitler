using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using SecretHitlerShared;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        // Connection to the server
        public HubConnection HubConnection { get; private set; }
        private readonly string _baseUrl;
        private readonly string _hubName;

        // Events
        public event Action<Player> PlayerConnected;
        public event Action<string> PlayerDisconnected;
        public event Action GameStarted;
        public event Action ClearAllPlayers;

        // Other properties
        internal Player ThisPlayer { get; set; }
        internal Player MasterPlayer { get; set; }

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

            this._baseUrl = baseUrl;
            this._hubName = hubName;

            // Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/{hubName}")
                .Build();
        }

        /// <summary>
        /// Will create (or recreate) the connection to the server, subscribe to events and start the connection
        /// </summary>
        /// <returns></returns>
        private async Task StartConnection()
        {
            // (Re)Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{this._baseUrl}/{this._hubName}")
                .Build();

            // This function must contain all event handlers
            // Handle the PlayerConnected event
            this.HubConnection.On<Player>(ServerCallbacks.PlayerConnectedName, connectedPlayer =>
            {
                this.PlayerConnected?.Invoke(connectedPlayer);
            });

            // Will be called when a player disconnects (this player) - will be triggered by the "primary" client
            this.HubConnection.On<Player, string>(ServerCallbacks.DisconnectPlayerName, (disconnectingPlayer, message) =>
            {
                //Console.WriteLine($"Player {disconnectingPlayer.ConnectionId} disconnected: {message}");
                this.PlayerDisconnected?.Invoke(message);
            });

            // Before starting a new game, clear all players from other clients
            this.HubConnection.On(ServerCallbacks.ClearAllPlayersName, () =>
            {
                this.ClearAllPlayers?.Invoke();
            });

            // Handle the GameStarted event
            this.HubConnection.On(ServerCallbacks.StartGameName, () =>
            {
                this.GameStarted?.Invoke();
            });

            // Set the default connection limit
            ServicePointManager.DefaultConnectionLimit = 10;

            //Start the connection
            await this.HubConnection.StartAsync();

            // Get the connection id
            this.ThisPlayer.ConnectionId = this.HubConnection.ConnectionId;
        }
        
        internal async Task ConnectPlayer(Player player)
        {
            this.ThisPlayer = player;
            // The connection is (re)created every time to avoid errors if the user navigates back to the main page
            await this.StartConnection();

            await this.HubConnection.InvokeAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        internal async Task StartOnlineGame(List<Player> connectedPlayers)
        {
            // Clear all players from other clients
            await this.HubConnection.InvokeAsync(ServerCallbacks.ClearAllPlayersName, this.ThisPlayer.LobbyCode);

            // Start the game
            if (!string.IsNullOrEmpty(this.ThisPlayer.LobbyCode))
            {
                await this.HubConnection.InvokeAsync(ServerCallbacks.StartGameName, this.ThisPlayer.LobbyCode);
            }
        }

        internal async Task SendFinalPlayingPlayers(List<Player> finalPlayers)
        {
            // Notify all players of the other connected players
            foreach (Player player in finalPlayers)
            {
                await this.HubConnection.InvokeAsync(ServerCallbacks.ConnectPlayerName, player);
            }
        }
    }
}
