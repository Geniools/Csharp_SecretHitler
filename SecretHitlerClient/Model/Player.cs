
namespace SecretHitler.Model
{
    public class Player
    {
        private string _username;
        private SecretRole _role;
        private PartyMembership _party;


        public bool IsHitler()
        {
            return this._role is SecretRole.Hitler;
        }
    }
}
