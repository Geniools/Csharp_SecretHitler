using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using SecretHitlerShared;
using System.Collections.ObjectModel;
using SecretHitler.Views;

namespace SecretHitler.Services
{
    public class SignalRService
    {
        public ObservableCollection<Player> Players { get; set; }

        // Connection to the server
        public HubConnection HubConnection { get; private set; }
        private readonly string _baseUrl;
        private readonly string _hubName;

        // Events
        public event Action<Player> OnPlayerConnected;
        public event Action<string> OnPlayerDisconnected;
        public event Action OnGameStarted;
        public event Action OnClearAllPlayers;
        public event Action<Player> OnPresidentSelected;
        public event Action OnChancellorSelected;
        public event Action<Player> OnChancellorVoting;
        public event Action<EntitySelectionStatus> OnPlayerSelectionStatus;
        public event Action<Player, BallotType> OnBallotVoted;
        public event Action<PolicyCard, PolicyCard, PolicyCard> OnPolicySelection;
        public event Action<PolicyCard> OnPolicyCardSelected;
        public event Action<PolicyCard> OnPolicyEnacted;


        // Other properties
        internal Player ThisPlayer { get; set; }
        internal Player PrimaryPlayer { get; set; }

        public SignalRService(string hubName, string baseUrl = "http://localhost", int portNr = 80)
        {
            this.Players = new ObservableCollection<Player>();

            // Android can't connect to localhost (for some reason)
            if (baseUrl.Equals("http://localhost") && DeviceInfo.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }

            if (portNr != 80 && portNr != 443)
            {
                baseUrl += $":{portNr}";
            }

            this._baseUrl = baseUrl;
            this._hubName = hubName;

            // Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/{hubName}")
                .Build();
        }

        /// <summary>
        /// Will create (or recreate) the connection to the server, subscribe to events and start the connection
        /// </summary>
        /// <returns></returns>
        private async Task StartConnection()
        {
            // (Re)Create the connection
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{this._baseUrl}/{this._hubName}")
                .Build();

            // This function must contain all event handlers

            // Handle the PlayerConnected event
            this.HubConnection.On<Player>(ServerCallbacks.PlayerConnectedName, connectedPlayer =>
            {
                this.OnPlayerConnected?.Invoke(connectedPlayer);
            });

            // Will be called when a player disconnects (this player) - will be triggered by the "primary" client
            this.HubConnection.On<Player, string>(ServerCallbacks.DisconnectPlayerName, (disconnectingPlayer, message) =>
            {
                //Console.WriteLine($"Player {disconnectingPlayer.ConnectionId} disconnected: {message}");
                this.OnPlayerDisconnected?.Invoke(message);
            });
            
            // Handle the GameStarted event
            this.HubConnection.On(ServerCallbacks.StartGameName, async () =>
            {
                this.OnGameStarted?.Invoke();
            });
            
            // Set the primary player
            this.HubConnection.On<Player>(ServerCallbacks.SetPrimaryPlayerName, primaryPlayer =>
            {
                this.PrimaryPlayer = primaryPlayer;
            });

            // Before starting a new game, clear all players from other clients
            this.HubConnection.On(ServerCallbacks.ClearAllPlayersName, () =>
            {
                this.OnClearAllPlayers?.Invoke();
            });            

            // Handle the PresidentSelected event
            this.HubConnection.On<Player>(ServerCallbacks.PresidentSelectedName, currentPresident =>
            {
                this.OnPresidentSelected?.Invoke(currentPresident);
            });

            // Handle the PlayerSelectionStatus event
            this.HubConnection.On<EntitySelectionStatus>(ServerCallbacks.EntitySelectionStatusName, (status) =>
            {
                this.OnPlayerSelectionStatus?.Invoke(status);
            });

            // Handle the Voting event
            this.HubConnection.On<Player>(ServerCallbacks.ChancellorVotingName, player =>
            {
                this.OnChancellorVoting?.Invoke(player);
            });

            // Handle the ChancellorSelected event
            this.HubConnection.On(ServerCallbacks.ChancellorSelectedName, () =>
            {
                this.OnChancellorSelected?.Invoke();
            });

            // Handle the VotingBallot event
            this.HubConnection.On<Player, BallotType>(ServerCallbacks.VotingBallotName, (player, card) =>
            {
                this.OnBallotVoted?.Invoke(player, card);
            });

            // Handle the Policy Selection event
            this.HubConnection.On<PolicyCard, PolicyCard, PolicyCard>(ServerCallbacks.PolicySelectionName, (card1, card2, card3) =>
            {
                this.OnPolicySelection?.Invoke(card1, card2, card3);
            });

            // Handle the Policy Card Selected event
            this.HubConnection.On<PolicyCard>(ServerCallbacks.PolicyCardSelectedName, (card) =>
            {
                this.OnPolicyCardSelected?.Invoke(card);
            });

            // Handle the Policy Enacted event
            this.HubConnection.On<PolicyCard>(ServerCallbacks.PolicyCardEnactedName, (card) =>
            {
                this.OnPolicyEnacted?.Invoke(card);
            });

            // Handle the Pop up message
            this.HubConnection.On<string, string>(ServerCallbacks.SendPopUpName, async (title, message) =>
            {
                await Shell.Current.Dispatcher.DispatchAsync(async () =>
                {
                    await Shell.Current.DisplayAlert(title, message, "Ok");
                });
            });

            // Set the default connection limit
            ServicePointManager.DefaultConnectionLimit = 10;

            //Start the connection
            await this.HubConnection.StartAsync();

            // Get the connection id
            this.ThisPlayer.ConnectionId = this.HubConnection.ConnectionId;
        }
        
