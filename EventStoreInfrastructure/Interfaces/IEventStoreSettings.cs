using System;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreSettings
    {

        string EventStoreHostname { get; }
        string EventStorePassword { get; }
        int EventStoreTcpPort { get; }
        int EventStoreHttpPort { get; }
        string EventStoreUser { get; }
        string Domain { get; }
        TimeSpan ProjectionTimeout { get; }
    }
}