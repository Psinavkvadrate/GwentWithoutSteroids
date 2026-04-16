using System;
using System.Linq;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Patterns.Interpreter.Effects
{
    public class WeakenRandom : IExpression
    {
        private int _count;
        private Random _rnd = new();

        public WeakenRandom(int count)
        {
            _count = count;
        }

        public void Interpret(GameContext context)
        {
            var all = BoardUtils
                .GetAllCards(context.CurrentPlayer)
                .Concat(BoardUtils.GetAllCards(context.Opponent))
                .OrderBy(_ => _rnd.Next())
                .Take(_count);

            foreach (var c in all)
                c.Power = 1;
        }
    }
}