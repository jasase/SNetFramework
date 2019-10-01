using System.IO;

namespace Framework.Abstraction.Extension
{
    public interface IEnvironmentParameters
    {
        DirectoryInfo ConfigurationDirectory { get; }

        DirectoryInfo ApplicationDirectory { get; }

        FileInfo ExecutablePath { get; }
    }
}
