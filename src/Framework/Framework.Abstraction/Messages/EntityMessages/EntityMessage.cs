using Framework.Contracts.Entities;
using Framework.Contracts.Extension.EventService;
using System;

namespace Framework.Contracts.Messages.EntityMessages
{
    public abstract class EntityMessage : EventMessage
    {
        public Type EntityType { get; private set; }
        public Guid EntityId { get; private set; }

        public EntityMessage(Entity entity)
            : this(entity.GetType(), entity.Id)
        { }

        public EntityMessage(Type entityType, Guid entityId)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
