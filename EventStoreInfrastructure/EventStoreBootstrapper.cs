using EventStoreInfrastructure.Interfaces;

namespace EventStoreInfrastructure
{
    public class EventStoreBootstrapper
    {
        private IDomainRepository _eventRepository;
        private readonly IEventStoreSettings _eventStoreSettings;
        private IEventStoreConnectionFactory _eventStoreConnectionFactory;
        private IEventStoreProjectionService _eventProjectionService;

        public EventStoreBootstrapper(IEventStoreSettings eventStoreSettings)
        {
            _eventStoreSettings = eventStoreSettings;
        }

        public IDomainRepository EventRepository
        {
            get
            { 
                return _eventRepository ??
                       (_eventRepository = new EventStoreDomainRepository(EventStoreConnectionFactory, EventSerializer, _eventStoreSettings.Domain));
            }
            set { _eventRepository = value; }
        }

        public IEventStoreProjectionService EventProjectionService
            =>
                _eventProjectionService ??
                (_eventProjectionService =
                    new EventStoreProjectionService(ProjectionsManagerFactory.Create(),
                        _eventStoreSettings.EventStoreUser, _eventStoreSettings.EventStorePassword));

        private IEventStoreProjectionsManagerFactory ProjectionsManagerFactory
            =>
                new EventStoreProjectionsManagerFactory(_eventStoreSettings.EventStoreHostname,
                    _eventStoreSettings.EventStorePort, _eventStoreSettings.ProjectionTimeout);

        private IEventSerializer EventSerializer => new EventSerializer(_eventStoreSettings.Domain);


        private IEventStoreConnectionFactory EventStoreConnectionFactory
            =>
                _eventStoreConnectionFactory ??
                (_eventStoreConnectionFactory =
                    new EventStoreConnectionFactory(_eventStoreSettings.EventStoreUser,
                        _eventStoreSettings.EventStorePassword, _eventStoreSettings.EventStoreHostname,
                        _eventStoreSettings.EventStorePort));
    }
}
