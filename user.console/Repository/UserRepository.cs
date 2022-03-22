using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.console.Configuration;
using user.console.Repository.Model;

namespace user.console.Repository
{
    public class UserRepository
    {
        protected readonly IMongoCollection<User> collection;

        public UserRepository(IOptions<MongoSetting> setting)
        {
            var client = new MongoClient(setting.Value.ToMongoClientSettings());
            var database = client.GetDatabase(setting.Value.DatabaseName);
            collection = database.GetCollection<User>("user");
        }

        public async Task InsertAsync(User entity)
        {
            await collection.InsertOneAsync(entity);
        }

        public async Task<User> GetAsync(Guid guid)
        {
            return (await collection.FindAsync(Builders<User>.Filter.Eq(x => x.Guid, guid))).FirstOrDefault();
        }

        public async Task DeleteAsync(Guid guid)
        {
            await collection.DeleteOneAsync(Builders<User>.Filter.Eq(x => x.Guid, guid));
        }
    }
}
