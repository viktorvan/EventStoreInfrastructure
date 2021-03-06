﻿using System;
using System.Net;
using EventStore.ClientAPI.SystemData;

namespace EventStoreInfrastructure
{
    public interface IEventStoreConfiguration
    {
        UserCredentials UserCredentials { get; }
        IPEndPoint EndPoint { get; }
    }

    public class EventStoreConfiguration : IEventStoreConfiguration
    {
        public UserCredentials UserCredentials { get; }
        public IPEndPoint EndPoint { get; }

        public EventStoreConfiguration(string user, string password, string ip, string port)
        {
            UserCredentials = new UserCredentials(user, password);
            var ipAddress = Dns.GetHostAddressesAsync(ip).Result[0];
            EndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(port));
        }
    }

}