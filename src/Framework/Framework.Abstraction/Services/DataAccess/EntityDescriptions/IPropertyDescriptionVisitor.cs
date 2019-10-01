using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IPropertyDescriptionVisitor
    {
        void Handle(IStringPropertyDescription stringPropertyDescription);
        void Handle(IIntegerPropertyDescription integerPropertyDescription);
        void Handle(IDateTimePropertyDescription dateTimePropertyDescription);
        void Handle(IEntityReferencePropertyDescription dateTimePropertyDescription);
        void Handle(IBoolPropertyDescription dateTimePropertyDescription);
        void Handle<TOtherEntity>(IEntityPropertyDescription<TOtherEntity> entityPropertyDescription)
            where TOtherEntity : Entity;
    }
}