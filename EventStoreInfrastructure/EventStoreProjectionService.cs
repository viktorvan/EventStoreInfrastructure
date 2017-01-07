using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using EventStoreInfrastructure.Interfaces;

namespace EventStoreInfrastructure
{
    public class EventStoreProjectionService : IEventStoreProjectionService
    {
        private readonly ProjectionsManager _projectionsManager;
        private readonly UserCredentials _credentials;

        public EventStoreProjectionService(ProjectionsManager projectionsManager, string username, string password)
        {
            _credentials = new UserCredentials(username, password);
            _projectionsManager = projectionsManager;
        }

        public void AddOrUpdateProjection(string name, string query)
        {
            _projectionsManager.CreateContinuousAsync(name, query, _credentials).Wait();
        }

        public string ReadProjectionResult(string name)
        {
            return _projectionsManager.GetResultAsync(name, _credentials).Result;
        }
    }
}
