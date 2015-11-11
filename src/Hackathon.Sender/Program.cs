namespace Hackathon.Sender
{
    using System;
    using Hackathon.Messages;
    using NServiceBus;

    class Program
    {
        static void Main(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Hackathon.Sender");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Enter a message to send, or an empty line to quit.");

                while (true)
                {
                    var message = Console.ReadLine();

                    if (String.IsNullOrEmpty(message))
                    {
                        Console.WriteLine("Shutting down...");
                        return;
                    }

                    bus.Send("Hackathon.Receiver", new HelloMessage
                    {
                        Message = message
                    });

                    Console.WriteLine("Message sent.");
                }
            }
        }
    }
}
