
namespace SecretHitlerShared
{
    public class Chat
    {
        private Dictionary<Player, string> _messages;

        public Chat()
        {
            this._messages = new Dictionary<Player, string>();
        }
    }
}
