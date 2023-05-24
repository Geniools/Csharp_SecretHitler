using SecretHitler.Model;
using SecretHitler.Services;
using System.Collections.ObjectModel;

namespace SecretHitler
{
    public class GameManager : BindableObject
    {
        public SignalRService SignalRService { get; private set; }

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
            this.SignalRService = new SignalRService();
            // Subscribe to events
            SignalRService.PlayerConnected += this.AddPlayer;

            // Create a new game
            this.Board = new Board();
            this.Players = new ObservableCollection<Player>();
            this.Chat = new Chat();
            this.GameStatus = new GameStatus();
            this.ElectionTracker = 0;
        }

        private void AddPlayer(Player player)
        {
            this.Players.Add(player);
        }

        public async Task StartGame()
        {
            throw new NotImplementedException();
        }

        public void EndGame()
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
