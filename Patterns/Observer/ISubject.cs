using System.Collections.Generic;

namespace GwentWithoutSteroids.Patterns.Observer
{
    public interface ISubject
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void Notify(GameEvent gameEvent);
    }
}