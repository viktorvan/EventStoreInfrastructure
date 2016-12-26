using System;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
