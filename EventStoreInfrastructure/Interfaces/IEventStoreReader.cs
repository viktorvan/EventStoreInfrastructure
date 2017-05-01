using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreReader
    {
        Task<List<IEvent>> ReadAllEventsAsync(long startPosition, string projectionBuilder, string stream);
        long LastPosition { get; }
    }
}