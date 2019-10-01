using System;
using Framework.Contracts.Entities;

namespace Framework.Contracts.Messages.EntityMessages
{
    public class EntityInsertedMessage : EntityMessage
    {
        public EntityInsertedMessage(Entity entity)
            : base(entity)
        { }
        public EntityInsertedMessage(Type entityType, Guid entityId)
            : base(entityType, entityId)
        { }
    }
}
