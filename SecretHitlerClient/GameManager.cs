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
        public const string HubName = "gameHub";

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
            // Create the SignalRService
            this.SignalRService = new SignalRService(HubName, "https://secrethitler.azurewebsites.net");
            
            // Subscribe to events
            SignalRService.PlayerConnected += this.AddPlayer;
            SignalRService.PlayerDisconnected += this.DisconnectPlayer;
            SignalRService.ClearAllPlayers += this.ClearAllPlayers;
            SignalRService.GameStarted += this.StartLocalGame;

            // Create a new game
            this.Board = new Board();
            this.Players = new ObservableCollection<Player>();
            this.Chat = new Chat();
            this.GameStatus = new GameStatus();
            this.ElectionTracker = 0;

            // Test commands
            this.AddTestPlayers();
        }

        // Event handlers / Pre game logic =====================================================================================

        private async void AddPlayer(Player player)
        {
            await Shell.Current.Dispatcher.DispatchAsync(async () =>
            {
                // Create a dictionary with error messages
                Dictionary<int, string> errorMessages = new Dictionary<int, string>()
                {
                    { 1, "Username already exists" },
                    { 2, "The lobby is full" },
                    { 3, "The game has already started" }
                };

                // Based on the possible error, increase the error code (while doing all the checks)
                int errorCode = 1;
                // Check if the username already exists
                if (!this.ContainsUsername(player))
                {
                    errorCode = 2;
                    // Check if the lobby is full
                    if (this.Players.Count <= 10)
                    {
                        errorCode = 3;
                        // Check if the game has already started
                        if (!this.GameStarted)
                        {
                            errorCode = -1;
                            this.Players.Add(player);
                        }
                    }
                }

                // If there was an error, disconnect the player (with the explenatory message)
                if(errorCode != -1)
                {
                    if (this.IsPrimary)
                    {
                        string message = errorMessages[errorCode];
                        await this.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.DisconnectPlayerName, player, message);
                    }
                }
            });
        }

        private async void DisconnectPlayer(string message)
        {
            this.SignalRService.ThisPlayer = null;

            // Stop the connection to the hub
            //await this.SignalRService.HubConnection.StopAsync();
            await this.SignalRService.HubConnection.DisposeAsync();

            // When interacting with the UI, use the dispatcher (has something to do with Threading...)
            await Shell.Current.Dispatcher.DispatchAsync(async () =>
            {
                // Display the message
                await Shell.Current.DisplayAlert("Disconnected", message, "OK");

                // Return to the start page
                await Shell.Current.GoToAsync("..");
            });
        }

        private async void StartLocalGame()
        {
            // Will prevent other players from joining
            this.GameStarted = true;

            if(this.IsPrimary)
            {
                // Randomly assign roles to players
                List<Player> finalPlayers = this.AssignRandomRolesToPlayers();
                this.Players = new ObservableCollection<Player>(finalPlayers);

                //await this.SignalRService.SendFinalPlayingPlayers(finalPlayers);
            }

            // When interacting with the UI, use the dispatcher (has something to do with Threading...)
            await Shell.Current.Dispatcher.DispatchAsync(async () => 
                await Shell.Current.GoToAsync(nameof(MainPage))
            );
        }

        private List<Player> AssignRandomRolesToPlayers()
        {
            Random randomGenerator = new Random();

            // Create a copy of the players list to avoid concurrency issues
            List<Player> players = new List<Player>(this.Players);

            // Create a list of roles
            List<Player> assignedPlayers = new List<Player>();
            int liberalsNeeded = 0;
            int fascistNeeded = 0;

            // Determine the number of liberals and fascist needed for the game (based on the amount of players)
            switch (this.Players.Count)
            {
                case 5:
                    liberalsNeeded = 3;
                    fascistNeeded = 1;
                    break;
                case 6:
                    liberalsNeeded = 4;
                    fascistNeeded = 1;
                    break;
                case 7:
                    liberalsNeeded = 4;
                    fascistNeeded = 2;
                    break;
                case 8:
                    liberalsNeeded = 5;
                    fascistNeeded = 2;
                    break;
                case 9:
                    liberalsNeeded = 5;
                    fascistNeeded = 3;
                    break;
                case 10:
                    liberalsNeeded = 6;
                    fascistNeeded = 3;
                    break;
                default:
                    throw new NotSupportedException($"The amount of players is not supported. Player range must be between 5 and 10. Players given: {this.Players.Count}");
            }

            int liberals = 0;
            int fascist = 0;

            // Assign the liberals
            while (liberals < liberalsNeeded)
            {
                Player randomPlayer = players[randomGenerator.Next(this.Players.Count)];

                if (!assignedPlayers.Contains(randomPlayer))
                {
                    randomPlayer.Role = SecretRole.Liberal;
                    randomPlayer.Party = PartyMembership.Liberal;
                    assignedPlayers.Add(randomPlayer);
                    liberals++;
                }
            }

            // Assign the fascists
            while (fascist < fascistNeeded)
            {
                Player randomPlayer = players[randomGenerator.Next(this.Players.Count)];

                if (!assignedPlayers.Contains(randomPlayer))
                {
                    randomPlayer.Role = SecretRole.Fascist;
                    randomPlayer.Party = PartyMembership.Fascist;
                    assignedPlayers.Add(randomPlayer);
                    fascist++;
                }
            }

            // Assign Hitler (must the be last player left unnasigned)
            foreach (Player player in players)
            {
                if(!assignedPlayers.Contains(player))
                {
                    player.Role = SecretRole.Hitler;
                    player.Party = PartyMembership.Fascist;
                    assignedPlayers.Add(player);
                    break;
                }
            }

            // Randomize the players in the list
            List<Player> randomizedPlayers = assignedPlayers.OrderBy(player => randomGenerator.Next()).ToList();

            return randomizedPlayers;
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
        /// Will return true if a player with the same username already exists in the game
        /// </summary>
        /// <param player="comparedPlayer">The Player to be compared with</param>
        /// <returns>True if such a player exists, false otherwise</returns>
        private bool ContainsUsername(Player comparedPlayer)
        {
            // Create a copy of the players list to avoid concurrency issues
            List<Player> players = new List<Player>(this.Players);

            foreach (Player player in players)
            {
                if (comparedPlayer.Equals(player))
                {
                    return true;
                }
            }

            return false;
        }


        // TEST FUNCTIONS =====================================================================================================
        // Test function to add players
        private void AddTestPlayers()
        {
            this.Players.Add(new Player("Test1"));
            this.Players.Add(new Player("Test2"));
            this.Players.Add(new Player("Test3"));
            this.Players.Add(new Player("Test4"));
            this.Players.Add(new Player("Test5"));
            this.Players.Add(new Player("Test6"));
            this.Players.Add(new Player("Test7"));
            this.Players.Add(new Player("Test8"));
            this.Players.Add(new Player("Test9"));
        }
    }
}
