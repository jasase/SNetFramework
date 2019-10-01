using System.Collections.Generic;

namespace Framework.Abstraction.Services.Docker
{
    public interface IDockerService
    {
        
        DockerContainerHandle StartContainer(string name, string image);
        DockerContainerHandle StartContainer(string name, 
                                             string image, 
                                             IEnumerable<DockerVolumeMapping> volumeMappings);

        void StopContainer(DockerContainerHandle handle);
        void WaitOnContainer(DockerContainerHandle handle);
        void RemoveContainer(DockerContainerHandle handle);
    }
}
