using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitler.Model;

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
        }
    }
}
