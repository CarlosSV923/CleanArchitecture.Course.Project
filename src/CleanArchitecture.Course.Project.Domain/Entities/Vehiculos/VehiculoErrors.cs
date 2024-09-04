using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos
{
    public static class VehiculoErrors {
        public static Error NotFound => new("Vehiculo.NotFound", "No existe el vehiculo solicitado");
        
    }
}