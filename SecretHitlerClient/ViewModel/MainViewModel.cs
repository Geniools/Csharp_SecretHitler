using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace SecretHitler.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly HubConnection _hubConnection;

    [ObservableProperty]
    public string username;

    [ObservableProperty]
    public string lobbyCode;

    [ObservableProperty]
    public string testText;

    public MainViewModel()
    {
        string baseUrl = "http://localhost";

        // Android can't connect to localhost
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            baseUrl = "http://10.0.2.2";
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}:5142/gameHub")
            .Build();

        this.SetupHubListeners();

        //Task.Run(() =>
        //{
        //    bool v = Dispatcher.Dispatch(async () =>
        //    {
        //        await _hubConnection.StartAsync();
        //    });
        //});

        this._hubConnection.StartAsync();
    }

    private void SetupHubListeners()
    {
        _hubConnection.On<string, string>("ReceiveMessage", (username, lobbyCode) =>
        {
            this.TestText = $"username: {username}, lobby: {lobbyCode}";
        });
    }

    [RelayCommand]
    public async Task SendTestMessageAsync()
    {
        await this.SendMessageToAll(this.Username, this.LobbyCode);
    }

    public async Task SendMessageToAll(string username, string lobbyCode)
    {
        await _hubConnection.InvokeAsync("SendMessageToAll", username, lobbyCode);
    }
}
