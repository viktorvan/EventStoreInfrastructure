using System;
using EventStoreInfrastructure.Interfaces;
using EventStoreInfrastructure.Tests.TestHelpers;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Serilog;
using Serilog.Events;
using Xunit;

namespace EventStoreInfrastructure.Tests
{
    public class EventDispatcherTests
    {
        [Theory, AutoFakeItEasyData]
        public void Dispatch_ShouldLogTryingToHandleEvent(
            [Frozen] ILogger logger,
            TestEvent @event,
            EventDispatcher dispatcher)
        {
            // arrange
            Log.Logger = logger;
            
            // act
            dispatcher.Dispatch(@event);

            // assert
            A.CallTo(() => logger.Write(LogEventLevel.Information, "Trying to dispatch event {type}", typeof(TestEvent))).MustHaveHappened();
        }

        [Theory, AutoFakeItEasyData]
        public void Dispatch_WhenNoMappingFound_ShouldLogMessage(
            [Frozen] ILogger logger,
            TestEvent @event,
            EventDispatcher dispatcher)
        {
            // arrange
            Log.Logger = logger;

            // act
            dispatcher.Dispatch(@event);

            // assert
            A.CallTo(
                () => logger.Write(LogEventLevel.Information, "No mapping for event {type} found", typeof (TestEvent)))
                .MustHaveHappened();
        } 


        [Theory, AutoFakeItEasyData]
        public void Dispatch_WhenMappingExists_ShouldHandle(
            TestEvent @event,
            EventDispatcher dispatcher)
        {
            // arrange
            var isHandlerCalled = false;
            dispatcher.AddMapping(typeof(TestEvent), x => isHandlerCalled = true);

            // act
            dispatcher.Dispatch(@event);

            // assert
            isHandlerCalled.Should().BeTrue();
        }

        [Theory, AutoFakeItEasyData]
        public void Dispatch_WhenMappingExists_ShouldLogHandlingEvent(
            [Frozen] ILogger logger,
            TestEvent @event,
            EventDispatcher dispatcher)
        {
            // arrange
            Log.Logger = logger;
            dispatcher.AddMapping(typeof(TestEvent), x => { });

            // act
            dispatcher.Dispatch(@event);

            // assert
            A.CallTo(() => logger.Write(LogEventLevel.Information, "Handling event {@event}", @event as IEvent))
                .MustHaveHappened();
        } 
    }

    public class TestEvent : IEvent {
        public TestEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
