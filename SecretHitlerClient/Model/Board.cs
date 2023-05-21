
namespace SecretHitler.Model
{
    class Board
    {
        private readonly HashSet<PolicyCard> _drawDeck;
        private byte _playedLiberalCards;
        private byte _playedFascistCards;


        public Board()
        {
            _drawDeck = new HashSet<PolicyCard>();
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
