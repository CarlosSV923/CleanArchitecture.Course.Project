using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos.Specifications;
namespace CleanArchitecture.Course.Project.Application.Vehiculos.PaginationVehiculos
{
    public sealed class PaginationVehiculosQueryHandler(
        IVehiculoRepository vehiculoRepository
    ) : IQueryHandler<PaginationVehiculosQuery, PaginationResult<Vehiculo, VehiculoId>>
    {

        private readonly IVehiculoRepository _vehiculoRepository = vehiculoRepository;

        public async Task<Result<PaginationResult<Vehiculo, VehiculoId>>> Handle(PaginationVehiculosQuery request, CancellationToken cancellationToken)
        {
            var specification = new VehiculoPaginationSpecification(
                request.Sort,
                request.PageSize,
                request.PageIndex,
                request.Model
            );

            var vehiculos = await _vehiculoRepository.GetAllWithSpecAsync(specification, cancellationToken);

            var specCount = new VehiculoCountingSpecification(request.Model);

            var count = await _vehiculoRepository.CountAsync(specCount, cancellationToken);

            var rounded = Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(request.PageSize));

            var totalPages = Convert.ToInt32(rounded);

            var recordsBypage = vehiculos.Count;

            return new PaginationResult<Vehiculo, VehiculoId>{
                Data = vehiculos,
                TotalCount = count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                TotalByPage = recordsBypage
            };
        }
    }
}
