using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;

namespace Framework.Core.Services.DataAccess.EntityDescriptions
{
    public class PropertyGroupDescription<TEntity> : IPropertyGroupDescription
        where TEntity : Entity
    {
        private readonly string _name;

        public string Name { get { return _name; } }

        public PropertyGroupDescription(string name)
        {
            _name = name;
        }
    }
}
