using System;
using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Messages.EntityMessages
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
