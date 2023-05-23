
namespace SecretHitlerShared
{
    public class Player
    {
        public string Username { get; set; }
        public string LobbyCode { get; set; }

        public Player(string username, string lobbyCode)
        {
            this.Username = username;
            this.LobbyCode = lobbyCode;
        }
    }
}
