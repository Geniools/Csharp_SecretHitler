
namespace SecretHitlerShared
{
    public class GameStatus
    {
        private LinkedList<Player> PlayingPlayers { get; set; }

        public Player CurrentPresident { get; set; }
        public Player PreviousPresident { get; set; }

        public Player CurrentChancelor { get; set; }
        public Player PreviousChancelor { get; set; }
        public Player CandidateChancellor { get; set; }

        public PresidentialsPowers PresidentialPower { get; set; }
        public PlayerSelectionStatus PlayerSelectionStatus { get; set; }

        public GameStatus()
        {
            this.PlayingPlayers = new LinkedList<Player>();
        }

        public void AddPlayer(Player player)
        {
            this.PlayingPlayers.AddLast(player);
        }

        public void RemovePlayer(Player player)
        {
            this.PlayingPlayers.Remove(player);
        }

        public Player EnactCandidateChancellor()
        {
            this.PreviousChancelor = this.CurrentChancelor;
            this.CurrentChancelor = this.CandidateChancellor;
            return this.CurrentChancelor;
        }

        public Player GetNextPresident()
        {
            // If there are no players eligible for president, then throw an exception
            if (this.PlayingPlayers.Count == 0)
            {
                throw new InvalidOperationException("No players are eligible for president");
            }

            // If there is no current president, then the first player in the list is the president
            if (this.CurrentPresident is null)
            {
                this.CurrentPresident = this.PlayingPlayers.First.Value;
                return this.CurrentPresident;
            }

            // If there is a current president, then the next player in the list is the president
            LinkedListNode<Player> nextPresident = this.PlayingPlayers.Find(this.CurrentPresident).Next;
            // If the next president is null, then the first player in the list is the president
            if (nextPresident is null)
            {
                nextPresident = this.PlayingPlayers.First;
            }

            this.CurrentPresident = nextPresident.Value;
            return this.CurrentPresident;
        }
    }
}
