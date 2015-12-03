using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Transports;

namespace Hackathon.SlackTransport
{
    public class QueueCreator : ICreateQueues
    {
        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            return Task.FromResult(0);
        }
    }
}
