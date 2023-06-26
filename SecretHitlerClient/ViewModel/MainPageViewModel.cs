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
        

        public MainPageViewModel(GameManager gameManager) : base(gameManager)
        {
            // Get the needed properties from the GameManager
            this.Players = this.GameManager.Players;
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

        [RelayCommand]
        private async Task VotingJa()
        {
            // When the players votes ja, this will be executed
            this.VotingVisibility = false;
        }

        [RelayCommand]
        private async Task VotingNein()
        {
            // When the players votes nein, this will be executed
            this.VotingVisibility = false;
        }

        [RelayCommand]
        private async Task RevealFascistPlayerIcons()
        {
            // TODO: First check if the player is a fascist, otherwise do nothing
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
        private async Task SelectPlayer()
        {
            
        }
    }
}