using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreReader
    {
        Task<List<IEvent>> ReadAllEventsAsync(int startPosition, string projectionBuilder, string stream);
        int LastPosition { get; }
    }
}