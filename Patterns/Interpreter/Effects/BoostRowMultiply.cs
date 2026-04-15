using System.Linq;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
{
    public class BoostRowMultiply : IExpression
    {
        private int _mult;

        public BoostRowMultiply(int mult)
        {
            _mult = mult;
        }

        public void Interpret(GameContext context)
        {
            var row = context.Opponent.Board.GetRows().First().Value;

            foreach (var card in row.GetCards())
            {
                card.Power *= _mult;
            }
        }
    }
}