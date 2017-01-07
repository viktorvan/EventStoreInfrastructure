using System;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreSettings
    {

        string EventStoreHostname { get; }
        string EventStorePassword { get; }
        int EventStorePort { get; }
        string EventStoreUser { get; }
        string Domain { get; }
        TimeSpan ProjectionTimeout { get; }
    }
}