using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using SecretHitlerShared;

namespace SecretHitler.ViewModel
{
    public partial class MainPageViewModel : ViewModel
    {
        // The players in the game
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        public bool FiveSixPlayerGame => this.Players.Count == 5 || this.Players.Count == 6;
        public bool SevenEightPlayerGame => this.Players.Count == 7 || this.Players.Count == 8;
        public bool NineTenPlayerGame => this.Players.Count == 9 || this.Players.Count == 10;

        public bool IsPrimary => this.GameManager.IsPrimary;

        // Policies
        private Dictionary<int, bool> _policyVisibility;

        // Liberal policies visibility
        [ObservableProperty]
        private bool _liberalPolicy1;
        [ObservableProperty]
        private bool _liberalPolicy2;
        [ObservableProperty]
        private bool _liberalPolicy3;
        [ObservableProperty]
        private bool _liberalPolicy4;
        [ObservableProperty]
        private bool _liberalPolicy5;

        // Fascist policies visibility
        [ObservableProperty]
        private bool _fascistPolicy1;
        [ObservableProperty]
        private bool _fascistPolicy2;
        [ObservableProperty]
        private bool _fascistPolicy3;
        [ObservableProperty]
        private bool _fascistPolicy4;
        [ObservableProperty]
        private bool _fascistPolicy5;
        [ObservableProperty]
        private bool _fascistPolicy6;

        // Board + text visibility
        [ObservableProperty]
        private bool _boardVisibility;

        // Voting buttons visibility
        [ObservableProperty]
        private bool _votingVisibility;

        // Card picker visibility and cards
        [ObservableProperty]
        private bool _cardPickerVisibility;

        [ObservableProperty]
        private string _card1;
        [ObservableProperty]
        private string _card2;
        [ObservableProperty]
        private string _card3;

        [ObservableProperty]
        private bool _card3Enabled;

        // Player selection visibility
        [ObservableProperty]
        private bool _playerSelectionVisibility;

        // Election tracker
        [ObservableProperty]
        private byte _electionTracker;

        // Label for events
        [ObservableProperty]
        private string _eventLabel;

        [ObservableProperty]
        private bool _eventLabelVisibility;
        

        public MainPageViewModel(GameManager gameManager) : base(gameManager)
        {
            // Subscibe to events
            this.GameManager.SignalRService.OnPresidentSelected += this.PresidentSelected;
            this.GameManager.SignalRService.OnChancellorVoting += this.ChancellorVoting;
            this.GameManager.SignalRService.OnPolicySelection += this.PolicySelection;
            this.GameManager.OnPolicyEnacted += this.PolicyEnacted;

            // Get the needed properties from the GameManager
            this.Players = this.GameManager.SignalRService.Players;
            this.ElectionTracker = this.GameManager.FailedElectionTracker;

            this._policyVisibility = new Dictionary<int, bool>();
            this._policyVisibility.Add(1, false);
            this._policyVisibility.Add(2, false);
            this._policyVisibility.Add(3, false);
            this._policyVisibility.Add(4, false);
            this._policyVisibility.Add(5, false);
            this._policyVisibility.Add(6, false);

            // Assign the (default) visibility of the policies
            this.LiberalPolicy1 = false;
            this.LiberalPolicy2 = false;
            this.LiberalPolicy3 = false;
            this.LiberalPolicy4 = false;
            this.LiberalPolicy5 = false;

            this.FascistPolicy1 = false;
            this.FascistPolicy2 = false;
            this.FascistPolicy3 = false;
            this.FascistPolicy4 = false;
            this.FascistPolicy5 = false;
            this.FascistPolicy6 = false;

            // Assign the (default) visibility of the board
            this.BoardVisibility = false;

            // Assign the (default) visibility of the voting buttons
            this.VotingVisibility = false;

            // Assign the (default) visibility of the card picker and card images
            this.CardPickerVisibility = false;

            // Assign the (default) visibility of the player selection
            this.PlayerSelectionVisibility = false;

            //this.SetPlayerPicture();
            this.EventLabel = "Default event";
        }

        private void PolicySelection(PolicyCard card1, PolicyCard card2, PolicyCard card3)
        {
            this.CardPickerVisibility = true;
            this.Card1 = card1.Image;
            this.Card2 = card2.Image;
            if(card3 != null)
            {
                this.Card3Enabled = true;
                this.Card3 = card3.Image;
            }
            else
            {
                this.Card3Enabled = false;
            }
        }

