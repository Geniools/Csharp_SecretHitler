using SecretHitler.Model;
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
        public bool IsPrimaryPlayer { get; set; }

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
            SignalRService.PlayerDisconnected += this.RemovePlayer;
            SignalRService.GameStarted += this.StartLocalGame;
            SignalRService.ClearAllPlayers += this.ClearAllPlayers;

            // Create a new game
            this.Board = new Board();
            this.Players = new ObservableCollection<Player>();
            this.Chat = new Chat();
            this.GameStatus = new GameStatus();
            this.ElectionTracker = 0;
        }

        // Event handlers / Pre game logic
        private void AddPlayer(Player player)
        {
            Shell.Current.Dispatcher.DispatchAsync(() =>
            {
                if (this.ContainsUsername(player.Username) is null && this.Players.Count <= 10)
                {
                    this.Players.Add(player);
                }
                else
                {
                    if (this.IsPrimaryPlayer)
                    {
                        PlayerShared disconnectedPlayer = new PlayerShared(player.Username, this.SignalRService.LobbyCode);

                        string message = "Username already exists or the lobby is full";
                        this.SignalRService.HubConnection.SendAsync(ServerCallbacks.DisconnectPlayerName, disconnectedPlayer, message);
                    }
                }
            });
        }

        private void RemovePlayer(Player player, string message)
        {
            Shell.Current.Dispatcher.DispatchAsync(async () =>
            {
                Player playerToRemove = this.ContainsUsername(player.Username);
                if (playerToRemove is not null)
                {
                    this.Players.Remove(playerToRemove);

                    if (playerToRemove.Username.Equals(this.SignalRService.PlayerUsername.ToLower()))
                    {
                        this.SignalRService.LobbyCode = string.Empty;
                        this.SignalRService.PlayerUsername = string.Empty;

                        await Shell.Current.DisplayAlert("Disconnected", message, "OK");

                        await this.SignalRService.HubConnection.StopAsync();
                        await Shell.Current.GoToAsync(nameof(StartPage));
                    }
                }
            });
        }

        private async void StartLocalGame()
        {
            await Shell.Current.Dispatcher.DispatchAsync(async () => 
                await Shell.Current.GoToAsync(nameof(MainPage))
            );
        }

        private void ClearAllPlayers()
        {
            if (!this.IsPrimaryPlayer)
            {
                this.Players.Clear();
            }
        }

        // Game logic
        private async Task EndGame()
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

        // Helper functions / Utility functions

        /// <summary>
        /// Will return the first player having the given username, null otherwise
        /// </summary>
        /// <param name="username">The username of the player to be searched for</param>
        /// <returns>The first player having the given username, null otherwise</returns>
        private Player ContainsUsername(string username)
        {
            // Create a copy of the players list to avoid concurrency issues
            List<Player> players = new List<Player>(this.Players);

            foreach (Player player in players)
            {
                if (player.Username.Equals(username.ToLower()))
                {
                    return player;
                }
            }

            return null;
        }
    }
}
