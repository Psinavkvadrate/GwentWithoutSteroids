using System.Collections.Generic;

namespace GwentLikeGame.Patterns.Observer
{
    public interface ISubject
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void Notify(GameEvent gameEvent);
    }
}