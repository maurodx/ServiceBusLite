using System;
using Microsoft.Practices.ServiceLocation;
using ServiceBusLite.Handlers;

namespace ServiceBusLite
{
    public static class ServiceBusLiteConfigurator
    {
        public static ServiceBusLiteConfig Using(IContainerAdapter adapter)
        {
            return new ServiceBusLiteConfig(adapter);
        }
    }

    public class ServiceBusLiteConfig
    {
        private readonly IContainerAdapter _adapter;
        public ServiceBusLiteConfig(IContainerAdapter adapter)
        {
            _adapter = adapter;
            _adapter.Register(typeof (IContainerAdapter), adapter.GetType());
            _adapter.Register<IHandlerRegistrarService, HandlerRegistrarService>();
            _adapter.Register<IBus, Bus>();
        }

        public ServiceBusLiteConfig RegisterHandler<TMessageHandler>() where TMessageHandler : IMessageHandler
        {
            _adapter.RegisterSingleton<IMessageHandler, TMessageHandler>();
            return this;
        }
    }
}