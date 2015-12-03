using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Settings;
using NServiceBus.Support;
using NServiceBus.Transports;
using System.Threading.Tasks;

namespace Hackathon.SlackTransport
{
    
    public class SlackTransport : TransportDefinition
    {
        protected override void ConfigureForReceiving(TransportReceivingConfigurationContext context)
        {
            context.SetMessagePumpFactory(p => new SlackMessagePusher(context.Settings.EndpointName(), context.ConnectionString));
            context.SetQueueCreatorFactory(() => new QueueCreator());
        }

        protected override void ConfigureForSending(TransportSendingConfigurationContext context)
        {
            context.SetDispatcherFactory(() => new SlackDispatcher(context.GlobalSettings.EndpointName(), context.ConnectionString));
        }

        public override IEnumerable<Type> GetSupportedDeliveryConstraints()
        {
            return new List<Type>();
        }

        public override TransactionSupport GetTransactionSupport()
        {
            return TransactionSupport.None;
        }

        public override IManageSubscriptions GetSubscriptionManager()
        {
            return new SlackSubscriptionManager();
        }

        public override string GetDiscriminatorForThisEndpointInstance()
        {
            return RuntimeEnvironment.MachineName;
        }
       

        public override string ToTransportAddress(LogicalAddress logicalAddress)
        {
            return logicalAddress.ToString();
        }

        public override OutboundRoutingPolicy GetOutboundRoutingPolicy(ReadOnlySettings settings)
        {
            return new OutboundRoutingPolicy(OutboundRoutingType.DirectSend, OutboundRoutingType.DirectSend, OutboundRoutingType.DirectSend);
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}
