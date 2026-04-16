using System.Linq;
using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Patterns.Interpreter.Effects
{
    public class BoostRowAdd : IExpression
    {
        private int _value;
        private RowType _row;

        public BoostRowAdd(int value, RowType row)
        {
            _value = value;
            _row = row;
        }

        public void Interpret(GameContext context)
        {
            var row = context.CurrentPlayer.Board.GetRow(_row);

            foreach (var card in row.GetCards())
                card.Power += _value;
        }
    }
}