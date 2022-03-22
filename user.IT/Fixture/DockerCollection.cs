using Xunit;

namespace user.IT.Fixture
{
    [CollectionDefinition(DockerCollection.Name)]
    public class DockerCollection : ICollectionFixture<DockerFixture>
    {
        public const string Name = "Docker-Collection";
    }
}
