using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
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
