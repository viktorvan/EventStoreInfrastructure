using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IDomainRepository
    {
        Task<IEnumerable<IEvent>> SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        Task<TResult> GetByIdAsync<TResult>(Guid id) where TResult : IAggregate, new();
    }
}
