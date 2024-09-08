using System.Linq.Expressions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Course.Project.Application.Paginations
{
    public interface IPaginationVehiculoRepository
    {
        Task<PagedResult<Vehiculo, VehiculoId>> GetPaginationResultAsync(
            Expression<Func<Vehiculo, bool>>? predicate,
            Func<IQueryable<Vehiculo>, IIncludableQueryable<Vehiculo, object>> include,
            int pageIndex,
            int pageSize,
            string orderBy,
            bool isAscending,
            bool disableTracking = true,
            CancellationToken cancellationToken = default
        );
    }
}