using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Users.Events
{
    public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
}