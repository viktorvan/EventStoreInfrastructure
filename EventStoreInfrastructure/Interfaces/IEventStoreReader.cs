using System.Collections.Generic;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreReader
    {
        List<IEvent> ReadAllEvents(int startPosition, string projectionBuilder, string stream);
        int LastPosition { get; }
    }
}