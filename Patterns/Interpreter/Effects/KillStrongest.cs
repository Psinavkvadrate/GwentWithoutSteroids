using System.Linq;
using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
{
    public class KillStrongest : IExpression
    {
        public void Interpret(GameContext context)
        {
            var card = BoardUtils.GetOpponentCards(context)
                .OrderByDescending(c => c.Power)
                .FirstOrDefault();

            if (card != null)
                Remove(card, context);
        }

        private void Remove(Card card, GameContext context)
        {
            foreach (var row in context.Opponent.Board.GetRows().Values)
            {
                row.RemoveCard(card);
            }
        }
    }
}