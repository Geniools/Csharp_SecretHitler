
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SecretHitler.Model
{
    public class Game
    {
        public ObservableCollection<Player> Players { get; }
        private Board _board;
        private Chat _chat;
        private GameStatus _gameStatus;
        private Player _winner;
        private Player _currentPresident;
        private Player _currentChancelor;
        private byte _electionTracker;

        public Game(Board board)
        {
            this._board = board;
            this.Players = new ObservableCollection<Player>();
            this._chat = new Chat();
            this._gameStatus = new GameStatus();
        }

        public void StartGame()
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
