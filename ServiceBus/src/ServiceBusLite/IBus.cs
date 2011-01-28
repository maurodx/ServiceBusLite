namespace ServiceBusLite
{
    public interface IBus
    {
        void Publish<TMessage>(TMessage message);
    }
}