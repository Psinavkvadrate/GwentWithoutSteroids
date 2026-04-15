using System;
using GwentLikeGame.Core.Board;
using GwentLikeGame.Patterns.Interpreter.Effects;

namespace GwentLikeGame.Patterns.Interpreter
{
    public static class EffectParser
    {
        public static IExpression Parse(string effectId)
        {
            return effectId switch
            {
                "boost_row_x2" => new BoostRowMultiply(2, RowType.Melee),
                "boost_row_2" => new BoostRowAdd(2, RowType.Melee),
                "kill_strongest" => new KillStrongest(),
                "kill_weakest" => new KillWeakest(2),
                "kill_all_strongest" => new KillStrongestAll(),
                "weaken_random_3" => new WeakenRandom(3),
                _ => throw new Exception($"Unknown effect: {effectId}")
            };
        }
    }
}