using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres.Events
{
    public sealed record AlquilerCanceladoDomainEvent(AlquilerId AlquilerId) : IDomainEvent;
}