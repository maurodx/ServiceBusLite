using System;
using System.Messaging;
using NUnit.Framework;

namespace ServiceBusLite.Tests
{
    [TestFixture]
    public class RequestReplyTest
    {
        private const string requestQueueName = ".\\private$\\RequestQueue";
        private const string replyQueueName = ".\\private$\\ReplyQueue";
        private const string invalidQueueName = ".\\private$\\InvalidQueue";
        [Test]
        public void RequestReplySpike()
        {
            IRequestor requestor = new Requestor(requestQueueName, replyQueueName);
            IReplier replier = new Replier(requestQueueName, invalidQueueName);

            requestor.Send(new Message
                               {
                                   Body = "Hello World"
                               });
            Message message = requestor.ReceiveSync();
            
            Assert.That(message.Body.Equals("Hello World"));
        }
    }

    public interface IRequestor
    {
        void Send(Message message);
        Message ReceiveSync();
    }

    public class Requestor : IRequestor
    {
        private MessageQueue _requestQueue;
        private MessageQueue _replyQueue;
        public Requestor(string requestQueueName, string replyQueueName)
        {
            _requestQueue = QueueFactory.Get(requestQueueName);
            _replyQueue = QueueFactory.Get(replyQueueName);

            _replyQueue.MessageReadPropertyFilter.SetAll();
            ((XmlMessageFormatter)_replyQueue.Formatter).TargetTypeNames = new string[]{"System.String,mscorlib"};
        }
        public void Send(Message message)
        {
            message.ResponseQueue = _replyQueue;
            _requestQueue.Send(message);
        }

        public Message ReceiveSync()
        {
            Message replyMessage = _requestQueue.Receive();
            return replyMessage;
        }
    }

    public interface IReplier : IDisposable
    {
        void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e);
    }

    public class Replier : IReplier
    {
        private MessageQueue _invalidQueue;
        private MessageQueue _requestQueue;

        public Replier(string requestQueueName, string invalidQueueName)
        {
            _requestQueue = QueueFactory.Get(requestQueueName);
            _invalidQueue = QueueFactory.Get(invalidQueueName);

            
            _requestQueue.MessageReadPropertyFilter.SetAll();
            ((XmlMessageFormatter)_requestQueue.Formatter).TargetTypeNames = new string[] { "System.String,mscorlib" };

            _requestQueue.ReceiveCompleted += OnReceiveCompleted;
            _requestQueue.BeginReceive();

        }

        public void OnReceiveCompleted(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue requestQueue = (MessageQueue) source;
            Message requestMessage = requestQueue.EndReceive(asyncResult.AsyncResult);

            try
            {
                MessageQueue replyQueue = requestMessage.ResponseQueue;
                Message replyMessage = new Message
                                           {
                                               Body = requestMessage.Body,
                                               CorrelationId = requestMessage.CorrelationId
                                           };
                replyQueue.Send(replyMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                requestMessage.CorrelationId = requestMessage.Id;
                _invalidQueue.Send(requestMessage);
            }

            requestQueue.BeginReceive();
        }

        public void Dispose()
        {
            if (_requestQueue != null)
                _requestQueue.ReceiveCompleted -= OnReceiveCompleted;
        }
    }


    public class QueueFactory
    {
        public static MessageQueue Get(string queueName)
        {
            if(MessageQueue.Exists(queueName))
                return new MessageQueue(queueName);
            return MessageQueue.Create(queueName);
        }
    }
}