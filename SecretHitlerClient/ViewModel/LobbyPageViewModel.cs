
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SecretHitler.Model;

namespace SecretHitler.ViewModel
{
    public partial class LobbyPageViewModel : ViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        protected LobbyPageViewModel(GameManager gameManager) : base(gameManager)
        {
            this.Players = this.GameManager.game.Players;
        }
    }
}
