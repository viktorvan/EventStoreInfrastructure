using System;
using System.Net;
using EventStore.ClientAPI.Projections;

namespace Workout.Infrastructure.EventStoreInfrastructure
{
    public interface IEventStoreProjectionsManagerFactory
    {
        ProjectionsManager Create();
    }

    public class EventStoreProjectionsManagerFactory : IEventStoreProjectionsManagerFactory
    {
        private readonly IPEndPoint _endPoint;
        private readonly TimeSpan _timeout;

        public EventStoreProjectionsManagerFactory(string ip, int port, TimeSpan timeout)
        {
            _timeout = timeout;
            var ipAddress = Dns.GetHostAddresses(ip)[0];
            _endPoint = new IPEndPoint(ipAddress, port);
        }

        public ProjectionsManager Create()
        {
            return new ProjectionsManager(null, _endPoint, _timeout);
        }
    }
}
