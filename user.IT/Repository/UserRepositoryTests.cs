using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using user.console.Configuration;
using user.console.Repository;
using user.console.Repository.Model;
using user.IT.Fixture;
using Xunit;

namespace user.IT.Repository
{
    [Collection(DockerCollection.Name)]
    public class UserRepositoryTests
    {
        private readonly UserRepository userRepository;

        public UserRepositoryTests()
        {
            userRepository = new UserRepository(Options.Create(new MongoSetting { Host = "localhost", Port = 27017, DatabaseName = "user" }));
        }

        [FactIT]
        public async Task Should_InsertAsync()
        {
            var userGuid = Guid.NewGuid();
            await userRepository.InsertAsync(new User { Guid = userGuid, FirstName = "foo", LastName = "bar" });

            var user = await userRepository.GetAsync(userGuid);
            Assert.NotNull(user);
            Assert.Equal("foo", user.FirstName);
            Assert.Equal("bar", user.LastName);
        }
    }
}
