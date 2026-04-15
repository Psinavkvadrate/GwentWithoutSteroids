using System.Linq;
using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Core.Players;

namespace GwentLikeGame.Patterns.Interpreter.Effects
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
                // ничья → убиваем у обоих
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