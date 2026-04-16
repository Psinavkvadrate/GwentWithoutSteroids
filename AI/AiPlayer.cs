using GwentWithoutSteroids.Core.Players;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.AI
{
    public class AiPlayer
    {
        private readonly AiEvaluator _evaluator = new();

        public void MakeMove(Player ai, Player opponent, Game game)
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
                Opponent = opponent,
                Game = game
            });
        }
    }
}