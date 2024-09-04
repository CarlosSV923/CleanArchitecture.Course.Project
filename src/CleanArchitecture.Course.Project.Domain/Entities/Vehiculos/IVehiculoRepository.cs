namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos
{
    public interface IVehiculoRepository
    {
        Task<Vehiculo?> GetByIdAsync(VehiculoId id, CancellationToken cancellationToken = default);
    }
}