using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStoreInfrastructure.Exceptions;
using EventStoreInfrastructure.Interfaces;

namespace EventStoreInfrastructure
{
    public class EventStoreDomainRepository : DomainRepositoryBase
    {
        private string _eventTypeHeader = "EventTypeName";

        private readonly IEventStoreConnection _connection;
        private readonly IEventSerializer _serializer;
        private readonly string _category;


        public EventStoreDomainRepository(IEventStoreConnectionFactory factory, IEventSerializer serializer, string category)
        {
            _serializer = serializer;
            _category = category;
            _connection = factory.CreateAsync().Result;
        }

        public override async Task<IEnumerable<IEvent>> SaveAsync<TAggregate>(TAggregate aggregate)
        {
            var events = aggregate.UncommitedEvents().ToList();
            var expectedVersion = CalculateExpectedVersion(aggregate, events);
            var eventData = events.Select(_serializer.CreateEventData);
            var streamName = AggregateToStreamName(aggregate.GetType(), aggregate.Id);
            await _connection.AppendToStreamAsync(streamName, expectedVersion, eventData);
            return events;
        }

        public override async Task<TResult> GetByIdAsync<TResult>(Guid id)
        {
            var streamName = AggregateToStreamName(typeof(TResult), id);

            var streamEvents = new List<IEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);
                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                {
                    throw new AggregateNotFoundException("Could not find aggregate of type " + typeof(TResult) + " and id " + id);
                }

                nextSliceStart = currentSlice.NextEventNumber;

                var events = currentSlice.Events.Select(e =>
                {
                    var metadata = _serializer.DeserializeObject<Dictionary<string, string>>(e.OriginalEvent.Metadata);
                    var eventData = _serializer.DeserializeObject(e.OriginalEvent.Data, metadata[_eventTypeHeader]);
                    return eventData as IEvent;
                });

                streamEvents.AddRange(events);
            } while (!currentSlice.IsEndOfStream);

            return BuildAggregate<TResult>(streamEvents);
        }

        

        private string AggregateToStreamName(Type type, Guid id)
        {
            return $"{_category}-{type.Name}-{id}";
        }
    }
}
