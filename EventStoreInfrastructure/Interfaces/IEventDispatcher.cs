namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent @event);
    }
}