        private void PolicyEnacted(PolicyCard card)
        {
            if(card.Party is PartyMembership.Liberal)
            {
                this.GameManager.Board.PlayedLiberalCards++;
            }
            else
            {
                this.GameManager.Board.PlayedFascistsCards++;
            }

            // Update the UI for liberal policies
            switch(this.GameManager.Board.PlayedLiberalCards)
            {
                case 1:
                    this.LiberalPolicy1 = true;
                    break;
                case 2:
                    this.LiberalPolicy1 = true;
                    this.LiberalPolicy2 = true;
                    break;
                case 3:
                    this.LiberalPolicy1 = true;
                    this.LiberalPolicy2 = true;
                    this.LiberalPolicy3 = true;
                    break;
                case 4:
                    this.LiberalPolicy1 = true;
                    this.LiberalPolicy2 = true;
                    this.LiberalPolicy3 = true;
                    this.LiberalPolicy4 = true;
                    break;
                case 5:
                    this.LiberalPolicy1 = true;
                    this.LiberalPolicy2 = true;
                    this.LiberalPolicy3 = true;
                    this.LiberalPolicy4 = true;
                    this.LiberalPolicy5 = true;
                    break;
            }

            // Update the UI for fascist policies
            switch(this.GameManager.Board.PlayedFascistsCards)
            {
                case 1:
                    this.FascistPolicy1 = true;
                    break;
                case 2:
                    this.FascistPolicy1 = true;
                    this.FascistPolicy2 = true;
                    break;
                case 3:
                    this.FascistPolicy1 = true;
                    this.FascistPolicy2 = true;
                    this.FascistPolicy3 = true;
                    break;
                case 4:
                    this.FascistPolicy1 = true;
                    this.FascistPolicy2 = true;
                    this.FascistPolicy3 = true;
                    this.FascistPolicy4 = true;
                    break;
                case 5:
                    this.FascistPolicy1 = true;
                    this.FascistPolicy2 = true;
                    this.FascistPolicy3 = true;
                    this.FascistPolicy4 = true;
                    this.FascistPolicy5 = true;
                    break;
                case 6:
                    this.FascistPolicy1 = true;
                    this.FascistPolicy2 = true;
                    this.FascistPolicy3 = true;
                    this.FascistPolicy4 = true;
                    this.FascistPolicy5 = true;
                    this.FascistPolicy6 = true;
                    break;
            }
        }

        private void ChancellorVoting(Player player)
        {
            this.GameManager.GameStatus.CandidateChancellor = player;

            // Update the UI
            this.BoardVisibility = false;
            this.VotingVisibility = true;
            this.CardPickerVisibility = false;
            this.PlayerSelectionVisibility = false;
            // Label text
            this.EventLabelVisibility = true;
            this.EventLabel = $"Vote for -{player.Username.ToUpper()}- as chancellor";
        }

        [RelayCommand]
        private async Task VotingJa()
        {
            // When the players votes ja, this will be executed
            this.VotingVisibility = false;
            this.EventLabel = "Waiting for others...";
            await this.GameManager.SignalRService.VoteBallot(BallotType.Ja);
        }

        [RelayCommand]
        private async Task VotingNein()
        {
            // When the players votes nein, this will be executed
            this.VotingVisibility = false;
            this.EventLabel = "Waiting for others...";
            await this.GameManager.SignalRService.VoteBallot(BallotType.Nein);
        }

        [RelayCommand]
        private async Task RevealFascistPlayerIcons()
        {
            await Shell.Current.Dispatcher.DispatchAsync(() => { 
                // First check if the player is a fascist, otherwise do nothing
                if(this.GameManager.SignalRService.ThisPlayer.IsFascist())
                {
                    // Check if the picture is already changed
                    if(this.IsDisplayingDefaultPictures())
                    {
                        this.SetPlayerPicture(picture: GameImages.FascistIcon);
                    }
                    else
                    {
                        this.SetPlayerPicture();
                    }
                }
            });
        }

        [RelayCommand]
        private async Task PlayNextRound()
        {
            if (this.GameManager.IsPrimary)
            {
                // Election phase
                Player president = this.GameManager.GameStatus.GetNextPresident();
                // Clear the voting results
                this.GameManager.Board.VotingResults.Clear();
                // Set the status to chancellor selection
                await this.GameManager.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.PlayerSelectionStatusName, this.GameManager.SignalRService.ThisPlayer.LobbyCode, EntitySelectionStatus.ChancellorSelection);
                // Notify players of the selected president
                await this.GameManager.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.PresidentSelectedName, president);
            }
        }

        private async void PresidentSelected(Player currentPresident)
        {
            this.GameManager.PresidentSelected(currentPresident);
            // Update the players that can be selected
            this.GameManager.UpdateSelectablePlayers();

            // If this is the president, change the UI
            if (this.GameManager.SignalRService.ThisPlayer.Equals(currentPresident))
            {
                // Change the UI based on the selected president
                this.BoardVisibility = false;
                this.VotingVisibility = false;
                this.CardPickerVisibility = false;
                // Change the visibility of the player selection
                this.PlayerSelectionVisibility = true;
                // Update the label
                this.EventLabelVisibility = true;
                this.EventLabel = "Propose a new chancellor";
            }
        }

        private bool IsDisplayingDefaultPictures()
        {
            foreach(Player pl in this.Players)
            {
                if(pl.ImageSource != GameImages.PlayerIcon)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetPlayerPicture(Player player = null, string picture = GameImages.PlayerIcon)
        {
            if (player is null)
            {
                // Change the picture of the players
                foreach (Player pl in this.Players)
                {
                    pl.ImageSource = picture;
                }
            }
            else
            {
                player.ImageSource = picture;
            }
        }

        [RelayCommand]
        private async Task SelectCard(FileImageSource cardSource)
        {
            PolicyCard card = new PolicyCard(PartyMembership.Liberal);
            string cardName = cardSource.ToString();

            // Create a policy card based on the source of the card image
            if(cardName.Contains("fascist"))
            {
                card = new PolicyCard(PartyMembership.Fascist);
            }

            this.CardPickerVisibility = false;

            // Notify the server of the selected card
            await this.GameManager.SignalRService.HubConnection.InvokeAsync(ServerCallbacks.PolicyCardSelectedName, this.GameManager.SignalRService.PrimaryPlayer, card);
        }
    }
}