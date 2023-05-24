using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SecretHitler.ViewModel;

public partial class StartPageViewModel : ViewModel
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _lobbyCode;

    [ObservableProperty]
    private string _playerJoinedLabel;

    public StartPageViewModel(GameManager gameManager) : base(gameManager) { }

    [RelayCommand]
    private async Task JoinLobby()
    {
        // Join a lobby
        await this.GameManager.SignalRService.JoinLobby(this.Username, this.LobbyCode);
        
        // Navigate to the join game page
        await Shell.Current.GoToAsync("JoinGamePage");
    }

    [RelayCommand]
    private async Task CreateLobby()
    {
        // Create a lobby
        await this.GameManager.SignalRService.CreateLobby(this.Username, this.LobbyCode);
        
        // Navigate to the lobby page
        await Shell.Current.GoToAsync("LobbyPage");
    }
}
