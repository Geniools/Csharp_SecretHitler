
namespace SecretHitlerShared
{
    public class PlayerShared
    {
        public string Username { get; private set; }
        public string LobbyCode { get; private set; }

        public PlayerShared(string username, string lobbyCode)
        {
            this.Username = username;
            this.LobbyCode = lobbyCode;
        }
    }
}
