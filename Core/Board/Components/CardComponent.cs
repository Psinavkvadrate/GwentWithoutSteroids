using GwentLikeGame.Core.Cards;

namespace GwentLikeGame.Core.Board.Components
{
    public class CardComponent : IBoardComponent
    {
        public Card Card { get; }

        public CardComponent(Card card)
        {
            Card = card;
        }

        public int GetPower()
        {
            return Card.Power;
        }
    }
}