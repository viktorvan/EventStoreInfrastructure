using System;
using System.Collections.Generic;
using EventStoreInfrastructure.Interfaces;

namespace EventStoreInfrastructure
{
    public class AggregateBase : IAggregate 
    {
        public int Version { get; protected set; } = -1;

        public Guid Id { get; protected set; }

        private readonly List<IEvent> _uncommitedEvents = new List<IEvent>();
        private readonly Dictionary<Type, Action<IEvent>> _routes = new Dictionary<Type, Action<IEvent>>();

        public void RaiseEvent(IEvent @event)
        {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

        protected void RegisterTransition<T>(Action<T> transition) where T : class, IEvent
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }

        public void ApplyEvent(IEvent @event)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event);
            }
            Version++;
        }

        public IEnumerable<IEvent> UncommitedEvents()
        {
            return _uncommitedEvents;
        }

        public void ClearUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }
    }
}
