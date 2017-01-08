using System.Threading.Tasks;
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

        [Theory(Skip="FakeItEasy does not handle async tests well it seems."), AutoFakeItEasyData]
        public async Task ReadAllEvents_ShouldLogReadingEvents(
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
            await sut.ReadAllEventsAsync(startPosition, builderName, streamName);

            // assert
            A.CallTo(() => logger.Write(LogEventLevel.Information, "ProjectionBuilder {name} reading all events from store {eventStore} from position {position}", builderName, service.ConnectionName, startPosition))
                .MustHaveHappened();
        }
    }
}