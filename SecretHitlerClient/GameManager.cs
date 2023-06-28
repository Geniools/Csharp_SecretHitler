﻿using SecretHitler.Services;
using SecretHitler.Views;
using Microsoft.AspNetCore.SignalR.Client;
using SecretHitlerShared;

namespace SecretHitler
{
    /// <summary>
    /// For better code readability, the GameManager class is split into multiple classes.
    /// The GameManager class is the main class of the application.
    /// </summary>
    public partial class GameManager : BindableObject
    {
        public Board Board { get; private set; }
        public Chat Chat { get; private set; }
        public byte FailedElectionTracker { get; private set; }
        
        // Event


        public GameManager()
        {
            // Create the SignalRService
            this.SignalRService = new SignalRService(HubName, "https://secrethitler.azurewebsites.net");

            // Subscribe to events
            SignalRService.OnPlayerConnected += this.AddPlayer;
            SignalRService.OnPlayerDisconnected += this.DisconnectPlayer;
            SignalRService.OnClearAllPlayers += this.ClearAllPlayers;
            SignalRService.OnGameStarted += this.StartLocalGame;

            foreach (Player player in this.SignalRService.Players)
            {
                player.OnPlayerSelected += this.PlayerSelected;
            }

            SignalRService.OnPlayerSelectionStatus += this.PlayerSelectionStatusChange;
            SignalRService.OnBallotVoted += this.BallotVotes;
            SignalRService.OnChancellorSelected += this.ChancellorSelected;

            // Create a new game
            this.Board = new Board();
            this.GameStatus = new GameStatus();
            this.FailedElectionTracker = 0;
            this.Chat = new Chat();

            // Test commands
            //this.AddTestPlayers();
        }
    }

    /// <summary>
    /// For better code readability, the GameManager class is split into multiple classes
    /// This file contains the SignalR related functions
    /// </summary>
    public partial class GameManager : BindableObject
    {
        // Services
        public SignalRService SignalRService { get; private set; }
        public const string HubName = "gameHub";
        
        // Game specific properties
        public bool IsPrimary { get; set; }
        public bool GameStarted { get; set; }

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
                    if (this.SignalRService.Players.Count <= 10)
                    {
                        errorCode = 3;
                        // Check if the game has already started
                        if (!this.GameStarted)
                        {
                            errorCode = -1;
                            this.SignalRService.Players.Add(player);
                        }
                    }
                }

                // If there was an error, disconnect the player (with the explenatory message)
                if (errorCode != -1)
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

            // Add all players to the game status
            foreach (Player player in this.SignalRService.Players)
            {
                this.GameStatus.AddPlayer(player);
            }

            // When interacting with the UI, use the dispatcher (has something to do with Threading...)
            await Shell.Current.Dispatcher.DispatchAsync(async () =>
                await Shell.Current.GoToAsync(nameof(MainPage))
            );
        }

        private void PlayerSelectionStatusChange(PlayerSelectionStatus status)
        {
            this.GameStatus.PlayerSelectionStatus = status;
        }

        private void ChancellorSelected(Player currentChancellor)
        {
            this.GameStatus.CurrentChancelor = currentChancellor;
            // TODO: Change the icon of the player
        }

        internal void PresidentSelected(Player currentPresident)
        {
            this.GameStatus.CurrentPresident = currentPresident;
            // TODO: Change the icon of the player
        }

        private void ClearAllPlayers()
        {
            if (!this.IsPrimary)
            {
                this.SignalRService.Players.Clear();
            }
        }
    }

    /// <summary>
    /// For better code readability, the GameManager class is split into multiple classes
    /// This file contains the game logic
    /// </summary>
    public partial class GameManager : BindableObject
    {        
        public GameStatus GameStatus { get; private set; }

        // Game logic ==========================================================================================================

        public async Task PlayNextRound()
        {
            if (this.IsPrimary)
            {
                // Election phase
                Player president = this.GameStatus.GetNextPresident();
                // Clear the voting results
                this.Board.VotingResults.Clear();
                // Set the status to chancellor selection
                await this.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.PlayerSelectionStatusName, this.SignalRService.ThisPlayer.LobbyCode, PlayerSelectionStatus.ChancellorSelection);
                // Notify players of the selected president
                await this.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.PresidentSelectedName, president);
            }
        }

        private async void PlayerSelected(Player player)
        {
            if (this.GameStatus.PlayerSelectionStatus is PlayerSelectionStatus.ChancellorSelection)
            {
                // Perform voting logic
                await this.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.ChancellorVotingName, player);
            }
        }

        // This function is only called by the primary player
        private void BallotVotes(Player player, BallotType type)
        {
            this.Board.VotingResults.Add(player, type);

            // Check if all players have voted
            if (this.SignalRService.Players.Count == this.Board.VotingResults.Count)
            {
                // Check if the vote was accepted
                int ja = 0;
                int nein = 0;

                foreach (BallotType ballot in this.Board.VotingResults.Values)
                {
                    if (ballot is BallotType.Ja)
                    {
                        ja++;
                    }
                    else
                    {
                        nein++;
                    }
                }

                // If the vote was accepted, the player is elected
                if (ja > nein)
                {
                    // TODO: Notify the players that the player is elected
                    this.GameStatus.EnactCandidateChancellor();
                    this.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.ChancellorSelectedName, this.GameStatus.CurrentChancelor);
                    Shell.Current.DisplayAlert("Election", "The election was successful", "OK");
                    // TODO: Start the process of enacting a policy
                }
                else
                {
                    this.FailedElectionTracker++;
                    // Check if the election tracker is at 3 (country is thrown into chaos)
                    if (this.FailedElectionTracker == 3)
                    {
                        // TODO: Reveal and enact the first policy from the top of the deck
                    }
                }
            }
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
            List<Player> players = new List<Player>(this.SignalRService.Players);

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
            this.SignalRService.Players.Add(new Player("Test1"));
            this.SignalRService.Players.Add(new Player("Test2"));
            this.SignalRService.Players.Add(new Player("Test3"));
            this.SignalRService.Players.Add(new Player("Test4"));
            this.SignalRService.Players.Add(new Player("Test5"));
            this.SignalRService.Players.Add(new Player("Test6"));
            this.SignalRService.Players.Add(new Player("Test7"));
            this.SignalRService.Players.Add(new Player("Test8"));
            this.SignalRService.Players.Add(new Player("Test9"));
        }
    }
}
