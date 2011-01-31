using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;

namespace ServiceBusLite
{
    public interface IContainerAdapter
    {
        void RegisterSingleton(Type service, Type implementation);
        void RegisterSingleton<TService, TImplementation>();
        void Register(Type service, Type implementation);
        void Register<TService, TImplementation>();

        TImplementation GetInstance<TImplementation>();
        IList<TImplementation> GetAllInstances<TImplementation>();
    }
}