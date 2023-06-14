using Microsoft.AspNetCore.SignalR.Client;
using SecretHitlerShared;
using System.Net;

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

            this._baseUrl = baseUrl;
            this._hubName = hubName;

            // Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/{hubName}")
                .Build();
        }

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

            this.HubConnection.On<Player, string>(ServerCallbacks.DisconnectPlayerName, (disconnectingPlayer, message) =>
            {
                //Console.WriteLine($"Player {disconnectingPlayer.ConnectionId} disconnected: {message}");
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

            ServicePointManager.DefaultConnectionLimit = 10;

            //Start the connection
            await this.HubConnection.StartAsync();

            // Get the connection id
            this.CurrentPlayer.ConnectionId = this.HubConnection.ConnectionId;
        }
        
        internal async Task ConnectPlayer(Player player)
        {
            this.CurrentPlayer = player;

            // Check if the connection is already started
            if (this.HubConnection.State != HubConnectionState.Connected)
            {
                await this.StartConnection();
            }

            await this.HubConnection.InvokeAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        internal async Task StartOnlineGame(List<Player> connectedPlayers)
        {
            // Clear all players from other clients
            await this.HubConnection.InvokeAsync(ServerCallbacks.ClearAllPlayersName, this.CurrentPlayer.LobbyCode);

            // Notify all players of the other connected players
            foreach (Player player in connectedPlayers)
            {
                await this.HubConnection.InvokeAsync(ServerCallbacks.ConnectPlayerName, player);
            }

            if (!string.IsNullOrEmpty(this.CurrentPlayer.LobbyCode))
            {
                await this.HubConnection.InvokeAsync(ServerCallbacks.StartGameName, this.CurrentPlayer.LobbyCode);
            }
        }
    }
}
