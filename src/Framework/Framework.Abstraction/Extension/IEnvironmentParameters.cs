using System.IO;

namespace Framework.Contracts.Extension
{
    public interface IEnvironmentParameters
    {
        DirectoryInfo ConfigurationDirectory { get; }

        DirectoryInfo ApplicationDirectory { get; }

        FileInfo ExecutablePath { get; }
    }
}
