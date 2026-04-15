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
            var all = BoardUtils.GetOpponentCards(context);

            if (!all.Any())
                return;

            var weakest = all
                .OrderBy(c => c.Power)
                .Take(_count)
                .ToList();

            int threshold = weakest.Max(c => c.Power);

            var targets = all
                .Where(c => c.Power <= threshold)
                .ToList();

            foreach (var card in targets)
            {
                foreach (var row in context.Opponent.Board.GetRows().Values)
                {
                    row.RemoveCard(card);
                }
            }
        }
    }
}