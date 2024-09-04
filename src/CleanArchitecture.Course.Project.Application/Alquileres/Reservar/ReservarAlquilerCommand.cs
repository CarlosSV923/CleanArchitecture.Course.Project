using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Alquileres.Reservar
{
    public record ReservarAlquilerCommand : ICommand<Guid>
    {
        public Guid UserId { get; init; }
        public Guid VehiculoId { get; init; }

        public DateOnly FechaInicio { get; init; }

        public DateOnly FechaFin { get; init; }
    }
}