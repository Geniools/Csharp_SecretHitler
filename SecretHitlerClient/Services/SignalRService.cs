
using Microsoft.AspNetCore.SignalR.Client;
using SecretHitler.Model;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        private readonly string _baseUrl = "http://localhost";
        private readonly HubConnection _hubConnection;

        public event Action<string> UpdateUsername;

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

        public void JoinGame(Player player)
        {
            // Invoke the server method to join the game
            this._hubConnection.InvokeAsync("JoinGame", player);
        }

        public void SendMessageToAll(string username, string lobbyCode)
        {
            // Invoke the server method to send a message to all players
            this._hubConnection.SendAsync("SendMessageToAll", username, lobbyCode);
        }

        public void StartGame()
        {
            // Invoke the server method to start the game
            this._hubConnection.InvokeAsync("StartGame");
        }

        private void RegisterEventHandlers()
        {
            this._hubConnection.On<string, string>("ReceiveMessage", (username, lobbyCode) =>
            {
                this.UpdateUsername?.Invoke(username);
            });

            // Register event handlers for server callbacks
            this._hubConnection.On("GameStarted", GameStarted);
            this._hubConnection.On<string>("GameEnded", GameEnded);
        }

        private void GameStarted()
        {
            // Handle the "GameStarted" event received from the server
        }

        private void GameEnded(string winner)
        {
            // Handle the "GameEnded" event received from the server
        }
    }
}
