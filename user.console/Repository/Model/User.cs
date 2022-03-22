using MongoDB.Bson.Serialization.Attributes;
using System;

namespace user.console.Repository.Model
{
    public class User
    {
        [BsonId]
        public Guid Guid { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }
    }
}
