using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitler.Views;

namespace SecretHitler.ViewModel;

public partial class StartPageViewModel : ViewModel
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _lobbyCode;

    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private string _title;

    public StartPageViewModel(GameManager gameManager) : base(gameManager) 
    {
        this.GameName = "Join or Create a Game:";
        this.Title = "Stealth Führer";
    }

    [RelayCommand]
    private async Task JoinLobby()
    {
        await this.AccessLobby();

        // Navigate to the join game page
        await Shell.Current.GoToAsync(nameof(JoinGamePage));
    }

    [RelayCommand]
    private async Task CreateLobby()
    {
        await this.AccessLobby();
        
        // Navigate to the lobby page
        await Shell.Current.GoToAsync(nameof(LobbyPage));
    }

    private async Task AccessLobby()
    {
        string errorMessage = this.CanAccessLobby();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            await Shell.Current.DisplayAlert("Error", errorMessage, "OK");
            return;
        }

        // Create a lobby
        await this.GameManager.SignalRService.ConnectPlayer(this.Username, this.LobbyCode);
    }

    private string CanAccessLobby()
    {
        string errorMessage = string.Empty;

        if (!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.LobbyCode))
        {
            if (this.Username.Length < 2)
            {
                errorMessage += "Username must be at least 2 characters long.\n";
            }
            else if (this.Username.Length > 15)
            {
                errorMessage += "Username must be less than 15 characters long.\n";
            }

            if (this.LobbyCode.Length < 4)
            {
                errorMessage += "Lobby code must be at least 4 characters in length.\n";
            }
            else if (this.LobbyCode.Length > 10)
            {
                errorMessage += "Lobby code must be less than 10 characters in length.\n";
            }
        }
        else
        {
            errorMessage += "Username and lobby code cannot be empty.\n";
        }

        return errorMessage;
    }
}
