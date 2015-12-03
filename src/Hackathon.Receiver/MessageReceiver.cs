namespace Hackathon.Receiver
{
    using System;
    using System.Threading.Tasks;
    using Hackathon.Messages;
    using NServiceBus;

    public class MessageReceiver : IHandleMessages<HelloMessage>
    {
   
        public Task Handle(HelloMessage message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received message:");
            Console.WriteLine(message.Message);
            return Task.FromResult(1);
        }
    }
}
