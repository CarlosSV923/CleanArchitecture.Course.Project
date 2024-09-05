using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos.Specifications
{
    public class VehiculoCountingSpecification(
        string? search
        ) : BaseSpecification<Vehiculo, VehiculoId>(
        x => string.IsNullOrEmpty(search) || x.Modelo == new Modelo(search)
        )
    {
        
    }
}