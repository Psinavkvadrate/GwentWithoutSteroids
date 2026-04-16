using System.Collections.Generic;
using System.Linq;
using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Core.Players;

namespace GwentWithoutSteroids.Core.GameLogic
{
    public static class BoardUtils
    {
        public static List<Card> GetAllCards(Player player)
        {
            return player.Board.GetRows()
                .SelectMany(r => r.Value.GetCards())
                .ToList();
        }

        public static List<Card> GetOpponentCards(GameContext context)
        {
            return GetAllCards(context.Opponent);
        }

        public static List<Card> GetPlayerCards(GameContext context)
        {
            return GetAllCards(context.CurrentPlayer);
        }

        public static List<Card> GetStrongest(GameContext context, int count)
        {
            return GetOpponentCards(context)
                .OrderByDescending(c => c.Power)
                .Take(count)
                .ToList();
        }

        public static List<Card> GetWeakest(GameContext context, int count)
        {
            return GetOpponentCards(context)
                .OrderBy(c => c.Power)
                .Take(count)
                .ToList();
        }
    }
}