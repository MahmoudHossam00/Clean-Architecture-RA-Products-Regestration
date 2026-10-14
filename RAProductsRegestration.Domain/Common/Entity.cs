namespace RAProductsRegestration.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
        private readonly List<DomainEvent> _domainEvents = [];
        protected Entity()
        {
            Id = Guid.Empty;
        }
        protected Entity(Guid id)
        {
            //Id = id;
            Id = Guid.Empty;
        }
        public void AddDomainEvent(DomainEvent ev)
        {
            _domainEvents.Add(ev);
        }
        public void RemoveDomainEvent(DomainEvent ev)
        {
            _domainEvents.Remove(ev);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
