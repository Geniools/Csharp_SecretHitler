
namespace SecretHitlerShared
{
    public class Player
    {
        // Hub properties
        public string LobbyCode { get; set; }
        public string ConnectionId { get; set; }

        // Game properties
        public string Username { get; private set; }
        public SecretRole Role { get; set; }
        public PartyMembership Party { get; set; }
        public string ImageSource { get; set; }

        public Player(
            string username, string lobbyCode = "", string imageSource = "check.png", 
            SecretRole role = SecretRole.Liberal, PartyMembership party = PartyMembership.Liberal
        )
        {
            // Hub properties
            this.LobbyCode = lobbyCode;
            this.ConnectionId = string.Empty;

            // Game properties
            this.Username = username;
            this.ImageSource = imageSource;

            // By default everyone is a liberal
            this.Role = role;
            this.Party = party;
        }

        public bool IsHitler()
        {
            return this.Role is SecretRole.Hitler;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Player player)
            {
                return this.Username.Trim().ToLower().Equals(player.Username.Trim().ToLower());
            }

            return false;
        }
    }
}