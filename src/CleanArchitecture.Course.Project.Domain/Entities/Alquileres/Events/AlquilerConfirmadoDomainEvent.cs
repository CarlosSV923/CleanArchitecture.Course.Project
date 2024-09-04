using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres.Events
{
    public sealed record AlquilerConfirmadoDomainEvent(AlquilerId AlquilerId) : IDomainEvent;
}