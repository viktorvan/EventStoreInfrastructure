using System;
using System.Net;
using EventStore.ClientAPI.Projections;
using Serilog;
using ILogger = EventStore.ClientAPI.ILogger;

namespace EventStoreInfrastructure
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
            var ipAddress = Dns.GetHostAddressesAsync(ip).Result[0];
            _endPoint = new IPEndPoint(ipAddress, port);
        }

        public ProjectionsManager Create()
        {            
            return new ProjectionsManager(new Logger(), _endPoint, _timeout);
        }
    }

    public class Logger : ILogger
    {
        public void Error(string format, params object[] args)
        {
            Log.Error(format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            Log.Error(ex, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Log.Information(format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            Log.Information(ex, format, args);
        }

        public void Debug(string format, params object[] args)
        {
            Log.Debug(format, args);
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            Log.Debug(ex, format, args);
        }
    }
}
