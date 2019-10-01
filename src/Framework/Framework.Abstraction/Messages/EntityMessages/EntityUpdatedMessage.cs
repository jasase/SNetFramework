using System;
using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Messages.EntityMessages
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
