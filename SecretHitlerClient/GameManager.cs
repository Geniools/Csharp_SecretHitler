using SecretHitler.Model;
using SecretHitler.Services;
using System.Collections.ObjectModel;
using SecretHitler.Views;

namespace SecretHitler
{
    public class GameManager : BindableObject
    {
        // Services
        public SignalRService SignalRService { get; private set; }

        // Game state
        public ObservableCollection<Player> Players { get; private set; }
        public Board Board { get; private set; }
        public Chat Chat { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public Player Winner { get; private set; }
        public Player CurrentPresident { get; private set; }
        public Player CurrentChancelor { get; private set; }
        public byte ElectionTracker { get; private set; }

        public GameManager()
        {
            this.SignalRService = new SignalRService("gameHub", "https://secrethitler.azurewebsites.net");
            // Subscribe to events
            SignalRService.PlayerConnected += this.AddPlayer;
            SignalRService.GameStarted += this.StartLocalGame;

            // Create a new game
            this.Board = new Board();
            this.Players = new ObservableCollection<Player>();
            this.Chat = new Chat();
            this.GameStatus = new GameStatus();
            this.ElectionTracker = 0;
        }

        private void AddPlayer(Player player)
        {
            Shell.Current.Dispatcher.DispatchAsync(() => this.Players.Add(player));
        }

        public async void StartLocalGame()
        {
            await Shell.Current.Dispatcher.DispatchAsync(async () => await Shell.Current.GoToAsync(nameof(MainPage)));
        }

        public async Task EndGame()
        {
            throw new NotImplementedException();
        }

        public Player RunNextElection()
        {
            throw new NotImplementedException();
        }

        public Player SetNextPresident()
        {
            throw new NotImplementedException();
        }
    }
}
