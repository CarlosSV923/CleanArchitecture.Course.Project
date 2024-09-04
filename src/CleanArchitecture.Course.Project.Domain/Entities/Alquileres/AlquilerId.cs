namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public record AlquilerId(Guid Value)
    {
        public static AlquilerId New() => new(Guid.NewGuid());
    }
}