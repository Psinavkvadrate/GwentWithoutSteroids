using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Patterns.Interpreter
{
    public interface IExpression
    {
        void Interpret(GameContext context);
    }
}