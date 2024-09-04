namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos
{
    public record Direccion
    {
        public string? Calle { get; init; }
        public string? Ciudad { get; init; }
        public string? Departamento { get; init; }
        public string? Provincia { get; init; }
        public string? Pais
        {
            get; init;
        }
    }
}