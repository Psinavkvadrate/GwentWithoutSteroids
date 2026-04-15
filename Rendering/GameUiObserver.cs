using GwentLikeGame.Patterns.Observer;
using SFML.Graphics;
using System.Collections.Generic;

namespace GwentLikeGame.Rendering
{
    public class GameUiObserver : IObserver
    {
        private string _lastMessage = "";
        private List<string> _log = new();

        public void OnEvent(GameEvent gameEvent)
        {
            _lastMessage = gameEvent.Message;
            _log.Add(_lastMessage);

            if (_log.Count > 10)
                _log.RemoveAt(0);
        }

        public string GetLastMessage() => _lastMessage;
        public List<string> GetLog() => _log;
    }
}