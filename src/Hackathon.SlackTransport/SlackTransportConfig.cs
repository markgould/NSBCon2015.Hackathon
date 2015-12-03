namespace Hackathon.Sender
{
    using System.Configuration;

    public class SlackTransportConfig : ConfigurationSection
    {
        [ConfigurationProperty("APIKey", IsRequired = true)]
        public string APIKey
        {
            get { return this["APIKey"] as string; }
            set { this["APIKey"] = value; }
        }
    }
}
