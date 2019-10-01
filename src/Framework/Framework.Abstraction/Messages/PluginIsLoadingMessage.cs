using Framework.Abstraction.Plugins;
using Framework.Abstraction.Extension.EventService;

namespace Framework.Abstraction.Messages
{
    public class PluginIsLoadingMessage : EventMessage
    {
        private PluginDescription _Description;

        public PluginDescription Description { get { return _Description; } }

        public PluginIsLoadingMessage(PluginDescription description)
        {
            _Description = description;
        }        
    }
}
