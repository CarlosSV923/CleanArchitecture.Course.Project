using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Vehiculos
{
    internal static class VehiculoMock
    {
        public static Vehiculo Create(
            Moneda precio,
            Moneda? mantenimiento = null
        )
        {
            return new Vehiculo(
                VehiculoId.New(),
                new Modelo("Toyota"),
                new Vin("80293859"),
                new Direccion {
                    Calle = "Calle 123",
                    Ciudad = "Ciudad",
                    Departamento = "Departamento",
                    Provincia = "Provincia",
                    Pais = "Pais"
                } ,
                precio,
                mantenimiento ?? Moneda.Zero(),
                DateTime.UtcNow.AddYears(-1),
                []
            );
        }
    }
}