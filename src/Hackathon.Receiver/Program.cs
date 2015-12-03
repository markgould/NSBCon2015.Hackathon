namespace Hackathon.Receiver
{
    using System;
    using System.Threading.Tasks;
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
            busConfiguration.EndpointName("Hackathon.Receiver");
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
