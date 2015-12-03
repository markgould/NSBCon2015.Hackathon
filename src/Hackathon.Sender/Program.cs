namespace Hackathon.Sender
{
    using System;
    using System.Threading.Tasks;
    using Hackathon.Messages;
    using Hackathon.SlackTransport;
    using NServiceBus;

    class Program
    {
        static void Main(string[] args)
        {
            Start().GetAwaiter().GetResult();
        }

        static async Task Start()
        {
            
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("sender");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseTransport<SlackTransport>().ConnectionString("xoxb-15862501655-ASNxruOyfmshKpXCJsLsKyxI");
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            var endpoint = await Endpoint.Start(busConfiguration);

            var context = endpoint.CreateBusContext();

            Console.WriteLine("Enter a message to send, or an empty line to quit.");

            while (true)
            {
                var message = Console.ReadLine();

                if (String.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Shutting down...");
                    return;
                }

                await context.Send("Hackathon.Receiver", new HelloMessage
                {
                    Message = message
                });

                Console.WriteLine("Message sent.");
            }
        }
    }
}
