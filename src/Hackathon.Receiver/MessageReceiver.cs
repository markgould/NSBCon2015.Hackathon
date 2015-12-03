namespace Hackathon.Receiver
{
    using System;
    using System.Threading.Tasks;
    using Hackathon.Messages;
    using NServiceBus;
    using System.Threading;
    public class MessageReceiver : IHandleMessages<HelloMessage>
    {
        private static int _messageCount;
        public Task Handle(HelloMessage message, IMessageHandlerContext context)
        {
            var count = Interlocked.Increment(ref _messageCount);
            Console.WriteLine("Received message:");
            Console.WriteLine(message.Message);

            return context.Reply(new ReplyMessage
            {
                MessageCount = count,
                Reply = string.Format("Got your message! Counter: {0}", count)
            });
        }
    }
}
