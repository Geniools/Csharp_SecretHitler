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

        [ObservableProperty]
        private string _lobbyWaitingText;

        public LobbyPageViewModel(GameManager gameManager) : base(gameManager)
        {
            this.Players = this.GameManager.Players;
            this.LobbyWaitingText = "Waiting for players to join";
        }

        [RelayCommand]
        private async Task StartGame()
        {
            // Create a list of players
            List<Player> connectedPlayers = new List<Player>();
            foreach (Player player in this.Players)
            {
                connectedPlayers.Add(player);
            }

            await this.GameManager.SignalRService.StartOnlineGame(connectedPlayers);
        }
    }
}
