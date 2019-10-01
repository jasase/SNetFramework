using Framework.Contracts.Plugins;
using Framework.Contracts.Extension.EventService;

namespace Framework.Contracts.Messages
{
  public class PluginIsLoadedMessage : EventMessage
  {
    private PluginDescription _Description;

    public PluginIsLoadedMessage(PluginDescription description)
    {
      _Description = description;
    }
  }
}
