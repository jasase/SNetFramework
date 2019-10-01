using Framework.Contracts.Plugins;
using Framework.Contracts.Extension.EventService;

namespace Framework.Contracts.Messages
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
