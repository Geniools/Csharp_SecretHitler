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
        private readonly HubConnection _hubConnection;

        // Events
        public event Action<Player> PlayerConnected;


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
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/{hubName}")
                .Build();
        }

        private async Task StartConnection()
        {
            // This function must contain all event handlers
            // Handle the PlayerConnected event
            this._hubConnection.On<PlayerShared>(PlayerConnectedName, playerShared =>
            {
                Player player = new Player(playerShared.Username);
                this.PlayerConnected?.Invoke(player);
            });

            //Start the connection
            //Shell.Current.Dispatcher.Dispatch(async () =>
            //{
            //    await this._hubConnection.StartAsync();
            //});

            await this._hubConnection.StartAsync();
        }
        
        internal async Task JoinLobby(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await this._hubConnection.SendAsync(PlayerConnectedName, player);
        }

        internal async Task CreateLobby(string username, string lobbyCode)
        {
            await this.StartConnection();
            PlayerShared player = new PlayerShared(username, lobbyCode);
            await _hubConnection.SendAsync(PlayerConnectedName, player);
        }
    }
}
