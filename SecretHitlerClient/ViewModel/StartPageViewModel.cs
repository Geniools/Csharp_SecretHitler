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

    [ObservableProperty]
    private bool _isJoinButtonEnabled;
    
    [ObservableProperty]
    private bool _isCreateButtonEnabled;

    public StartPageViewModel(GameManager gameManager) : base(gameManager)
    {
        this.IsJoinButtonEnabled = true;
        this.IsCreateButtonEnabled = true;
    }

    [RelayCommand]
    private void JoinLobby()
    {
        // Join a lobby
        this.GameManager.SignalRService.JoinLobby(this.Username, this.LobbyCode);
    }

    [RelayCommand]
    private void CreateLobby()
    {
        // Create a lobby
        this.GameManager.SignalRService.CreateLobby(this.Username, this.LobbyCode);
    }
}
