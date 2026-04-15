namespace GwentLikeGame.Patterns.Observer
{
    public enum GameEventType
    {
        RoundStarted,
        CardPlayed,
        CardReplaced,   // ✅ добавили
        TurnChanged,
        PlayerPassed,   // ✅ добавили
        RoundEnded,
        GameEnded,
        Info            // ✅ добавили (для текстовых сообщений)
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