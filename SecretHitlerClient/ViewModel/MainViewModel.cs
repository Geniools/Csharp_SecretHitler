using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using SecretHitler.Services;

namespace SecretHitler.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly SignalRService _signalRService;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _lobbyCode;

    [ObservableProperty]
    private string _testText;

    public MainViewModel()
    {
        // Initialize the SignalR service
        this._signalRService = new SignalRService();

    }
}
