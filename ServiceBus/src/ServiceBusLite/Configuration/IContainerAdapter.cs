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

        void Initialize();

        TImplementation GetInstance<TImplementation>(Type service);
        TImplementation GetInstance<TImplementation, TService>();
        IEnumerable<object> GetAllInstances<TImplmentation>();

        IEnumerable<IMessageHandler> GetHandlersFor(IMessage messageType);
    }
}