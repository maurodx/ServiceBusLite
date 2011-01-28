using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ServiceBusLite.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void PublishMessage()
        {
            IBus bus = new Bus();
            bus.Publish(new TestMessage{Message = "TestMessage"});
        }
    }

    public class TestMessage
    {
        public string Message { get; set; }
    }
}
