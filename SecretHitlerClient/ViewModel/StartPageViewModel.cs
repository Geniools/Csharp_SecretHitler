using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecretHitler.Views;
using SecretHitlerShared;

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
        // Make sure the connection to the server is closed
        //this.GameManager.SignalRService.HubConnection.StopAsync();

        // Set the title and game name
        this.GameName = "Stealth Führer";
        this.Title = "Join or Create a Game:";
    }

    [RelayCommand]
    private async Task JoinLobby()
    {
        Player player = new Player(this.Username, this.LobbyCode);
        try
        {
            await this.AccessLobby(player);
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }

        this.GameManager.IsPrimary = false;

        // Navigate to the join game page
        await Shell.Current.GoToAsync(nameof(JoinGamePage));
    }

    [RelayCommand]
    private async Task CreateLobby()
    {
        Player player = new Player(this.Username, this.LobbyCode);
        try
        {
            await this.AccessLobby(player);
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }

        this.GameManager.IsPrimary = true;
        // Add the player to the list of players
        this.GameManager.SignalRService.Players.Add(player);

        // Navigate to the lobby page
        await Shell.Current.GoToAsync(nameof(LobbyPage));
    }

    private async Task AccessLobby(Player player)
    {
        string errorMessage = this.CanAccessLobby();

        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        // Create a lobby
        await this.GameManager.SignalRService.ConnectPlayer(player);
    }

    private string CanAccessLobby()
    {
        string errorMessage = string.Empty;

        if (!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.LobbyCode))
        {
            // Check for special symbols in the username
            if (this.Username.Any(c => !char.IsLetterOrDigit(c)))
            {
                errorMessage += "Username cannot contain special characters.\n";
            }

            // Check for special symbols in the lobby code
            if (this.LobbyCode.Any(c => !char.IsLetterOrDigit(c)))
            {
                errorMessage += "Lobby code cannot contain special characters.\n";
            }

            // Check the length of the username
            if (this.Username.Length < 2)
            {
                errorMessage += "Username must be at least 2 characters long.\n";
            }
            else if (this.Username.Length > 10)
            {
                errorMessage += "Username must be less than 10 characters long.\n";
            }

            // Check the length of the lobby code
            if (this.LobbyCode.Length < 4)
            {
                errorMessage += "Lobby code must be at least 4 characters in length.\n";
            }
            else if (this.LobbyCode.Length > 6)
            {
                errorMessage += "Lobby code must be less than 6 characters in length.\n";
            }
        }
        else
        {
            errorMessage += "Username and lobby code cannot be empty.\n";
        }

        return errorMessage;
    }
}
