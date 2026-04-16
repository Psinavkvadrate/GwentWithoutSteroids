using System.Collections.Generic;
using System.Linq;
using GwentWithoutSteroids.Core.Cards;

namespace GwentWithoutSteroids.Core.Board.Components
{
    public class Row : IBoardComponent
    {
        private readonly List<IBoardComponent> _children = new();

        public void Add(IBoardComponent component)
        {
            _children.Add(component);
        }

        public void Remove(IBoardComponent component)
        {
            _children.Remove(component);
        }

        public List<IBoardComponent> GetChildren() => _children;

        public int GetPower()
        {
            return _children.Sum(c => c.GetPower());
        }

        public void Clear()
        {
            _children.Clear();
        }

        public IEnumerable<Card> GetCards()
        {
            return _children
                .OfType<CardComponent>()
                .Select(c => c.Card);
        }

        public void RemoveCard(Card card)
        {
            var target = _children
                .OfType<CardComponent>()
                .FirstOrDefault(c => c.Card == card);

            if (target != null)
                _children.Remove(target);
        }
    }
}