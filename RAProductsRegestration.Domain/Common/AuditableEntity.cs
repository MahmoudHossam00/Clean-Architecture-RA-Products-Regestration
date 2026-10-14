

namespace RAProductsRegestration.Domain.Common
{
    public abstract class AuditableEntity:Entity
    {
        protected AuditableEntity()
        {
        }

        protected AuditableEntity(Guid id)
            : base(id)
        {
        }

        public DateTimeOffset CreatedAtUtc
        {
            get;
            private set;
        }

        public string? CreatedBy
        {
            get;
            private set;
        }

        public DateTimeOffset? LastModifiedUtc
        {
            get;
            private set;
        }

        public string? ModifiedBy
        {
            get;
            private set;
        }
    }
}
