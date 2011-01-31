using System;
using System.Collections.Generic;
using System.Linq;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;

namespace ServiceBusLite.StructureMapContainerAdapter
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        private readonly IContainer _container;
        
        public StructureMapContainerAdapter(IContainer container)
        {
            _container = container;            
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

        public TImplementation GetInstance<TImplementation>()
        {
            return _container.GetInstance<TImplementation>();
        }

        public IList<TImplementation> GetAllInstances<TImplementation>()
        {
            return _container.GetAllInstances<TImplementation>();
        }
    }
}
