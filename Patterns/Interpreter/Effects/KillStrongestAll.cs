using System.Linq;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Patterns.Interpreter.Effects
{
    public class KillStrongestAll : IExpression
    {
        public void Interpret(GameContext context)
        {
           var all = BoardUtils.GetOpponentCards(context);

            if (!all.Any())
                return;

            var max = all.Max(c => c.Power);

            var targets = BoardUtils.GetOpponentCards(context)
                .Where(c => c.Power == max)
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