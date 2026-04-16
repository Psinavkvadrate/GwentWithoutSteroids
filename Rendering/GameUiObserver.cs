using GwentWithoutSteroids.Patterns.Observer;
using SFML.Graphics;
using System.Collections.Generic;

namespace GwentWithoutSteroids.Rendering
{
    public class GameUiObserver : IObserver
    {
        private string _lastMessage = "";
        private List<string> _log = new();
        public List<string> GetLog() => _log;

        public void OnEvent(GameEvent gameEvent)
        {
            _lastMessage = gameEvent.Message;
            _log.Add(_lastMessage);

            if (_log.Count > 10)
                _log.RemoveAt(0);
        }

        public string GetLastMessage() => _lastMessage;
    }
}