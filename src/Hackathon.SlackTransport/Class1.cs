using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SlackTransport
{
    using SlackConnector;
    using SlackConnector.Models;

    public class Test
    {
        public Test()
        {
            TestSlack().GetAwaiter().GetResult();
        }

        public async Task TestSlack()
        {
            var connector = new SlackConnector();
            var connection = await connector.Connect("xoxp-15847839794-15848140720-15847023921-2bed7a8cc3");
            connection.OnMessageReceived += MessageReceived;
            connection.OnDisconnect += Disconnected;

            var msg =new BotMessage()
            {
               Text = "testing",
               
               ChatHub = connection.ConnectedChannels().First(x => x.Name == "#sender")
            };

            await connection.Say(msg);



        }

        void Disconnected()
        {
            
        }

        Task MessageReceived(SlackMessage message)
        {
            return Task.FromResult(0);
        }
    }
}
