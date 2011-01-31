using System;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public abstract class MessageHandler<TMessage> : IMessageHandler
    {
        public delegate void CompletedEventHandler(object sender, MessageHandledEventArg<IMessage> e);
        public abstract event MessageHandler<IMessage>.CompletedEventHandler OnCompleted;

        public abstract void Handle<TMessage>(TMessage message) where TMessage : IMessage;

        public virtual bool CanHandle(Type type)
        {
            return type == typeof (TMessage);
        }
    }

    public class MessageHandledEventArg<TMessage> : EventArgs
    {
        public TMessage Message { get; private set; }
        public MessageHandledEventArg(TMessage message)
        {
            Message = message;
        }
    }
}