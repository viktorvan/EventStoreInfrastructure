using System;
using EventStoreInfrastructure.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace EventStoreInfrastructure.Tests
{
    public class EventDispatcherTests
    {
        private readonly EventDispatcher _sut;

        public EventDispatcherTests()
        {
            _sut = new EventDispatcher();
        }

        public void Dispatch_ShouldLogTryingToHandleEvent()
        {
            // arrange
            var logger = A.Fake<ILogger>();
            Log.Logger = logger;
            var @event = new TestEvent(Guid.NewGuid());
            
            // act
            _sut.Dispatch(@event);

            // assert
            A.CallTo(() => logger.Write(LogEventLevel.Information, "Trying to dispatch event {type}", typeof(TestEvent))).MustHaveHappened();
        }

        public void Dispatch_WhenNoMappingFound_ShouldLogMessage()
        {
            // arrange
            var logger = A.Fake<ILogger>();
            Log.Logger = logger;
            var @event = new TestEvent(Guid.NewGuid());
            // act
            _sut.Dispatch(@event);

            // assert
            A.CallTo(
                () => logger.Write(LogEventLevel.Information, "No mapping for event {type} found", typeof (TestEvent)))
                .MustHaveHappened();
        } 


        public void Dispatch_WhenMappingExists_ShouldHandle()
        {
            // arrange
            var @event = new TestEvent(Guid.NewGuid());
            var isHandlerCalled = false;
            _sut.AddMapping(typeof(TestEvent), x => isHandlerCalled = true);
            
            // act
            _sut.Dispatch(@event);

            // assert
            isHandlerCalled.Should().BeTrue();
        }

        public void Dispatch_WhenMappingExists_ShouldLogHandlingEvent()
        {
            // arrange
            var logger = A.Fake<ILogger>();
            Log.Logger = logger;
            _sut.AddMapping(typeof(TestEvent), x => { });
            var @event = new TestEvent(Guid.NewGuid());

            // act
            _sut.Dispatch(@event);

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
