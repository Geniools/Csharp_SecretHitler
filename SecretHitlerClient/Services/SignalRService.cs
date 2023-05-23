using Microsoft.AspNetCore.SignalR.Client;

using Player = SecretHitlerShared.Player;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        // Names of server callbacks
        private const string PlayerConnectedName = "PlayerConnected";
        private const string SessionStartedName = "SessionStarted";

        private readonly string _baseUrl = "http://localhost";
        private readonly HubConnection _hubConnection;

        public event Action<Player> PlayerConnected;

        public SignalRService()
        {
            // Android can't connect to localhost
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                this._baseUrl = "http://10.0.2.2";
            }

            // Create the connection
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl($"{this._baseUrl}:5142/gameHub")
                .Build();

            // This function must contain all event handlers
            this.RegisterEventHandlers();

            // Start the connection
            this._hubConnection.StartAsync();
        }
        private void RegisterEventHandlers()
        {
            // Register event handlers for server callbacks
            this._hubConnection.On("GameStarted", GameStarted);
            this._hubConnection.On<string>("GameEnded", GameEnded);

            // Handle the PlayerConnected event
            this._hubConnection.On<Player>(PlayerConnectedName, (player) =>
            {
                this.PlayerConnected?.Invoke(player);
            });
        }

        public void StartGame()
        {
            // Invoke the server method to start the game
            this._hubConnection.InvokeAsync("StartGame");
        }

        private void GameStarted()
        {
            throw new NotImplementedException();
        }

        private void GameEnded(string winner)
        {
            throw new NotImplementedException();
        }

        internal void JoinLobby(string username, string lobbyCode)
        {
            Player player = new Player(username, lobbyCode);
            this._hubConnection.SendAsync("PlayerConnect", player);
        }

        internal void CreateLobby(string username, string lobbyCode)
        {
            Player player = new Player(username, lobbyCode);
            this._hubConnection.SendAsync("PlayerConnect", player);
        }
    }
}
