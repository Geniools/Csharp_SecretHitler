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
    private string _playerJoined;

    public StartPageViewModel()
    {
        this.SignalRService.UpdateUsername += this.UpdateUsername;
    }

    [RelayCommand]
    private void SendMessage()
    { 
        // Send a message to the server
        this.SignalRService.SendMessageToAll(this.Username, this.LobbyCode);
    }

    private void UpdateUsername(string username)
    {
        this.PlayerJoined = username;
    }
}
