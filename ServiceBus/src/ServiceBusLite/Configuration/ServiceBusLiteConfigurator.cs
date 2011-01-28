using Microsoft.Practices.ServiceLocation;

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
            _adapter.Initialize();
        }

        public ServiceBusLiteConfig  RegisterHandler<TService, TImplementation>()
        {
            _adapter.Register<TService, TImplementation>();
            return this;
        } 
    }
}