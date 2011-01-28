using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;

namespace ServiceBusLite
{
    public class Bus : IBus
    {
        private  IContainerAdapter _containerAdapter;
        
        public void Initialize(IContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter;
        }


        public void Publish(IMessage message)
        {
            var handlers = _containerAdapter.GetHandlersFor(message);
            foreach (var handler in handlers)
            {
                if (handler.CanHandle(message.GetType()))
                    handler.Handle(message);
            }
        }
    }
}
