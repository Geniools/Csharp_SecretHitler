using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitlerShared;

namespace SecretHitler.ViewModel
{
    public partial class MainPageViewModel : ViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        [ObservableProperty]
        private ObservableCollection<PolicyCard> _electedPolicies;

        public MainPageViewModel(GameManager gameManager) : base(gameManager)
        {
            this.Players = this.GameManager.Players;

            // Create the policies
            this.ElectedPolicies = this.GameManager.Board.ElectedPolicies;

            // Test commands
            this.AddPlayers();

            this.ArrangePlayers();
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
            for (int i = 0; i < this.Players.Count; i++)
            {
                if (i > 5)
                {
                    this.Players[i].RowNr = i - 5;
                    this.Players[i].ColumnNr = 0;
                }
                else
                {
                    this.Players[i].RowNr = i;
                    this.Players[i].ColumnNr = 0;
                }
            }
        }
    }
}