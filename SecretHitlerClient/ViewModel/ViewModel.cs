using CommunityToolkit.Mvvm.ComponentModel;

namespace SecretHitler.ViewModel
{
    public class ViewModel : ObservableObject
    {
        public GameManager GameManager { get; private set; }

        protected ViewModel(GameManager gameManager)
        {
            // Initialize the SignalR service
            this.GameManager = gameManager;
        }
    }
}
