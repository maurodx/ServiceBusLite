using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using ServiceBusLite.Handlers;
using ServiceBusLite.Messages;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace ServiceBusLite.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        public void TestPublish()
        {
            IContainer container = new Container();
            ServiceBusLiteConfigurator.Using(new StructureMapContainerAdapter.StructureMapContainerAdapter(container))
                .RegisterHandler<IMessageHandler, MyHandler>();

            container.AssertConfigurationIsValid();
            var bus = container.GetInstance<IBus>();
            bus.Publish(new MyMessage{Body = "Hello World"});
        }
    }

    public class MyMessage : IMessage
    {
        public string Body { get; set; }
    }

    public class MyHandler : MessageHandler<MyMessage>
    {
        public override void Handle(MyMessage message)
        {
            Console.WriteLine(message.Body);
        }
    }
}
