
namespace SecretHitler.Model
{
    public class Player
    {
        public string Username { get; private set; }
        public SecretRole Role { get; set; }
        public PartyMembership Party { get; set; }
        public string ImageSource { get; set; }

        public Player(string username, string imageSource = "check.png", SecretRole role = SecretRole.Liberal, PartyMembership party = PartyMembership.Liberal)
        {
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
    }
}