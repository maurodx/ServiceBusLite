using System;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public interface IMessageHandler
    {
        void Handle(object message);
        bool CanHandle(Type type);
    }

    public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IMessage
    {
        void Handle(TMessage message);
    }


}