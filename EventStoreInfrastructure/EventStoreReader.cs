using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using EventStoreInfrastructure.Interfaces;
using Serilog;

namespace EventStoreInfrastructure
{
    public class EventStoreReader : IEventStoreReader
    {
        private string _projectionBuilderName;
        private int _lastPosition;
        private readonly IEventSerializer _serializer;
        private readonly IEventStoreConnectionService _service;

        public EventStoreReader(IEventSerializer serializer, IEventStoreConnectionService service)
        {
            _serializer = serializer;
            _service = service;
        }

        public int LastPosition => _lastPosition;

        public List<IEvent> ReadAllEvents(int startPosition, string projectionBuilder, string stream)
        {
            _projectionBuilderName = projectionBuilder;
            var events = ReadAllEvents(stream, startPosition);

            return events.Select(x => _serializer.DeserializeEvent(x.OriginalEvent) as IEvent).ToList();
        }

        private List<ResolvedEvent> ReadAllEvents(string stream, int startPosition)
        {
            Log.Information(
                "ProjectionBuilder {name} reading all events from store {eventStore} from position {position}",
                _projectionBuilderName, _service.ConnectionName, startPosition);

            var events = _service.ReadStreamEvents(stream, startPosition);
            _lastPosition = _service.CurrentPosition;
            return events;
        }
    }

    public interface IEventStoreConnectionService
    {
        List<ResolvedEvent> ReadStreamEvents(string streamName, int startPosition);
        string ConnectionName { get; }
        int CurrentPosition { get; }
    }

    public class EventStoreConnectionService : IEventStoreConnectionService
    {
        private int _currentPosition;
        private readonly IEventStoreConnection _connection;

        public EventStoreConnectionService(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public string ConnectionName => _connection.ConnectionName;
        public int CurrentPosition => _currentPosition;

        public List<ResolvedEvent> ReadStreamEvents(string streamName, int startPosition)
        {
            var streamEvents = new List<ResolvedEvent>();
            _currentPosition = startPosition;
            StreamEventsSlice currentSlice;
            do
            {
                currentSlice = _connection.ReadStreamEventsForwardAsync(streamName, _currentPosition, 200, true).Result;
                _currentPosition = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return streamEvents;
        }
    }
}