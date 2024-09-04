using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Vehiculos.SearchVehiculos
{
    public sealed record SearchVehiculosQuery(DateOnly FechaInicio, DateOnly FechaFin) : IQuery<IReadOnlyList<VehiculoResponse>>
    {

    }
}