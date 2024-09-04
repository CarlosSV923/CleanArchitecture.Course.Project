namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos
{
    public record VehiculoId(Guid Value)
    {
        public static VehiculoId New() => new(Guid.NewGuid());
    }
}