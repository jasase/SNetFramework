using System;
using Framework.Contracts.Entities;

namespace Framework.Contracts.Messages.EntityMessages
{
    public class EntityUpdatedMessage : EntityMessage
    {
        public EntityUpdatedMessage(Entity entity)
            : base(entity)
        { }
        public EntityUpdatedMessage(Type entityType, Guid entityId)
            : base(entityType, entityId)
        { }
    }
}
