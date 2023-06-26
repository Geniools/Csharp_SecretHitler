
namespace SecretHitlerShared
{
    public class GameStatus
    {
        public HashSet<Player> PlayersEligiblePresident { get; set; }
        public HashSet<Player> PlayersEligibleChancelor { get; set; }

        public Player CurrentPresident { get; set; }
        public Player PreviousPresident { get; set; }

        public Player CurrentChancelor { get; set; }
        public Player PreviousChancelor { get; set; }

        public PresidentialsPowers PresidentialPower { get; set; }

        public GameStatus()
        {
            this.PlayersEligiblePresident = new HashSet<Player>();
            this.PlayersEligibleChancelor = new HashSet<Player>();
        }
    }
}
