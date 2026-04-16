using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Patterns.Interpreter.Effects
{
    public class BoostRowMultiply : IExpression
    {
        private int _mult;
        private RowType _row;

        public BoostRowMultiply(int mult, RowType row)
        {
            _mult = mult;
            _row = row;
        }

        public void Interpret(GameContext context)
        {
            var row = context.CurrentPlayer.Board.GetRow(_row);

            foreach (var card in row.GetCards())
            {
                card.Power *= _mult;
            }
        }
    }
}