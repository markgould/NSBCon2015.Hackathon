using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Transports;
using SlackConnector;
using NServiceBus;
using SlackConnector.Models;

namespace Hackathon.SlackTransport
{
    using System.Diagnostics;
    using System.IO;
    using NServiceBus.Extensibility;
    using SlackConnector = SlackConnector.SlackConnector;


    public class SlackMessagePusher : IPushMessages
    {
        private readonly EndpointName _endpointName;
        private Func<PushContext, Task> _pipe;
        private ISlackConnection _connection;
        private string _apiKey;

        public SlackMessagePusher(EndpointName endpointName, string apiKey)
        {
            _endpointName = endpointName;
            _apiKey = apiKey;
        }

        public void Init(Func<PushContext, Task> pipe, PushSettings settings)
        {
            _pipe = pipe;
            var connector = new SlackConnector();
            _connection = connector.Connect(_apiKey).GetAwaiter().GetResult();
        }

        public void Start(PushRuntimeSettings limitations)
        {
            Console.WriteLine("Starting Slack MessagePusher for endpoint {0}", _endpointName);
            _connection.OnMessageReceived += MessageReceived;
        }

        async Task MessageReceived(SlackMessage message)
        {

            Debug.WriteLine(message.RawData);

            await _pipe(new PushContext("test", new Dictionary<string, string>(), new MemoryStream(),
                new NoOpTransaction(), new ContextBag()));
        }

        public Task Stop()
        {
            Console.WriteLine("Stopping Slack MessagePusher for endpoint {0}", _endpointName);
            _connection.Disconnect();
            return Task.FromResult(0);
        }

        private class NoOpTransaction : TransportTransaction { }
    }
}
