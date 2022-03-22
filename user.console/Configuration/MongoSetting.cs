using MongoDB.Driver;

namespace user.console.Configuration
{
    public class MongoSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
    }
}
