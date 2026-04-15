using System.Collections.Generic;
using GwentLikeGame.Core.Cards;

namespace GwentLikeGame.Core.Decks
{
    public class Deck
    {
        private List<Card> _cards;

        public Deck(List<Card> cards)
        {
            _cards = cards;
        }

        public List<Card> GetCards()
        {
            return _cards;
        }
    }
}