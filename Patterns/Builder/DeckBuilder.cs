using System.Collections.Generic;
using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Core.Decks;
using GwentWithoutSteroids.Patterns.Factory;

namespace GwentWithoutSteroids.Patterns.Builder
{
    public class DeckBuilder
    {
        private List<Card> _cards = new();
        private readonly ICardFactory _factory;

        public DeckBuilder(ICardFactory factory)
        {
            _factory = factory;
        }

        public DeckBuilder AddMelee(string name, int power)
        {
            _cards.Add(_factory.CreateUnit(name, power, RowType.Melee));
            return this;
        }

        public DeckBuilder AddRanged(string name, int power)
        {
            _cards.Add(_factory.CreateUnit(name, power, RowType.Ranged));
            return this;
        }

        public DeckBuilder AddSiege(string name, int power)
        {
            _cards.Add(_factory.CreateUnit(name, power, RowType.Siege));
            return this;
        }

        public DeckBuilder AddSkill(string name, string effectId)
        {
            _cards.Add(_factory.CreateSkill(name, effectId));
            return this;
        }

        public Deck Build()
        {
            return new Deck(_cards);
        }

        public DeckBuilder Reset()
        {
            _cards = new List<Card>();
            return this;
        }
    }
}