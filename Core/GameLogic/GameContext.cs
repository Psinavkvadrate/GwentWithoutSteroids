using GwentWithoutSteroids.Patterns.Observer;

namespace GwentWithoutSteroids.Core.GameLogic
{
    public class GameContext
    {
        public Players.Player CurrentPlayer { get; set; }
        public Players.Player Opponent { get; set; }

        public Game Game { get; set; }
    }
}