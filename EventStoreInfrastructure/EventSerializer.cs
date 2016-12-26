using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using EventStoreInfrastructure.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace EventStoreInfrastructure
{
    public class EventSerializer : IEventSerializer
    {
        private const string EventTypeHeader = "EventTypeName";
        private readonly string _domain;

        public EventSerializer(string domain)
        {
            _domain = domain;
        }

        public EventData CreateEventData(object @event)
        {
            var eventHeaders = new Dictionary<string, string>()
            {
                {
                    EventTypeHeader, @event.GetType().AssemblyQualifiedName
                },
                {
                    "Domain", _domain
                }
            };
            var eventDataHeaders = SerializeObject(eventHeaders);
            var data = SerializeObject(@event);
            var eventData = new EventData(Guid.NewGuid(), @event.GetType().Name, true, data, eventDataHeaders);
            return eventData;
        }

        private byte[] SerializeObject(object obj)
        {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(jsonObj);
            return data;
        }

        public object DeserializeEvent(RecordedEvent originalEvent)
        {
            if (originalEvent.Metadata != null)
            {
                var metadata = DeserializeObject<Dictionary<string, string>>(originalEvent.Metadata);

                if (metadata != null && metadata.ContainsKey("Domain") && metadata["Domain"] != _domain)
                {
                    Log.Information("Event domain {domain}, expected domain {expectedDomain}", metadata["Domain"], _domain);                    
                }

                if (metadata == null || !metadata.ContainsKey(EventTypeHeader) || metadata["Domain"] != _domain)
                {
                    return null;
                }

                return DeserializeObject(originalEvent.Data, metadata[EventTypeHeader]);
            }
            return null;
        }

        public T DeserializeObject<T>(byte[] data)
        {
            return (T) (DeserializeObject(data, typeof (T).AssemblyQualifiedName));
        }

        public object DeserializeObject(byte[] data, string typeName)
        {
            var jsonString = Encoding.UTF8.GetString(data);
            try
            {
                return JsonConvert.DeserializeObject(jsonString, Type.GetType(typeName));
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

    }
}
