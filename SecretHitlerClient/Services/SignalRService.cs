using Microsoft.AspNetCore.SignalR.Client;
using SecretHitler.Model;
using SecretHitlerShared;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        // Connection to the server
        public HubConnection HubConnection { get; private set; }

        // Events
        public event Action<Player> PlayerConnected;
        public event Action<Player, string> PlayerDisconnected;
        public event Action GameStarted;
        public event Action ClearAllPlayers;

        // Other properties
        internal string LobbyCode { get; set; }
        internal string PlayerUsername { get; set; }

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
            this.HubConnection.On<PlayerShared>(ServerCallbacks.PlayerConnectedName, playerShared =>
            {
                Player player = new Player(playerShared.Username);
                this.PlayerConnected?.Invoke(player);

                if (string.IsNullOrEmpty(this.LobbyCode))
                {
                    this.LobbyCode = playerShared.LobbyCode;
                }
            });

            this.HubConnection.On<PlayerShared, string>(ServerCallbacks.DisconnectPlayerName, (disconnectingPlayer, message) =>
            {
                Player player = new Player(disconnectingPlayer.Username);
                this.PlayerDisconnected?.Invoke(player, message);
            });

            // Handle the GameStarted event
            this.HubConnection.On(ServerCallbacks.StartGameName, () =>
            {
                this.GameStarted?.Invoke();
            });

            this.HubConnection.On(ServerCallbacks.ClearAllPlayersName, () =>
            {
                
            });

            //Start the connection
            await this.HubConnection.StartAsync();
        }
        
        internal async Task ConnectPlayer(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await this.HubConnection.SendAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        internal async Task DisconnectPlayer(string username, string lobbyCode)
        {
            await this.HubConnection.SendAsync(ServerCallbacks.PlayerDisconnectedName, username, lobbyCode);
        }

        internal async Task StartOnlineGame(List<Player> connectedPlayers)
        {
            // Clear all players from other clients
            await this.HubConnection.SendAsync(ServerCallbacks.ClearAllPlayersName, this.LobbyCode);

            // Notify all players of the other connected players
            foreach (Player player in connectedPlayers)
            {
                PlayerShared playerShared = new PlayerShared(player.Username, this.LobbyCode);
                await this.HubConnection.SendAsync(ServerCallbacks.ConnectPlayerName, playerShared);
            }

            if (!string.IsNullOrEmpty(this.LobbyCode))
            {
                await this.HubConnection.SendAsync(ServerCallbacks.StartGameName, this.LobbyCode);
            }
        }
    }
}
