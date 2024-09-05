using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos
{
    public interface IVehiculoRepository
    {
        Task<Vehiculo?> GetByIdAsync(VehiculoId id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Vehiculo>> GetAllWithSpecAsync(
            ISpecification<Vehiculo, VehiculoId> specification,
            CancellationToken cancellationToken = default
        );

        Task<int> CountAsync(
            ISpecification<Vehiculo, VehiculoId> specification,
            CancellationToken cancellationToken = default
        );
    }
}