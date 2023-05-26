using Microsoft.AspNetCore.SignalR.Client;
using SecretHitler.Model;
using SecretHitlerShared;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        // Names of server callbacks
        private const string PlayerConnectedName = "PlayerConnected";
        private const string SessionStartedName = "SessionStarted";
        private const string StartGameName = "StartGame";
        private const string EndGameName = "EndGame";
        private const string ChatMessageName = "ChatMessage";
        private const string ElectionVoteName = "ElectionVote";

        // Connection to the server
        public HubConnection HubConnection { get; }

        // Events
        public event Action<Player> PlayerConnected;
        public event Action GameStarted;

        // Other properties
        public string LobbyCode { get; private set; }



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
            this.HubConnection.On<PlayerShared>(PlayerConnectedName, playerShared =>
            {
                Player player = new Player(playerShared.Username);
                this.PlayerConnected?.Invoke(player);

                if (string.IsNullOrEmpty(this.LobbyCode))
                {
                    this.LobbyCode = playerShared.LobbyCode;
                }
            });

            // Handle the GameStarted event
            this.HubConnection.On(StartGameName, () =>
            {
                this.GameStarted?.Invoke();
            });

            //Start the connection
            await this.HubConnection.StartAsync();
        }
        
        internal async Task ConnectPlayer(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await this.HubConnection.SendAsync(PlayerConnectedName, player);
        }

        internal async Task StartOnlineGame()
        {
            if (!string.IsNullOrEmpty(this.LobbyCode))
            {
                await this.HubConnection.SendAsync(StartGameName, this.LobbyCode);
            }
        }
    }
}
