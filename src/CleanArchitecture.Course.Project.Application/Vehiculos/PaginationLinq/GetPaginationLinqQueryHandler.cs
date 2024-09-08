using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Application.Paginations;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Application.Vehiculos.PaginationLinq
{
    internal sealed class GetPaginationLinqQueryHandler(
        IPaginationVehiculoRepository repository
    ) : IQueryHandler<GetPaginationLinqQuery, PagedResult<Vehiculo, VehiculoId>>
    {
        private readonly IPaginationVehiculoRepository _repository = repository;
        public async Task<Result<PagedResult<Vehiculo, VehiculoId>>> Handle(GetPaginationLinqQuery query, CancellationToken cancellationToken = default)
        {
            var predicate = PredicateBuilder.New<Vehiculo>(true);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                predicate = predicate.Or(vehiculo => vehiculo.Modelo!.Value.Contains(query.Search));
            }

            var result = await _repository.GetPaginationResultAsync(
                predicate,
                p => p.Include(x => x.Direccion!),
                query.PageIndex,
                query.PageSize,
                query.OrderBy!,
                query.IsAscending,
                true,
                cancellationToken
            );

            return result;
        }
    }
}