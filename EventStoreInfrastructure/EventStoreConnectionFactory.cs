using System;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace EventStoreInfrastructure
{
    public interface IEventStoreConnectionFactory
    {
        IEventStoreConnection Create();
    }

    public class EventStoreConnectionFactory : IEventStoreConnectionFactory
    {
        private readonly UserCredentials _credentials;
        private readonly IPEndPoint _endPoint;

        public EventStoreConnectionFactory(string username, string password, string ip, int port)
        {
            _credentials = new UserCredentials(username, password);

            var ipAddress = Dns.GetHostAddresses(ip)[0];
            _endPoint = new IPEndPoint(ipAddress, port);
        }

        public IEventStoreConnection Create()
        {
            var now = DateTime.Now;
            ConnectionSettings settings =
                ConnectionSettings.Create()
                    // todo should set up some logging here. ideally a serilog-wrapper
                    //.UseFileLogger($"c:\\logs\\eventstorelogs-{now.Hour}-{now.Minute}-{now.Second}-{now.Millisecond}.log")
                    //.EnableVerboseLogging()
                    .KeepReconnecting()
                    .KeepRetrying()
                    .SetDefaultUserCredentials(_credentials);

            var connection = EventStoreConnection.Create(settings, _endPoint);
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}
