using System;
using System.Collections.Generic;
using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Decks;
using GwentWithoutSteroids.Core.Board.Components;

namespace GwentWithoutSteroids.Core.Players
{
    public class Player
    {
        public string Name { get; }
        public Deck Deck { get; private set; }
        public List<Card> Hand { get; private set; } = new();
        public BoardComposite Board { get; } = new();

        public bool HasPassed { get; set; }

        public Player(string name, Deck deck)
        {
            Name = name;
            Deck = deck;
        }

        public void Draw(int count)
        {
            var rnd = new Random();
            var cards = Deck.GetCards();

            for (int i = 0; i < count && cards.Count > 0; i++)
            {
                var index = rnd.Next(cards.Count);

                var template = cards[index];

                Hand.Add(template.Clone()); 

                cards.RemoveAt(index);       
            }
        }

        public void PlayCard(Card card)
        {
            if (card is UnitCard unit)
            {
                Board.GetRow(unit.Row).Add(new CardComponent(card));
            }
        }
    }
}