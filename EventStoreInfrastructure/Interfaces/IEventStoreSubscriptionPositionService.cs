namespace EventStoreInfrastructure.Interfaces
{
    public interface IEventStoreSubscriptionPositionService
    {
        int LatestPositionFor(string projectionBuilderName);
        void UpdatePositionFor(string name, int position);
    }
}