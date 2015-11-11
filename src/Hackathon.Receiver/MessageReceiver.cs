namespace Hackathon.Receiver
{
    using System;
    using Hackathon.Messages;
    using NServiceBus;

    public class MessageReceiver : IHandleMessages<HelloMessage>
    {
        public void Handle(HelloMessage message)
        {
            Console.WriteLine("Received message:");
            Console.WriteLine(message.Message);
        }
    }
}
