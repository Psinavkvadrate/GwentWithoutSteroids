using System.Linq;
using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.GameLogic;
using GwentWithoutSteroids.Core.Players;

namespace GwentWithoutSteroids.Patterns.Interpreter.Effects
{
    public class KillStrongest : IExpression
    {
        public void Interpret(GameContext context)
        {
            var enemyCards = BoardUtils.GetOpponentCards(context);
            var myCards = BoardUtils.GetPlayerCards(context);

            if (!enemyCards.Any() && !myCards.Any())
                return;

            int enemyMax = enemyCards.Any() ? enemyCards.Max(c => c.Power) : 0;
            int myMax = myCards.Any() ? myCards.Max(c => c.Power) : 0;

            if (enemyMax > myMax)
            {
                RemoveByPower(context.Opponent, enemyMax);
            }
            else if (myMax > enemyMax)
            {
                RemoveByPower(context.CurrentPlayer, myMax);
            }
            else
            {
                RemoveByPower(context.Opponent, enemyMax);
                RemoveByPower(context.CurrentPlayer, myMax);
            }
        }

        private void RemoveByPower(Player player, int power)
        {
            foreach (var row in player.Board.GetRows().Values)
            {
                var cards = row.GetCards()
                    .Where(c => c.Power == power)
                    .ToList();

                foreach (var c in cards)
                    row.RemoveCard(c);
            }
        }
    }
}