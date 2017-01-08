using System.Threading.Tasks;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreProjectionService
    {
        Task CreateOrUpdateProjectionAsync(string name, string query);
        Task<string> ReadProjectionResultAsync(string name);
    }
}