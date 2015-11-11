namespace Hackathon.Messages
{
    using NServiceBus;

    public class HelloMessage : ICommand
    {
        public string Message { get; set; }
    }
}
