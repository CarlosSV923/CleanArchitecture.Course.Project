using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Infrastructure
{
    public abstract class BaseTest
    {
        public static T AssertDomainEventWasPublished<T>(IEntity entity) where T : IDomainEvent
        {
            var domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault() ?? throw new Exception($"Domain event of type {typeof(T).Name} was not published.");
            return domainEvent!;
        }
    }
}
