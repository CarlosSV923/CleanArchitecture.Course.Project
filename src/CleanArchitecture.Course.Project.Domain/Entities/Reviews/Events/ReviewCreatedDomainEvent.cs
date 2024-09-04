using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Reviews.Events
{
    public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;
}