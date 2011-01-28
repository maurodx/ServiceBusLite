using ServiceBusLite.Messages;

namespace ServiceBusLite
{
    public interface IBus
    {
        void Publish(IMessage message);
        //void Publish<TMessage>(TMessage message) where TMessage : IMessage;
    }
}