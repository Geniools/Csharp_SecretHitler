using CommunityToolkit.Mvvm.ComponentModel;
using SecretHitler.Services;

namespace SecretHitler.ViewModel
{
    public class ViewModel : ObservableObject
    {
        private readonly SignalRService _signalRService;
        protected SignalRService SignalRService => this._signalRService;

        protected ViewModel()
        {
            // Initialize the SignalR service
            this._signalRService = new SignalRService();
        }
    }
}
