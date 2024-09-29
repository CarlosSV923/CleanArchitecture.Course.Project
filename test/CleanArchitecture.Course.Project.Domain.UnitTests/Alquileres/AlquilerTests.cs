using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.UnitTests.Infrastructure;
using CleanArchitecture.Course.Project.Domain.UnitTests.Users;
using CleanArchitecture.Course.Project.Domain.UnitTests.Vehiculos;
using Xunit;
using FluentAssertions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres.Events;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Alquileres
{
    public class AlquilerTests : BaseTest
    {
        [Fact]
        public void Reserva_Should_RaiseAlquilerReservaDomainEvent()
        {
            //Arrange

            var user = User.Create(
                                UserMock.Name,
                                UserMock.LastName,
                                UserMock.Email,
                                UserMock.Password
                                );

            var precio = new Moneda(10.0m, TipoMoneda.USD);
            var duracion = DateRange.Create(
                new DateOnly(2024, 1, 1),
                new DateOnly(2025, 1, 1)
            );

            var vehiculo = VehiculoMock.Create(precio);
            var precioService = new PrecioService();

            //Act
            var alquiler = Alquiler.Reservar(
                vehiculo,
                user.Id!,
                duracion,
                DateTime.UtcNow,
                precioService
            );

            //Assert
            var domainEvent = AssertDomainEventWasPublished<AlquilerReservadoDomainEvent>(alquiler);
            domainEvent.AlquierId.Should().Be(alquiler.Id);
        }
    }
}