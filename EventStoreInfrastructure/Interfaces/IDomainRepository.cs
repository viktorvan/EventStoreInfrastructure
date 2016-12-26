using System;
using System.Collections.Generic;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IDomainRepository
    {
        IEnumerable<IEvent> Save<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        TResult GetById<TResult>(Guid id) where TResult : IAggregate, new();
    }
}
