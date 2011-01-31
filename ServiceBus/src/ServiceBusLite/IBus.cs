using System.Collections.Generic;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;

namespace ServiceBusLite
{
    public interface IBus
    {
        void Publish(IMessage message);
        IList<IMessageHandler> GetHandlersFor(IMessage message);
    }
}