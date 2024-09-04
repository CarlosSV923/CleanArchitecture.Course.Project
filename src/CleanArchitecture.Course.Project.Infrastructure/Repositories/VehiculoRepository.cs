using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Infrastructure.Repositories
{
    internal sealed class VehiculoRepository(ApplicationDbContext context) : Repository<Vehiculo, VehiculoId>(context), IVehiculoRepository
    {
        
    }
}