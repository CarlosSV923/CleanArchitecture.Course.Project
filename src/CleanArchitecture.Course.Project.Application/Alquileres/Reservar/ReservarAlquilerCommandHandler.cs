using CleanArchitecture.Course.Project.Application.Abstractions.Clock;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Application.Exceptions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Application.Alquileres.Reservar
{
    internal sealed class ReservarAlquilerCommandHandler(
        IUserRepository userRepository,
        IVehiculoRepository vehiculoRepository,
        IAlquilerRepository alquilerRepository,
        IUnitOfWork unitOfWork,
        PrecioService precioService,
        IDateTimeProvider dateTimeProvider
        ) : ICommandHandler<ReservarAlquilerCommand, Guid>
    {

        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IVehiculoRepository _vehiculoRepository = vehiculoRepository;
        private readonly IAlquilerRepository _alquilerRepository = alquilerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly PrecioService _precioService = precioService;

        public async Task<Result<Guid>> Handle(ReservarAlquilerCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(command.UserId), cancellationToken);

            if (user is null)
            {
                return Result.Failure<Guid>(UserErros.NotFound);
            }

            var vehiculo = await _vehiculoRepository.GetByIdAsync(new VehiculoId(command.VehiculoId), cancellationToken);

            if (vehiculo is null)
            {
                return Result.Failure<Guid>(VehiculoErrors.NotFound);
            }

            var duracion = DateRange.Create(command.FechaInicio, command.FechaFin);

            if (await _alquilerRepository.IsOverlappingAsync(vehiculo, duracion, cancellationToken))
            {
                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }

            try
            {
                var alquiler = Alquiler.Reservar(
                vehiculo,
                user.Id!,
                duracion,
                _dateTimeProvider.CurrentTime,
                _precioService
            );

                _alquilerRepository.Add(alquiler);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return alquiler.Id!.Value;
            }
            catch (ConsurrencyException)
            {
                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }

        }
    }
}