using System.Linq;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Infrastructure.Repositories
{
    internal sealed class AlquilerRepository(ApplicationDbContext context) : Repository<Alquiler, AlquilerId>(context), IAlquilerRepository
    {

        private static readonly List<AlquilerStatus> ActiveStatuses = 
        [
            AlquilerStatus.Reservado,
            AlquilerStatus.Completado,
            AlquilerStatus.Confirmado
        ];

        public async Task<bool> IsOverlappingAsync(
            Vehiculo vehiculo,
            DateRange duracion,
            CancellationToken cancellationToken = default
            )
        {
            return await _context.Set<Alquiler>().AnyAsync(
                alquiler => alquiler.VehiculoId == vehiculo.Id
                            && alquiler.Duracion!.Start <= duracion.End
                            && alquiler.Duracion!.End >= duracion.Start
                            && alquiler.Status != null && ActiveStatuses.Contains(alquiler.Status.Value),
                cancellationToken
            );
        }
    }
}