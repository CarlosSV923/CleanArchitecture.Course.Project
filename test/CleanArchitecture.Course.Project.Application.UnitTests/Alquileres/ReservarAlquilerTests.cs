using CleanArchitecture.Course.Project.Application.Abstractions.Clock;
using CleanArchitecture.Course.Project.Application.Alquileres.Reservar;
using CleanArchitecture.Course.Project.Application.UnitTests.Users;
using CleanArchitecture.Course.Project.Application.UnitTests.Vehiculos;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace CleanArchitecture.Course.Project.Application.UnitTests.Alquileres
{
    public class ReservarAlquilerTests
    {

        private readonly ReservarAlquilerCommandHandler _handler;

        private readonly IUserRepository _userRepository;

        private readonly IVehiculoRepository _vehiculoRepository;

        private readonly IAlquilerRepository _alquilerRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly DateTime UtcNow = DateTime.UtcNow;

        private readonly ReservarAlquilerCommand _command = new ReservarAlquilerCommand
        {
            UserId = Guid.NewGuid(),
            VehiculoId = Guid.NewGuid(),
            FechaInicio = new DateOnly(2024, 1, 1),
            FechaFin = new DateOnly(2025, 1, 1)
        };

        public ReservarAlquilerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _vehiculoRepository = Substitute.For<IVehiculoRepository>();
            _alquilerRepository = Substitute.For<IAlquilerRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.CurrentTime.Returns(UtcNow);

            _handler = new ReservarAlquilerCommandHandler(
                _userRepository,
                _vehiculoRepository,
                _alquilerRepository,
                _unitOfWork,
                new PrecioService(),
                dateTimeProvider
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_When_UserNull()
        {
            // Arrange
            _userRepository.GetByIdAsync(new UserId(_command.UserId), Arg.Any<CancellationToken>()).Returns((User?)null);

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Error.Should().Be(UserErros.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_When_VehiculoNull()
        {
            // Arrange
            var userMock = UserMock.Create();

            _userRepository.GetByIdAsync(
                new UserId(_command.UserId),
                Arg.Any<CancellationToken>()
            ).Returns(userMock);

            _vehiculoRepository.GetByIdAsync(
                new VehiculoId(_command.VehiculoId),
                Arg.Any<CancellationToken>()
            ).Returns((Vehiculo?)null);

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Error.Should().Be(VehiculoErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Shoud_ReturnFailure_WhenVehiculoIsAlquilado()
        {
            //Arrange
            var userMock = UserMock.Create();
            var vehiculoMock = VehiculoMock.Create();
            var duracion = DateRange.Create(
                _command.FechaInicio,
                _command.FechaFin
            );

            _userRepository.GetByIdAsync(
                new UserId(_command.UserId),
                Arg.Any<CancellationToken>()
            ).Returns(userMock);

            _vehiculoRepository.GetByIdAsync(
                new VehiculoId(_command.VehiculoId),
                Arg.Any<CancellationToken>()
            ).Returns(vehiculoMock);

            _alquilerRepository.IsOverlappingAsync(
                vehiculoMock,
                duracion,
                Arg.Any<CancellationToken>()
            ).Returns(true);

            //Act

            var result = await _handler.Handle(_command, CancellationToken.None);

            //Assert
            result.Error.Should().Be(AlquilerErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
        {
            //Arrange
            var userMock = UserMock.Create();
            var vehiculoMock = VehiculoMock.Create();
            var duracion = DateRange.Create(
                _command.FechaInicio,
                _command.FechaFin
            );

            _userRepository.GetByIdAsync(
                new UserId(_command.UserId),
                Arg.Any<CancellationToken>()
            ).Returns(userMock);

            _vehiculoRepository.GetByIdAsync(
                new VehiculoId(_command.VehiculoId),
                Arg.Any<CancellationToken>()
            ).Returns(vehiculoMock);

            _alquilerRepository.IsOverlappingAsync(
                vehiculoMock,
                duracion,
                Arg.Any<CancellationToken>()
            ).Returns(false);

            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

            //Act

            var result = await _handler.Handle(_command, CancellationToken.None);

            //Assert
            result.Error.Should().Be(AlquilerErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAlquilerIsReservado()
        {
            //Arrange
            var userMock = UserMock.Create();
            var vehiculoMock = VehiculoMock.Create();
            var duracion = DateRange.Create(
                _command.FechaInicio,
                _command.FechaFin
            );

            _userRepository.GetByIdAsync(
                new UserId(_command.UserId),
                Arg.Any<CancellationToken>()
            ).Returns(userMock);

            _vehiculoRepository.GetByIdAsync(
                new VehiculoId(_command.VehiculoId),
                Arg.Any<CancellationToken>()
            ).Returns(vehiculoMock);

            _alquilerRepository.IsOverlappingAsync(
                vehiculoMock,
                duracion,
                Arg.Any<CancellationToken>()
            ).Returns(false);

            //Act

            var result = await _handler.Handle(_command, CancellationToken.None);

            //Assert

            result.IsSucceeded.Should().BeTrue();
        }

    }
}
