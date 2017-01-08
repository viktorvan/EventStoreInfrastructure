using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreInfrastructure.Interfaces;

namespace EventStoreInfrastructure
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        public abstract Task<IEnumerable<IEvent>> SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        public abstract Task<TResult> GetByIdAsync<TResult>(Guid id) where TResult : IAggregate, new();

        protected int CalculateExpectedVersion(IAggregate aggregate, List<IEvent> events)
        {
            return aggregate.Version - events.Count;
        }

        protected TResult BuildAggregate<TResult>(IEnumerable<IEvent> events) where TResult : IAggregate, new()
        {
            var result = new TResult();
            foreach (var @event in events)
            {
                result.ApplyEvent(@event);
            }
            return result;
        }
    }
}
