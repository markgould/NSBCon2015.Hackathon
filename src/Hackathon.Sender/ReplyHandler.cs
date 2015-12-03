using Hackathon.Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Sender
{
    public class ReplyHandler : IHandleMessages<ReplyMessage>
    {
        public Task Handle(ReplyMessage message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received reply:");
            Console.WriteLine(message.Reply);
            return Task.FromResult(0);
        }
    }
}
