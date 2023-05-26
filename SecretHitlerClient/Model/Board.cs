
using System.Collections;
using System.Collections.ObjectModel;

namespace SecretHitler.Model
{
    public class Board
    {
        private readonly HashSet<PolicyCard> _drawDeck;
        public byte PlayedLiberalCards { get; private set; }
        public byte PlayedFascistsCards { get; private set; }

        public ObservableCollection<PolicyCard> ElectedPolicies { get; private set; }


        public Board()
        {
            _drawDeck = new HashSet<PolicyCard>();
            this.ElectedPolicies = new ObservableCollection<PolicyCard>();
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
