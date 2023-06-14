using SecretHitler.Services;
using System.Collections.ObjectModel;
using SecretHitler.Views;
using Microsoft.AspNetCore.SignalR.Client;
using SecretHitlerShared;

namespace SecretHitler
{
    public class GameManager : BindableObject
    {
        // Services
        public SignalRService SignalRService { get; private set; }

        // Game specific properties
        public bool IsPrimary { get; set; }
        public bool GameStarted { get; set; }

        // Game state
        public ObservableCollection<Player> Players { get; private set; }
        public Board Board { get; private set; }
        public Chat Chat { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public Player Winner { get; private set; }
        public Player CurrentPresident { get; private set; }
        public Player CurrentChancelor { get; private set; }
        public byte ElectionTracker { get; private set; }

        public GameManager()
        {
            this.SignalRService = new SignalRService("gameHub", "https://secrethitler.azurewebsites.net");
            // Subscribe to events
            SignalRService.PlayerConnected += this.AddPlayer;
            SignalRService.PlayerDisconnected += this.DisconnectPlayer;
            SignalRService.GameStarted += this.StartLocalGame;
            SignalRService.ClearAllPlayers += this.ClearAllPlayers;

            // Create a new game
            this.Board = new Board();
            this.Players = new ObservableCollection<Player>();
            this.Chat = new Chat();
            this.GameStatus = new GameStatus();
            this.ElectionTracker = 0;
        }

        // Event handlers / Pre game logic =====================================================================================

        private async void AddPlayer(Player player)
        {
            await Shell.Current.Dispatcher.DispatchAsync(() =>
            {
                Dictionary<int, string> errorMessages = new Dictionary<int, string>()
                {
                    { 1, "Username already exists" },
                    { 2, "The lobby is full" },
                    { 3, "The game has already started" }
                };
                int errorCode = 1;

                if (this.ContainsUsername(player) is null)
                {
                    errorCode = 2;
                    if (this.Players.Count <= 10)
                    {
                        errorCode = 3;
                        if (!this.GameStarted)
                        {
                            errorCode = -1;
                            this.Players.Add(player);
                        }
                    }
                }

                if(errorCode != -1)
                {
                    if (this.IsPrimary)
                    {
                        string message = errorMessages[errorCode];
                        this.SignalRService.HubConnection.SendAsync(ServerCallbacks.DisconnectPlayerName, player, message);
                    }
                }
            });
        }

        private async void DisconnectPlayer(string message)
        {
            await Shell.Current.Dispatcher.DispatchAsync(async () =>
            {
                this.SignalRService.CurrentPlayer = null;

                // Return to the start page
                await Shell.Current.GoToAsync("..");

                // Display the message
                await Shell.Current.DisplayAlert("Disconnected", message, "OK");

                // Stop the connection to the hub
                await this.SignalRService.HubConnection.DisposeAsync();
                await this.SignalRService.HubConnection.StopAsync();

                //await this.SignalRService.DisconnectPlayer();
            });
        }

        private async void StartLocalGame()
        {
            this.GameStarted = true;
            await Shell.Current.Dispatcher.DispatchAsync(async () => 
                await Shell.Current.GoToAsync(nameof(MainPage))
            );
        }

        private void ClearAllPlayers()
        {
            if (!this.IsPrimary)
            {
                this.Players.Clear();
            }
        }

        // Game logic ==========================================================================================================

        private Task EndGame()
        {
            throw new NotImplementedException();
        }

        private Player RunNextElection()
        {
            throw new NotImplementedException();
        }

        private Player SetNextPresident()
        {
            throw new NotImplementedException();
        }

        // Helper functions / Utility functions ===============================================================================

        /// <summary>
        /// Will return the first player having the given username, null otherwise
        /// </summary>
        /// <param name="username">The username of the player to be searched for</param>
        /// <returns>The first player having the given username, null otherwise</returns>
        private Player ContainsUsername(Player comparedPlayer)
        {
            // Create a copy of the players list to avoid concurrency issues
            List<Player> players = new List<Player>(this.Players);

            foreach (Player player in players)
            {
                if (comparedPlayer.Equals(player))
                {
                    return player;
                }
            }

            return null;
        }
    }
}
