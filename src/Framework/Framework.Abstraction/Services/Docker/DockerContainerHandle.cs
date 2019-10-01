namespace Framework.Contracts.Services.Docker
{
    public class DockerContainerHandle
    {
        public string ContainerId { get; private set; }

        public DockerContainerHandle(string containerId)
        {
            ContainerId = containerId;
        }
    }
}
