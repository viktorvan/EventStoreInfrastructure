namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreProjectionService
    {
        void AddOrUpdateProjection(string name, string query);
        string ReadProjectionResult(string name);
    }
}