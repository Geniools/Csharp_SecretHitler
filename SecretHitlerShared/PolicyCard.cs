
namespace SecretHitlerShared
{
    public class PolicyCard
    {
        private PartyMembership _party;
        public PartyMembership Party {
            get
            {
                return this._party;
            }
            private set
            {
                if (value is PartyMembership.Fascist)
                {
                    this.Image = GameImages.FascistArticle;
                }
                else
                {
                    this.Image = GameImages.LiberalArticle;
                }

                this._party = value;
            }
        }
        public string Image { get; private set; }

        public PolicyCard(PartyMembership party)
        {
            this.Party = party;
        }

        public override string ToString()
        {
            if(this.Party is PartyMembership.Fascist)
            {
                return "Fascist";
            }
            else
            {
                return "Liberal";
            }
        }
    }
}
