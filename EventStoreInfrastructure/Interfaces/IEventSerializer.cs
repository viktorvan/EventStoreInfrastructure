using EventStore.ClientAPI;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventSerializer
    {
        EventData CreateEventData(object @event);
        object DeserializeEvent(RecordedEvent originalEvent);
        T DeserializeObject<T>(byte[] data);
        object DeserializeObject(byte[] data, string typeName);
    }
}