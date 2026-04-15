using System.Linq;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Core.Cards;

namespace GwentLikeGame.AI
{
    public class AiEvaluator
    {
        public AiAction Evaluate(Player ai, Player opponent)
        {
            int aiPower = ai.Board.GetPower();
            int opponentPower = opponent.Board.GetPower();

            // 1. Если уже выигрываем сильно → пас
            if (aiPower > opponentPower + 10)
                return AiAction.Pass();

            // 2. Если у нас нет карт → пас
            if (!ai.Hand.Any())
                return AiAction.Pass();

            // 3. Если мы проигрываем → ищем сильную карту
            if (aiPower < opponentPower)
            {
                var best = ai.Hand.OrderByDescending(c => c.Power).First();
                return AiAction.Play(best);
            }

            // 4. Если почти равенство → средняя карта (не тратить сильные)
            var mid = ai.Hand
                .OrderBy(c => c.Power)
                .ElementAt(ai.Hand.Count / 2);

            return AiAction.Play(mid);
        }
    }
}