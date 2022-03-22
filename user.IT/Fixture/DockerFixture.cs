using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace user.IT.Fixture
{
    public class DockerFixture : IDisposable
    {
        private readonly DockerClient dockerClient;

        private readonly List<DockerImageModel> images = new List<DockerImageModel>
        {
            new DockerImageModel
            {
                Name = "mongodb-test-it", SourceName = "mongo", Tag = "4.2", PortBindings = new Dictionary<string, string> { { "27017", "27017" } }
            }
        };

        private bool isInitialized = false;

        public DockerFixture()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                dockerClient = new DockerClientConfiguration().CreateClient();
                images.ForEach(StartContainerFromImage);
            }
        }

        private void StartContainerFromImage(DockerImageModel image)
        {
            var containers = dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true }).GetAwaiter().GetResult();
            if (!containers.Any(container => container.Names.Select(name => name.Replace("/", "")).Contains(image.Name)))
            {
                dockerClient.Images.CreateImageAsync(image.ToImageCreateParameter(), null, GetProgressHandler()).GetAwaiter().GetResult();
                dockerClient.Containers.CreateContainerAsync(image.ToCreateContainerParameters()).GetAwaiter().GetResult();
                dockerClient.Containers.StartContainerAsync(image.Name, null).GetAwaiter().GetResult();
            }
        }

        private static Progress<JSONMessage> GetProgressHandler()
        {
            return new Progress<JSONMessage>((m) => { Console.WriteLine(JsonSerializer.Serialize(m)); Debug.WriteLine(JsonSerializer.Serialize(m)); });
        }

        public void Dispose()
        {
            images.ForEach(image =>
            {
                dockerClient.Containers.StopContainerAsync(image.Name, new ContainerStopParameters { WaitBeforeKillSeconds = 5 }, CancellationToken.None).GetAwaiter().GetResult();
                dockerClient.Containers.RemoveContainerAsync(image.Name, new ContainerRemoveParameters { Force = true }, CancellationToken.None).GetAwaiter().GetResult();
                Thread.Sleep(5000);
            });
            isInitialized = false;

            GC.SuppressFinalize(this);
        }
    }
}
