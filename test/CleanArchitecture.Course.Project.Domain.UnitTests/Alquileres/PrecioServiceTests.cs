using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.UnitTests.Vehiculos;
using Xunit;
using FluentAssertions;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Alquileres
{
    public class PrecioServiceTests
    {
        [Fact]
        public void CalcularPrecio_Should_ReturnCorrectPrecio()
        {
            // Arrange
            var precio = new Moneda(10.0m, TipoMoneda.USD);
            var periodo = DateRange.Create(
                new DateOnly(2024, 1, 1),
                new DateOnly(2025, 1, 1)
            );

            var expectedTotal = new Moneda(
                precio.Monto * periodo.CantidadDias,
                TipoMoneda.USD
            );

            var vehiculo = VehiculoMock.Create(precio);

            var precioService = new PrecioService();


            // Act

            var precioDetalleResult = precioService.CalcularPrecio(vehiculo, periodo);

            // Assert

            precioDetalleResult.Total.Should().Be(expectedTotal);
        }

        [Fact]
        public void CalcularPrecio_Should_ReturnCorrectPrecio_WhenMantenimientoIsIncluded()
        {
            // Arrange
            var precio = new Moneda(10.0m, TipoMoneda.USD);
            var mantenimiento = new Moneda(1.0m, TipoMoneda.USD);
            var periodo = DateRange.Create(
                new DateOnly(2024, 1, 1),
                new DateOnly(2025, 1, 1)
            );

            var expectedTotal = new Moneda(
                precio.Monto * periodo.CantidadDias,
                TipoMoneda.USD
            );

            expectedTotal += mantenimiento;

            var vehiculo = VehiculoMock.Create(precio, mantenimiento);

            var precioService = new PrecioService();

            // Act
            var precioDetalleResult = precioService.CalcularPrecio(vehiculo, periodo);

            // Assert
            precioDetalleResult.Total.Should().Be(expectedTotal);

        }
    }
}