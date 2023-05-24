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

        private readonly string _baseUrl;
        private readonly HubConnection _hubConnection;

        public event Action<Player> PlayerConnected;


        public SignalRService(string baseUrl)
        {
            this._baseUrl = baseUrl;

            // Android can't connect to localhost
            if (baseUrl.Equals("http://localhost") && DeviceInfo.Platform == DevicePlatform.Android)
            {
                this._baseUrl = "http://10.0.2.2";
            }

            // Create the connection
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl($"{this._baseUrl}:5142/gameHub")
                .Build();
        }

        public SignalRService() : this("http://localhost"){}

        private async Task StartConnection()
        {
            // This function must contain all event handlers
            // Handle the PlayerConnected event
            this._hubConnection.On<PlayerShared>(PlayerConnectedName, playerShared =>
            {
                Player player = new Player(playerShared.Username);
                this.PlayerConnected?.Invoke(player);
            });

            // Start the connection
            await this._hubConnection.StartAsync();
        }
        
        internal async Task JoinLobby(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await this._hubConnection.SendAsync("PlayerConnect", player);
        }

        internal async Task CreateLobby(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await _hubConnection.SendAsync("PlayerConnect", player);
        }
    }
}
