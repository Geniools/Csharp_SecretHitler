using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitlerShared;

namespace SecretHitler.ViewModel
{
    public partial class LobbyPageViewModel : ViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Player> _players;

        [ObservableProperty]
        private string _lobbyWaitingText;

        public LobbyPageViewModel(GameManager gameManager) : base(gameManager)
        {
            this.Players = this.GameManager.SignalRService.Players;
            this.LobbyWaitingText = "Waiting for players to join";
        }

        [RelayCommand]
        private async Task StartGame()
        {
            await this.GameManager.SignalRService.StartOnlineGame();
        }
    }
}
