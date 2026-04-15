using System.Linq;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
{
    public class BoostRowAdd : IExpression
    {
        private int _value;

        public BoostRowAdd(int value)
        {
            _value = value;
        }

        public void Interpret(GameContext context)
        {
            var row = context.Opponent.Board.GetRows().First().Value;

            foreach (var card in row.GetCards())
            {
                card.Power += _value;
            }
        }
    }
}