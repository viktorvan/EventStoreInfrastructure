﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<IEvent>> ReadAllEventsAsync(int startPosition, string projectionBuilder, string stream)
        {
            _projectionBuilderName = projectionBuilder;
            var events = await ReadAllEventsAsync(stream, startPosition);

            return events.Select(x => _serializer.DeserializeEvent(x.OriginalEvent) as IEvent).ToList();
        }

        private async Task<List<ResolvedEvent>> ReadAllEventsAsync(string stream, int startPosition)
        {
            Log.Information(
                "ProjectionBuilder {name} reading all events from store {eventStore} from position {position}",
                _projectionBuilderName, _service.ConnectionName, startPosition);

            var events = await _service.ReadStreamEventsAsync(stream, startPosition);
            _lastPosition = _service.CurrentPosition;
            return events;
        }
    }

    public interface IEventStoreConnectionService
    {
        Task<List<ResolvedEvent>> ReadStreamEventsAsync(string streamName, int startPosition);
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

        public async Task<List<ResolvedEvent>> ReadStreamEventsAsync(string streamName, int startPosition)
        {
            var streamEvents = new List<ResolvedEvent>();
            _currentPosition = startPosition;
            StreamEventsSlice currentSlice;
            do
            {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(streamName, _currentPosition, 200, true);
                _currentPosition = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return streamEvents;
        }
    }
}