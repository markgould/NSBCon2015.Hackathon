using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using SlackConnector.Models;
using Newtonsoft.Json;
using NServiceBus.Transports;
using NServiceBus.Extensibility;
using NServiceBus;
using SlackConnector;

namespace Hackathon.SlackTransport
{


    public class RawAttachment
    {
        public string text { get; set; }
        public string title { get; set; }
        public int id { get; set; }
        public string fallback { get; set; }
    }

    public class RawMessage
    {
        public string type { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public List<RawAttachment> attachments { get; set; }
        public string channel { get; set; }
        public string ts { get; set; }
    }

    public class SlackMessagePusher : IPushMessages
    {
        private readonly EndpointName _endpointName;
        private Func<PushContext, Task> _pipe;
        private ISlackConnection _connection;
        private string _apiKey;
        private PushSettings _settings;

        public SlackMessagePusher(EndpointName endpointName, string apiKey)
        {
            _endpointName = endpointName;
            _apiKey = apiKey;
        }

        public void Init(Func<PushContext, Task> pipe, PushSettings settings)
        {
            _pipe = pipe;
            _settings = settings;
            var connector = new SlackConnector.SlackConnector();
            _connection = connector.Connect(_apiKey).GetAwaiter().GetResult();
        }

        public void Start(PushRuntimeSettings limitations)
        {
            //Console.WriteLine("Starting Slack MessagePusher for endpoint {0} input queue {1}", _endpointName, _settings.InputQueue);
            _connection.OnMessageReceived += MessageReceived;
        }

        async Task MessageReceived(SlackMessage message)
        {
            //Quick hack to ignore timeouts queue
            if (_settings.InputQueue.Contains("."))
                return;

            var rawMsg = JsonConvert.DeserializeObject<RawMessage>(message.RawData);

            if (message.ChatHub.Name != string.Concat("#", _endpointName))
                return;

            if (rawMsg.attachments.Count == 0)
                return;

            var idAttachment = rawMsg.attachments.First(a => a.title == "MessageId");
            var headersAttachment = rawMsg.attachments.First(a => a.title == "Headers");
            var bodyAttachment = rawMsg.attachments.First(a => a.title == "Body");

            if (idAttachment == null || headersAttachment == null || bodyAttachment == null)
                return;

            var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(headersAttachment.text);
            var body =  new MemoryStream(JsonConvert.DeserializeObject<byte[]>(bodyAttachment.text));

            await _pipe(new PushContext(idAttachment.text, headers, body,
                new NoOpTransaction(), new ContextBag()));
        }

        public Task Stop()
        {
            _connection.Disconnect();
            return Task.FromResult(0);
        }

        private class NoOpTransaction : TransportTransaction { }
    }
}
