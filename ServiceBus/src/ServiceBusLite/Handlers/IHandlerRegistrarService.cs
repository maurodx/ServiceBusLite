using System.Collections.Generic;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public interface IHandlerRegistrarService
    {
        IEnumerable<IMessageHandler> GetHandlersFor(IMessage messageType);
    }
}