using System;

namespace Framework.Contracts.Entities
{
    public abstract class Entity : EqualableDataObject
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
        }

        public Guid Id { get; set; }
        public virtual bool IsDeleted { get; set;}

        public override bool Equals(object obj)
        {
            return TryCast<Entity>(obj, Equals, false);
        }

        protected bool Equals(Entity other)
        {
            return Id.Equals(other.Id) &&
                   base.Equals(other);
        }
    }
}
