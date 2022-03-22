using MongoDB.Driver;

namespace user.console.Configuration
{
    public class MongoSetting
    {
        public const string ConfigurationKey = "mongo";

        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public MongoClientSettings ToMongoClientSettings()
        {
            MongoCredential credential = null;
            if (!string.IsNullOrEmpty(DatabaseName) &&
                !string.IsNullOrEmpty(User) &&
                !string.IsNullOrEmpty(Password))
            {
                credential = MongoCredential.CreateCredential(DatabaseName, User, Password);
            }

            return new MongoClientSettings
            {
                Server = new MongoServerAddress(Host, Port),
                Credential = credential,
            };
        }
    }
}
