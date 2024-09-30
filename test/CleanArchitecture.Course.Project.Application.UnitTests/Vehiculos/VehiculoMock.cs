
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Application.UnitTests.Vehiculos
{
    public static class VehiculoMock
    {
        public static Vehiculo Create() => new (
            new VehiculoId(Guid.NewGuid()),
            new Modelo("Toyota"),
            new Vin("12345678901234567"),
            new Direccion() {
                Calle = "Calle",
                Ciudad = "Ciudad",
                Departamento = "Departamento",
                Provincia = "Provincia",
                Pais = "Pais"
            },
            new Moneda(100m, TipoMoneda.USD),
            Moneda.Zero(),
            DateTime.UtcNow,
            []
        );
    }
}