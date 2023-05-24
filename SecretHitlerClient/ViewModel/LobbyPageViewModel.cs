
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitler.Model;

namespace SecretHitler.ViewModel
{
    public partial class LobbyPageViewModel : ViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        public LobbyPageViewModel(GameManager gameManager) : base(gameManager)
        {
            this.Players = this.GameManager.Players;
        }

        [RelayCommand]
        private async Task StartGame()
        {
            await this.GameManager.StartGame();
        }
    }
}
