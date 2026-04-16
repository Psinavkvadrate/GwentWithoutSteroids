using System.Collections.Generic;
using System.Linq;
using GwentWithoutSteroids.Core.Cards;

namespace GwentWithoutSteroids.Core.Decks
{
    public class Deck
    {
        private List<Card> _cards;
        private List<Card> _originalCards;

        public Deck(List<Card> cards)
        {
            _originalCards = cards.Select(c => c.Clone()).ToList();

            _cards = cards.Select(c => c.Clone()).ToList();
        }

        public List<Card> GetCards()
        {
            return _cards;
        }

        public void Reset()
        {
            _cards = _originalCards
                .Select(c => c.Clone())
                .ToList();
        }
    }
}