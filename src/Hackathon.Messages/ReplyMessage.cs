using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Messages
{
    public class ReplyMessage : IMessage
    {
        public string Reply { get; set; }

        public int MessageCount { get; set; }
    }
}
