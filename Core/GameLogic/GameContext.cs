using GwentLikeGame.Patterns.Observer;

namespace GwentLikeGame.Core.GameLogic
{
    public class GameContext
    {
        public Players.Player CurrentPlayer { get; set; }
        public Players.Player Opponent { get; set; }

        public Game Game { get; set; }
    }
}