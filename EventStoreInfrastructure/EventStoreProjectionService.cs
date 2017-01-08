using System.Threading.Tasks;
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

        public async Task AddOrUpdateProjectionAsync(string name, string query)
        {
            await _projectionsManager.CreateContinuousAsync(name, query, _credentials);
        }

        public async Task<string> ReadProjectionResultAsync(string name)
        {
            return await _projectionsManager.GetResultAsync(name, _credentials);
        }
    }
}
