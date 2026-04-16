namespace GwentWithoutSteroids.Patterns.Observer
{
    public interface IObserver
    {
        void OnEvent(GameEvent gameEvent);
    }
}