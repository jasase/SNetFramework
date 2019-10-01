using System.IO;

namespace Framework.Abstraction.Services.Docker
{
    public class DockerVolumeMapping
    {
        public DirectoryInfo Source { get; set; }
        public string ContainerDestination { get; set; }
    }
}
