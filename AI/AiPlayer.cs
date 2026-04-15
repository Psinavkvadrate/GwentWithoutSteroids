using GwentLikeGame.Core.Players;
using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.AI
{
    public class AiPlayer
    {
        private readonly AiEvaluator _evaluator = new();

        public void MakeMove(Player ai, Player opponent)
        {
            var action = _evaluator.Evaluate(ai, opponent);

            if (action.Type == AiActionType.Pass)
            {
                ai.HasPassed = true;
                return;
            }

            var card = action.Card;
            ai.Hand.Remove(card);

            card.Play(new GameContext
            {
                CurrentPlayer = ai,
                Opponent = opponent
            });
        }
    }
}