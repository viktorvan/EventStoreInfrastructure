using System.Threading.Tasks;

namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreProjectionService
    {
        Task<bool> CreateOrUpdateProjectionAsync(string name, string query);
        Task<string> ReadProjectionResultAsync(string name);
    }
}