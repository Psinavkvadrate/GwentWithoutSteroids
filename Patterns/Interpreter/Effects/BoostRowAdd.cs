using System.Linq;
using GwentLikeGame.Core.Board;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
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