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
        private readonly IHandlerRegistrarService _registrarService;

        public Bus(IHandlerRegistrarService registrarService)
        {
             _registrarService = registrarService;
        }

        public void Publish(IMessage message)
        {
            var handlers = GetHandlersFor(message);
            
            foreach (var handler in handlers)
            {
                handler.Handle(message);
            }
        }

        public IList<IMessageHandler> GetHandlersFor(IMessage message)
        {
            var handlers = _registrarService.GetHandlersFor(message);
            return handlers.Where(handler => handler.CanHandle(message.GetType())).ToList();
        }
    }
}
