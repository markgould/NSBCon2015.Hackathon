using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Transports;

namespace Hackathon.SlackTransport
{
    
    class SlackSubscriptionManager : IManageSubscriptions
    {
        public Task Subscribe(Type eventType, ContextBag context)
        {
            throw new NotImplementedException();
        }

        public Task Unsubscribe(Type eventType, ContextBag context)
        {
            throw new NotImplementedException();
        }
    }
}
