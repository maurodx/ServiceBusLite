using System;
using ServiceBusLite.Messages;

namespace ServiceBusLite.Handlers
{
    public interface IMessageHandler
    {
        //void Handle(object message);
        void Handle<TMessage>(TMessage message) where TMessage : IMessage;
        bool CanHandle(Type type);
        event MessageHandler<IMessage>.CompletedEventHandler OnCompleted;
    }

    //public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IMessage
    //{
    //    void Handle(TMessage message);
    //}


}