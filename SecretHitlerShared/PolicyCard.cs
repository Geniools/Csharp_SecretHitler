
namespace SecretHitlerShared
{
    public class PolicyCard
    {
        public PartyMembership Party { get; private set; }

        public PolicyCard(PartyMembership party)
        {
            this.Party = party;
        }

        public override string ToString()
        {
            return this.Party.ToString();
        }
    }
}
