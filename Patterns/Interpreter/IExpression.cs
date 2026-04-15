using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Patterns.Interpreter
{
    public interface IExpression
    {
        void Interpret(GameContext context);
    }
}