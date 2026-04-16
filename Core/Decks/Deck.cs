using System.Collections.Generic;
using GwentWithoutSteroids.Core.Cards;

namespace GwentWithoutSteroids.Core.Decks
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