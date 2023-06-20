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

        // Liberal
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

        // Fascist
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

        // Election tracker
        [ObservableProperty]
        private byte _electionTracker;
        

        public MainPageViewModel(GameManager gameManager) : base(gameManager)
        {
            // Get the needed properties from the GameManager
            this.Players = this.GameManager.Players;
            this.ElectionTracker = this.GameManager.ElectionTracker;

            // Assign the (default) vissibility of the policies
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

            //this.SetPlayerPicture();
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
    }
}