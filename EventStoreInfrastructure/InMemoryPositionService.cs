using System.Collections.Generic;
using EventStore.ClientAPI;
using EventStoreInfrastructure.Interfaces;

namespace Workout.Infrastructure.EventStoreInfrastructure
{
    public class InMemoryPositionService : IEventStoreSubscriptionPositionService
    {
        private readonly Dictionary<string,int> _currentPosition = new Dictionary<string, int>();


        public int LatestPositionFor(string projectionBuilderName)
        {
            if (_currentPosition.ContainsKey(projectionBuilderName))
            {
                return _currentPosition[projectionBuilderName];
            }

            return StreamPosition.Start;
        }

        public void UpdatePositionFor(string name, int position)
        {
            _currentPosition[name] = position;
        }
    }
}