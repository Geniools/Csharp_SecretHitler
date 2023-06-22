
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

            this.CreateDeck();
        }

        private void CreateDeck()
        {
            // 11 fascist policies
            for(byte i = 0; i < 11; i++ )
            {
                this._drawDeck.Add(new PolicyCard(PartyMembership.Fascist));
            }

            // 6 liberal policies
            for(byte i = 0; i < 6; i++ )
            {
                this._drawDeck.Add(new PolicyCard(PartyMembership.Liberal));
            }

            // Shuffle the deck
            //this.ShuffleDeck();
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
    }
}
