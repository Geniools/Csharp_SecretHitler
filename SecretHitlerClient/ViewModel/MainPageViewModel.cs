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
            this.Players = this.GameManager.Players;
            this.ElectionTracker = this.GameManager.ElectionTracker;

            // Assign the vissibility of the policies
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

            // Test commands
            this.AddPlayers();

            this.ArrangePlayers();

            // Change the picture of the players
            foreach (Player pl in this.Players)
            {
                pl.ImageSource = GameImages.PlayerImage;
            }
        }

        // Test function to add players
        private void AddPlayers()
        {
            this.GameManager.Players.Add(new Player("Test1"));
            this.GameManager.Players.Add(new Player("Test2"));
            this.GameManager.Players.Add(new Player("Test3"));
            this.GameManager.Players.Add(new Player("Test4"));
            this.GameManager.Players.Add(new Player("Test5"));
            this.GameManager.Players.Add(new Player("Test6"));
            this.GameManager.Players.Add(new Player("Test7"));
            this.GameManager.Players.Add(new Player("Test8"));
            this.GameManager.Players.Add(new Player("Test9"));
        }

        private void ArrangePlayers()
        {
            // Assign the displayed Row and Column to each player
            //for (int i = 0; i < this.Players.Count; i++)
            //{
            //    if (i > 5)
            //    {
            //        this.Players[i].RowNr = i - 5;
            //        this.Players[i].ColumnNr = 0;
            //    }
            //    else
            //    {
            //        this.Players[i].RowNr = i;
            //        this.Players[i].ColumnNr = 0;
            //    }
            //}

            for (int i = 0; i < this.Players.Count; i++)
            {
                this.Players[i].ColumnNr = i;
            }
        }
    }
}