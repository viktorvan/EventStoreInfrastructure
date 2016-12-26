using EventStoreInfrastructure.Interfaces;
using EventStoreInfrastructure.Tests.TestHelpers;
using FakeItEasy;
using Ploeh.AutoFixture.Xunit2;
using Serilog;
using Serilog.Events;
using Xunit;

namespace EventStoreInfrastructure.Tests
{
    public class EventStoreReaderTests
    {       

        [Theory, AutoFakeItEasyData]
        public void ReadAllEvents_ShouldLogReadingEvents(
            [Frozen] ILogger logger,
            [Frozen] IEventStoreConnectionService service,
            [Frozen] IEventDispatcher dispatcher,
            IEvent @event,
            int startPosition,
            string builderName,
            string streamName,
            EventStoreReader sut)
        {
            // arrange
            Log.Logger = logger;
            
            // act
            sut.ReadAllEvents(startPosition, builderName, streamName);

            // assert
            A.CallTo(() => logger.Write(LogEventLevel.Information, "ProjectionBuilder {name} reading all events from store {eventStore} from position {position}", builderName, service.ConnectionName, startPosition))
                .MustHaveHappened();
        }
    }
}