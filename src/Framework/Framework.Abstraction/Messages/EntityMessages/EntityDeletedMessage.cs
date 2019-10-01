using System;
using Framework.Contracts.Entities;

namespace Framework.Contracts.Messages.EntityMessages
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
