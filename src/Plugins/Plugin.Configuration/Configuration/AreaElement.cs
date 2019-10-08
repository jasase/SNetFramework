namespace ConfigurationPlugin.Configuration
{
    public abstract class AreaElement
    {
        public string Key { get; set; }

        protected AreaElement()
        {
            Key = string.Empty;
        }

        public abstract void Accept(IAreaElementVisitor visitor);
    }

    public interface IAreaElementVisitor
    {
        void Handle(Algorithm setting);
        void Handle(Setting setting);        
    }
}
