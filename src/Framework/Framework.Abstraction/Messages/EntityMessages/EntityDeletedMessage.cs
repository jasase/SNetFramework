using System;
using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Messages.EntityMessages
{
    public class EntityDeletedMessage : EntityMessage
    {
        public EntityDeletedMessage(Entity entity)
            : base(entity)
        { }
        public EntityDeletedMessage(Type entityType, Guid entityId)
            : base(entityType, entityId)
        { }
    }
}
