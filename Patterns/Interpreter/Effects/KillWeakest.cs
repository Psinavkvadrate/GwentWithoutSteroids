using System.Linq;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
{
    public class KillWeakest : IExpression
    {
        private int _count;

        public KillWeakest(int count)
        {
            _count = count;
        }

        public void Interpret(GameContext context)
        {
            var cards = BoardUtils.GetOpponentCards(context)
                .OrderBy(c => c.Power)
                .Take(_count)
                .ToList();

            foreach (var card in cards)
            {
                foreach (var row in context.Opponent.Board.GetRows().Values)
                {
                    row.RemoveCard(card);
                }
            }
        }
    }
}