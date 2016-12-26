using System;
using System.Collections.Generic;
using EventStoreInfrastructure.Interfaces;
using Serilog;

namespace EventStoreInfrastructure
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<Type, Action<IEvent>> _mapping = new Dictionary<Type, Action<IEvent>>();

        public void Dispatch(IEvent @event)
        {
            var type = @event.GetType();
            Log.Information("Trying to dispatch event {type}", type);
            if (!_mapping.ContainsKey(type))
            {
                Log.Information("No mapping for event {type} found", type);
                return;
            }

            Log.Information("Handling event {@event}", @event);
            _mapping[type](@event);
        }

        public void AddMapping(Type eventType, Action<IEvent> handler)
        {
            _mapping.Add(eventType, handler);
        }

        public Dictionary<Type, Action<IEvent>> GetMappings()
        {
            return _mapping;
        } 
    }
}