        internal async Task VoteBallot(BallotType votingCard)
        {
            await this.HubConnection.InvokeAsync(ServerCallbacks.VotingBallotName, this.ThisPlayer, this.PrimaryPlayer.ConnectionId, votingCard);
        }

        internal async Task ConnectPlayer(Player player)
        {
            this.ThisPlayer = player;
            // The connection is (re)created every time to avoid errors if the user navigates back to the main page
            await this.StartConnection();

            await this.HubConnection.InvokeAsync(ServerCallbacks.PlayerConnectedName, player);
        }

        internal async Task StartOnlineGame()
        {
            // Clear all players from other clients
            await this.HubConnection.InvokeAsync(ServerCallbacks.ClearAllPlayersName, this.ThisPlayer.LobbyCode);

            // Randomly assign roles to players
            List<Player> finalPlayers = this.AssignRandomRolesToPlayers();
            this.Players = new ObservableCollection<Player>(finalPlayers);

            // Set the primary player (the one who started the game)
            await this.HubConnection.InvokeAsync(ServerCallbacks.SetPrimaryPlayerName, this.ThisPlayer);

            // Notify all players of the other connected players
            foreach (Player player in this.Players)
            {
                // WILL NOT WORK WITH TEST PLAYERS
                await this.HubConnection.InvokeAsync(ServerCallbacks.ConnectPlayerName, player);
            }

            // Start the game
            if (!string.IsNullOrEmpty(this.ThisPlayer.LobbyCode))
            {
                await this.HubConnection.InvokeAsync(ServerCallbacks.StartGameName, this.ThisPlayer.LobbyCode);
            }
        }

        /// <summary>
        /// Will assign random roles to the players playing the game based on their number
        /// Should be called only once, at the beginning of the game by the "Primary" player
        /// </summary>
        /// <returns>A list of players assigned random roles for the game.</returns>
        /// <exception cref="NotSupportedException">If the player number is not between 5 and 10.</exception>
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
                    // For testing purposes we just return
                    return this.Players.ToList();
                    //throw new NotSupportedException($"The amount of players is not supported. Player range must be between 5 and 10. Players given: {this.Players.Count}");
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
                if (!assignedPlayers.Contains(player))
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
    }
}
