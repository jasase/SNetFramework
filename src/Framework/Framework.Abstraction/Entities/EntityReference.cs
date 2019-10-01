using System;

namespace Framework.Contracts.Entities
{
    public class EntityReference : Entity
    {
        public Guid ReferencedEntityId { get; set; }
        public Guid SourceId { get; set; }
        public string TargetType { get; set; }
        public string SourceType { get; set; }

        public EntityReference(Entity entity)
        {
            ReferencedEntityId = entity.Id;
            TargetType = entity.GetType().FullName;
        }

        public EntityReference()
        { }

        public override bool Equals(object obj)
        {
            var other = obj as EntityReference;
            if (other != null)
            {
                return ReferencedEntityId.Equals(other.ReferencedEntityId);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ReferencedEntityId.GetHashCode() ^ 5463;
        }
    }
}
