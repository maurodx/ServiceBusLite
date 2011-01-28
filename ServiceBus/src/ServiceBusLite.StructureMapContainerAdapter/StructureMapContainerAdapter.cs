using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;

namespace ServiceBusLite.StructureMapContainerAdapter
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        private IContainer _container;
        private StructureMapServiceLocator _serviceLocator;
        
        public StructureMapContainerAdapter(IContainer container)
        {
            _container = container;            
        }

        public void Initialize()
        {
            _serviceLocator = new StructureMapServiceLocator(_container);
            _container.Configure(c => c.For<IBus>().Singleton().Use<Bus>().OnCreation(i => i.Initialize(this)));
        }

        public void RegisterSingleton(Type service, Type implementation)
        {
            _container.Configure(c => c.For(service).Singleton().Use(implementation));
        }

        public void RegisterSingleton<TService, TImplementation>()
        {
            RegisterSingleton(typeof(TService), typeof(TImplementation));
        }

        public void Register(Type service, Type implementation)
        {
            _container.Configure(c => c.For(service).Use(implementation));
        }

        public void Register<TService, TImplementation>()
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        public TImplementation GetInstance<TImplementation>(Type service)
        {
            return (TImplementation)_serviceLocator.GetInstance(service);
        }

        public TImplementation GetInstance<TImplementation, TService>()
        {
            return (TImplementation)_serviceLocator.GetInstance(typeof(TService));
        }

        public IEnumerable<object> GetAllInstances<TImplementation>()
        {
            var results = _serviceLocator.GetAllInstances(typeof(TImplementation));
            return results;
        }

        public IEnumerable<IMessageHandler> GetHandlersFor(IMessage messageType)
        {
            var results = GetAllInstances<IMessageHandler>().ToList();
            foreach(var result in results)
            {
                var handler = result as IMessageHandler;
                if (handler != null)
                    if (handler.CanHandle(messageType.GetType()))
                        yield return handler;
            }
        }
    }
}
