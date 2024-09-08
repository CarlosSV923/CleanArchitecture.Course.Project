using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Application.Vehiculos.PaginationLinq
{
    public record GetPaginationLinqQuery : PaginationParams, IQuery<PagedResult<Vehiculo, VehiculoId>>
    {
        
    }
}