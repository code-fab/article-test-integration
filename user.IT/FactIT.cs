using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using Xunit;

namespace user.IT
{
    public class FactIT : FactAttribute
    {
        public FactIT()
        {
            if (!HasDocker())
            {
                Skip = "Docker instance not found";
            }
        }

        private static bool HasDocker()
        {
            try
            {
                DockerClient dockerClient = new DockerClientConfiguration().CreateClient();
                var containers = dockerClient.Containers.ListContainersAsync(new ContainersListParameters { Limit = 10 }).Result;
                Console.WriteLine("Local environment has a docker instance");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
