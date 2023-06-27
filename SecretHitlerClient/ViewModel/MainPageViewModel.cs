using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        // Card picker visibility
        [ObservableProperty]
        private bool _cardPickerVisibility;

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

            // Get the needed properties from the GameManager
            this.Players = this.GameManager.SignalRService.Players;
            this.ElectionTracker = this.GameManager.ElectionTracker;

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

            // Assign the (default) visibility of the card picker
            this.CardPickerVisibility = true;

            // Assign the (default) visibility of the player selection
            this.PlayerSelectionVisibility = false;

            //this.SetPlayerPicture();
            this.EventLabel = "Default event";
        }

        private void ChancellorVoting(Player player)
        {
            // Update the UI
            this.BoardVisibility = false;
            this.VotingVisibility = true;
            this.CardPickerVisibility = false;
            this.PlayerSelectionVisibility = false;
            this.EventLabel = $"Vote for {player.Username} to be the chancellor";
        }

        [RelayCommand]
        private async Task VotingJa()
        {
            // When the players votes ja, this will be executed
            this.VotingVisibility = false;
            await this.GameManager.SignalRService.VoteBallot(BallotType.Ja);
        }

        [RelayCommand]
        private async Task VotingNein()
        {
            // When the players votes nein, this will be executed
            this.VotingVisibility = false;
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
            await this.GameManager.PlayNextRound();
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

        private void PresidentSelected()
        {
            // Change the UI based on the selected president
            this.BoardVisibility = false;
            this.VotingVisibility = false;
            this.CardPickerVisibility = false;
            // Change the visibility of the player selection
            this.PlayerSelectionVisibility = true;
            this.EventLabel = "Select a player to be the chancellor";
        }
    }
}