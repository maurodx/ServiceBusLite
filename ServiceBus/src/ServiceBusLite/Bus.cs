using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceBusLite
{
    public class Bus : IBus
    {
        public void Publish<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
