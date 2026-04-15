using System;

namespace GwentLikeGame.Patterns.Observer
{
    public class ConsoleGameLogger : IObserver
    {
        public void OnEvent(GameEvent gameEvent)
        {
            Console.WriteLine($"[{gameEvent.Type}] {gameEvent.Message}");

            if (gameEvent.Type == GameEventType.RoundEnded)
                Console.WriteLine($"Score: {gameEvent.PlayerScore} - {gameEvent.AiScore}");
        }
    }
}