
namespace SecretHitler.Model
{
    public class Player
    {
        public string Username { get; private set; }
        public SecretRole Role { get; set; }
        public PartyMembership Party { get; set; }

        public Player(string username)
        {
            this.Username = username;
        }

        public bool IsHitler()
        {
            return this.Role is SecretRole.Hitler;
        }
    }
}