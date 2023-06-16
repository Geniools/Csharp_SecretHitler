
using System.Collections;
using System.Collections.ObjectModel;

namespace SecretHitlerShared
{
    public class Board
    {
        private readonly HashSet<PolicyCard> _drawDeck;
        public byte PlayedLiberalCards { get; private set; }
        public byte PlayedFascistsCards { get; private set; }

        public ObservableCollection<PolicyCard> LiberalPolicies { get; private set; }
        public ObservableCollection<PolicyCard> FascistPolicies { get; private set; }


        public Board()
        {
            _drawDeck = new HashSet<PolicyCard>();

            this.LiberalPolicies = new ObservableCollection<PolicyCard>();
            this.FascistPolicies = new ObservableCollection<PolicyCard>();

            // Test functions
            this.AddTestCards();
        }

        public PolicyCard DrawNextPolicy()
        {
            throw new NotImplementedException();
        }

        private void EnactLiberalPolicy()
        {
            throw new NotImplementedException();
        }

        private void EnactFascistPolicy()
        {
            throw new NotImplementedException();
        }


        // Test functions
        private void AddTestCards()
        {
            // Add liberal Cards
            this.LiberalPolicies.Add(new PolicyCard(PartyMembership.Liberal));
            this.LiberalPolicies.Add(new PolicyCard(PartyMembership.Liberal));
            this.LiberalPolicies.Add(new PolicyCard(PartyMembership.Liberal));

            // Add fascist Cards
            this.FascistPolicies.Add(new PolicyCard(PartyMembership.Fascist));
            this.FascistPolicies.Add(new PolicyCard(PartyMembership.Fascist));
            this.FascistPolicies.Add(new PolicyCard(PartyMembership.Fascist));
        }
    }
}
