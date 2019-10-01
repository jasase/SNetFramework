namespace Framework.Contracts.Plugins
{
  public interface IPlugin
  {
    PluginDescription Description { get; }
    bool IsActivated { get; }

    void Activate();
  }
}
