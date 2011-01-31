using System;
using System.Linq;
using NUnit.Framework;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;
using StructureMap;

namespace ServiceBusLite.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        public void TestRegistration()
        {
            using (IContainer container = new Container())
            {
                ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                    .RegisterHandler<MyHandler>();
                container.AssertConfigurationIsValid();
            }
        }

        [Test]
        public void TestPublish()
        {
            using (IContainer container = new Container())
            {
                ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                    .RegisterHandler<MyHandler>();

                var bus = container.GetInstance<IBus>();
                var myMessage = new MyMessage {Body = "Hello World"};
                bus.Publish(myMessage);

                IMessageHandler handler = bus.GetHandlersFor(myMessage).First();
                var myHandler = handler as MyHandler;
                Assert.NotNull(myHandler);
                Assert.That(myHandler.Message.Body == "Hello World");
            }
        }

        [Test]
        public void TestPublishTwoSourcesOfSameMessageType()
        {
            using (IContainer container = new Container())
            {
                ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                    .RegisterHandler<MyHandler>()
                    .RegisterHandler<MyHandler2>();

                var bus = container.GetInstance<IBus>();
                var message = new MyMessage {Body = "Hello World"};
                bus.Publish(message);

                var handlers = bus.GetHandlersFor(message);
                Assert.That(handlers.Count == 2);
            }
        }


        [Test]
        public void TestPublishTwoSources()
        {
            using (IContainer container = new Container())
            {
                ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                    .RegisterHandler<MyHandler>()
                    .RegisterHandler<MyHandler2>()
                    .RegisterHandler<MySecondMessageHandler>();

                var bus = container.GetInstance<IBus>();
                var message = new MyMessage {Body = "Hello World"};
                bus.Publish(message);

                var secondMessage = new MySecondMessage {Body = "Yo World"};
                bus.Publish(secondMessage);

                var handlers = bus.GetHandlersFor(message);
                Assert.That(handlers.Count == 2);

                var secondMessageHandlers = bus.GetHandlersFor(secondMessage);
                Assert.That(secondMessageHandlers.Count == 1);
            }
        }

        [Test]
        public void TestPublishWithService()
        {
            using (IContainer container = new Container())
            {
                ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                    .RegisterHandler<MyHandler>();
                container.Configure(c => c.For<IMyService>().Use<MyService>());

                var bus = container.GetInstance<IBus>();
                var myservice = container.GetInstance<IMyService>();
                var message = new MyMessage { Body = "Hello World" };
                bus.Publish(message);

                Assert.IsNotNull(myservice.MyMessage);
                Assert.That(message.Body == myservice.MyMessage.Body);

                myservice.Dispose();
            }
        }

    }

    public interface IMyService
    {
        MyMessage MyMessage { get; set; }
        void Dispose();
    }

    public class MyService : IDisposable, IMyService
    {
        private IMessageHandler _myHandler;
        public MyMessage MyMessage { get; set; }

        public MyService(IMessageHandler myHandler)
        {
            _myHandler = myHandler;
            _myHandler.OnCompleted += OnCompleted;
        }

        void OnCompleted(object sender, MessageHandledEventArg<IMessage> messageHandledEventArg)
        {
            if (messageHandledEventArg.Message != null)
                MyMessage = (MyMessage)messageHandledEventArg.Message;
        }

        public void Dispose()
        {
            if(_myHandler != null)
                _myHandler.OnCompleted -= OnCompleted;
        }
    }

    public class MyMessage : IMessage
    {
        public string Body { get; set; }
    }

    public class MyHandler : MessageHandler<MyMessage>
    {
        public MyMessage Message { get; private set; }
        

        public override void Handle<TMessage>(TMessage message)
        {
            Message = message as MyMessage;
            OnCompleted(this, new MessageHandledEventArg<IMessage>(message));
        }

        public override event MessageHandler<IMessage>.CompletedEventHandler OnCompleted;
    }

    public class MyHandler2 : MessageHandler<MyMessage>
    {
        public MyMessage Message { get; private set; }
        

        public override void Handle<TMessage>(TMessage message)
        {
            Message = message as MyMessage;
        }

        public override event MessageHandler<IMessage>.CompletedEventHandler OnCompleted;
    }

    public class MySecondMessage : IMessage
    {
        public string Body { get; set; }
    }

    public class MySecondMessageHandler : MessageHandler<MySecondMessage>
    {
        public MySecondMessage Message { get; private set; }
        
        public override void Handle<TMessage>(TMessage message)
        {
            Message = message as MySecondMessage;
        }

        public override event MessageHandler<IMessage>.CompletedEventHandler OnCompleted;
    }
}
