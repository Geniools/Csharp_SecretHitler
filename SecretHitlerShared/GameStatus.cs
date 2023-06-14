
namespace SecretHitlerShared
{
    public class GameStatus
    {
        private readonly HashSet<Player> _playersEligibleChancelor;
        private readonly HashSet<Player> _playersEligiblePresident;
        private Player _previousPresident;
        private Player _previousChancelor;
        private PresidentialsPowers _presidentialsPowers;

        public GameStatus()
        {
            _playersEligibleChancelor = new HashSet<Player>();
            _playersEligiblePresident = new HashSet<Player>();
        }
    }
}
