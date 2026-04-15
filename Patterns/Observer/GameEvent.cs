namespace GwentLikeGame.Patterns.Observer
{
    public enum GameEventType
    {
        RoundStarted,
        CardPlayed,
        TurnChanged,
        RoundEnded,
        GameEnded
    }

    public class GameEvent
    {
        public GameEventType Type { get; }

        public string Message { get; }

        public int PlayerScore { get; }
        public int AiScore { get; }

        public GameEvent(GameEventType type, string message = "", int playerScore = 0, int aiScore = 0)
        {
            Type = type;
            Message = message;
            PlayerScore = playerScore;
            AiScore = aiScore;
        }
    }
}