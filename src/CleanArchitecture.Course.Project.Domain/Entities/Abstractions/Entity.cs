namespace CleanArchitecture.Course.Project.Domain.Entities.Abstractions
{
    public abstract class Entity<TEntityId> : IEntity
    {

        protected Entity() { }
        protected Entity(TEntityId id) => Id = id;
        private readonly List<IDomainEvent> _domainEvents = [];
        public TEntityId? Id { get; init; }

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent); 
    }
}