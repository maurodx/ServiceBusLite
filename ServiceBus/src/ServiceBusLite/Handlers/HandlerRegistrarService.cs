using System;
using System.Collections.Generic;
using System.Linq;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public class HandlerRegistrarService : IHandlerRegistrarService
    {
        private IContainerAdapter _containerAdapter;

        public HandlerRegistrarService(IContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter;
        }

        public IEnumerable<IMessageHandler> GetHandlersFor(IMessage messageType)
        {
            var results = _containerAdapter.GetAllInstances<IMessageHandler>().ToList();
            foreach (var handler in results)
            {
                if (handler != null)
                    if (handler.CanHandle(messageType.GetType()))
                        yield return handler;
            }
        }
    }
}