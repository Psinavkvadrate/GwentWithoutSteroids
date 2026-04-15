using System;
using System.Linq;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter.Effects
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
            var cards = BoardUtils.GetOpponentCards(context)
                .OrderBy(_ => _rnd.Next())
                .Take(_count);

            foreach (var c in cards)
                c.Power = 1;
        }
    }
}