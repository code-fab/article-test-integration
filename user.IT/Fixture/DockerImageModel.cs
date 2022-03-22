using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace user.IT.Fixture
{
    public class DockerImageModel
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public string SourceName { get; set; }

        /// <summary>
        /// Port bindings for container, respect docker synthax <local,container>
        /// </summary>
        public Dictionary<string, string> PortBindings { get; set; }

        public ImagesCreateParameters ToImageCreateParameter()
        {
            return new ImagesCreateParameters { FromImage = this.SourceName, Tag = this.Tag };
        }

        public CreateContainerParameters ToCreateContainerParameters()
        {
            var openPorts = new Dictionary<string, EmptyStruct>();
            var hostConfig = new HostConfig();

            if (PortBindings != null && PortBindings.Count > 0)
            {
                var hostMapping = new Dictionary<string, IList<PortBinding>>();

                PortBindings.Keys.ToList().ForEach(keyPort =>
                {
                    openPorts.Add(PortBindings[keyPort], new EmptyStruct());
                    hostMapping.Add(PortBindings[keyPort], new List<PortBinding> { new PortBinding { HostIP = "127.0.0.1", HostPort = keyPort } });
                });

                hostConfig = new HostConfig { PortBindings = hostMapping };
            }

            return new CreateContainerParameters
            {
                Image = $"{this.SourceName}:{this.Tag}",
                Name = this.Name,
                HostConfig = hostConfig,
                ExposedPorts = openPorts
            };
        }
    }
}
