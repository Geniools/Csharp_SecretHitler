
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SecretHitlerShared
{
    public class Board
    {
        private Stack<PolicyCard> _drawDeck;
        private List<PolicyCard> _discardDeck;

        public Dictionary<Player, BallotType> VotingResults { get; private set; }
        public List<PolicyCard> PolicyCardsResults { get; set; }

        public byte PlayedLiberalCards { get; set; }
        public byte PlayedFascistsCards { get; set; }

        public Board()
        {
            this._drawDeck = new Stack<PolicyCard>();
            this._discardDeck = new List<PolicyCard>();

            this.VotingResults = new Dictionary<Player, BallotType>();
            this.PolicyCardsResults = new List<PolicyCard>();

            this.CreateDeck();
        }

        private void CreateDeck()
        {
            // First check if the discard pile is empty
            if (this._discardDeck.Count == 0)
            {
                // 11 fascist policies
                for (byte i = 0; i < 11; i++)
                {
                    this._drawDeck.Push(new PolicyCard(PartyMembership.Fascist));
                }

                // 6 liberal policies
                for (byte i = 0; i < 6; i++)
                {
                    this._drawDeck.Push(new PolicyCard(PartyMembership.Liberal));
                }
            }
            else
            {
                PolicyCard[] discardedCards = this._discardDeck.ToArray();
                PolicyCard[] leftDeckCards = this._drawDeck.ToArray();

                // Combine the two arrays
                PolicyCard[] allCards = discardedCards.Concat(leftDeckCards).ToArray();
                this._drawDeck = new Stack<PolicyCard>(allCards);
            }

            // Shuffle the deck
            this.ShuffleDeck();
        }

        public void AddToDiscard(PolicyCard card)
        {
            this._discardDeck.Add(card);
        }

        private void ShuffleDeck()
        {
            PolicyCard[] cards = this._drawDeck.ToArray(); 
            Random random = new Random();
            int n = cards.Length;
            for( int i = 0; i < (n - 1); i++)
            {
                int r = i + random.Next(n - i);
                PolicyCard card = cards[r];
                cards[r] = cards[i];
                cards[i] = card;
            }
            this._drawDeck = new Stack<PolicyCard>(cards);
        }

        public PolicyCard DrawNextPolicy()
        {
            if( this._drawDeck.Count == 0 )
            {
                CreateDeck();
            }

            return this._drawDeck.Pop();
        }

        public PolicyCard[] PeekThreePolicies()
        {
            PolicyCard[] cards = new PolicyCard[3];
            if( this._drawDeck.Count < 3 )
            {
                CreateDeck();
            }
            for ( int i = 0; i < 3; i++ )
            {
                cards[i] = _drawDeck.Pop();
            }
            for( int i = 0; i < 3; i++ )
            {
                _drawDeck.Push(cards[i]);
            }
            return cards;
        }

        public PolicyCard[] GetThreePolicies()
        {
            PolicyCard[] cards = new PolicyCard[3];
            if (this._drawDeck.Count < 3)
            {
                CreateDeck();
            }
            for (int i = 0; i < 3; i++)
            {
                cards[i] = _drawDeck.Pop();
            }
            return cards;
        }
    }
}
