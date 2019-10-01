using System.IO;

namespace Framework.Contracts.Services.Docker
{
    public class DockerVolumeMapping
    {
        public DirectoryInfo Source { get; set; }
        public string ContainerDestination { get; set; }
    }
}
