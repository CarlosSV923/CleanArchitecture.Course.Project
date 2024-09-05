using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Application.Vehiculos.PaginationVehiculos
{
    public sealed record PaginationVehiculosQuery : 
    SpecificationEntry,
    IQuery<PaginationResult<Vehiculo, VehiculoId>>
    {
        public string? Model { get; init; }
    }
}