namespace Hackathon.Receiver
{
    using System;
    using System.Threading.Tasks;
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
            busConfiguration.EndpointName("receiver");
            busConfiguration.UseTransport<SlackTransport>().ConnectionString("xoxb-15857024355-n47TqzrtxXrTyl0zI0RrzO3a");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            var endpoint = await Endpoint.Start(busConfiguration);

            var context = endpoint.CreateBusContext();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
