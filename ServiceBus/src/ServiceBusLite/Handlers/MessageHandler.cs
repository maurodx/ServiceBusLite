using System;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
    {
        public abstract void Handle(TMessage message);

        public virtual void Handle(object message)
        {
            Handle((TMessage)message);
        }

        public virtual bool CanHandle(Type type)
        {
            return type == typeof (TMessage);
        }
    }
}