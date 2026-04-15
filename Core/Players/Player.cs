using System;
using System.Collections.Generic;
using GwentLikeGame.Core.Board;
using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.Decks;
using GwentLikeGame.Core.Board.Components;

namespace GwentLikeGame.Core.Players
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

                // 🔥 КЛЮЧЕВОЕ ИЗМЕНЕНИЕ
                Hand.Add(template.Clone());  // ✅ кладём КОПИЮ

                cards.RemoveAt(index);       // можно оставить (как "вытянули карту")
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