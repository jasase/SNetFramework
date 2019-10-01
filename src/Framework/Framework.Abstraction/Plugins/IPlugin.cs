namespace Framework.Abstraction.Plugins
{
  public interface IPlugin
  {
    PluginDescription Description { get; }
    bool IsActivated { get; }

    void Activate();
  }
}
