using SecretHitler.Model;
using SecretHitler.Services;

namespace SecretHitler
{
    public class GameManager : BindableObject
    {
        public SignalRService SignalRService { get; private set; }
        public Game game { get; private set; }

        public GameManager(SignalRService signalRService)
        {
            this.SignalRService = signalRService;

            // Create a new game
            Board board = new Board();
            this.game = new Game(board);
        }
    }
}
