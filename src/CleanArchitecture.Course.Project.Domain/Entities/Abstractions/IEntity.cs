namespace CleanArchitecture.Course.Project.Domain.Entities.Abstractions
{
    public interface IEntity
    {
        IReadOnlyList<IDomainEvent> GetDomainEvents();
        void ClearDomainEvents();
    }
}