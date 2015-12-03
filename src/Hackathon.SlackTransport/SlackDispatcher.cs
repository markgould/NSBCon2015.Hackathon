using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Transports;
using NServiceBus;
using SlackConnector;
using Newtonsoft.Json;
using NServiceBus.Routing;
using SlackConnector.Models;

namespace Hackathon.SlackTransport
{
    public class SlackDispatcher : IDispatchMessages
    {
        private readonly EndpointName _endpointName;
        private readonly string _apiKey;
        private ISlackConnection _slackConnection;

        public SlackDispatcher(EndpointName endpointName, string apiKey)
        {
            _endpointName = endpointName;
            _apiKey = apiKey;
        }

        public async Task Dispatch(IEnumerable<TransportOperation> outgoingMessages, ContextBag context)
        {
            await CheckConnection();

            foreach (var op in outgoingMessages)
            {
                var tag = op.DispatchOptions.AddressTag as UnicastAddressTag;
                var hub = _slackConnection.ConnectedChannels().FirstOrDefault(x => x.Name.Equals(string.Concat("#", tag.Destination), StringComparison.InvariantCultureIgnoreCase));

                if (hub == null)
                {
                    continue;
                }


                var attachments = new List<SlackAttachment>();
                attachments.Add(new SlackAttachment()
                {
                    Title = "MessageId",
                    Text = op.Message.MessageId
                });
                attachments.Add(new SlackAttachment()
                {
                    Title = "Headers",
                    Text = JsonConvert.SerializeObject(op.Message.Headers)
                });
                attachments.Add(new SlackAttachment()
                {
                    Title = "Body",
                    Text = JsonConvert.SerializeObject(op.Message.Body)
                });

                var slackMessage = new BotMessage()
                {
                    ChatHub = hub,
                    Attachments = attachments
                };

                await _slackConnection.Say(slackMessage);
            }
        }

        private async Task CheckConnection()
        {
            if (_slackConnection != null)
                return;

            var connector = new SlackConnector.SlackConnector();
            _slackConnection = await connector.Connect(_apiKey);
        }
    }
}