using Framework.Abstraction.Plugins;
using Framework.Abstraction.Extension.EventService;

namespace Framework.Abstraction.Messages
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
