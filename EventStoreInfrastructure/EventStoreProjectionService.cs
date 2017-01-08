using System.Threading.Tasks;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using EventStoreInfrastructure.Interfaces;
using System.Linq;

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

        public async Task<bool> CreateOrUpdateProjectionAsync(string name, string query)
        {
            if (await Exists(name))
            {
                var oldQuery = await _projectionsManager.GetQueryAsync(name, _credentials);
                if (oldQuery == query) return false;
            }

            await _projectionsManager.DeleteAsync(name, _credentials);
            await _projectionsManager.CreateContinuousAsync(name, query, _credentials);

            return true;
        }

        public async Task<string> ReadProjectionResultAsync(string name)
        {
            if (!await Exists(name))
            {
                return null;
            }

            return await _projectionsManager.GetResultAsync(name, _credentials);
        }

        private async Task<bool> Exists(string name)
        {
            var all = await _projectionsManager.ListAllAsync(_credentials);
            return all.Any(x => x.Name == name);
        }
    }
}
