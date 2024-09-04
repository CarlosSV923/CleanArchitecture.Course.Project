using CleanArchitecture.Course.Project.Application.Abstractions.Email;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres.Events;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Alquileres.Reservar{
    internal sealed class ReservarAlquilerDomainEventHandler(
        IAlquilerRepository alquilerRepository,
        IUserRepository userRepository,
        IEmailService emailService
    )
    
     : INotificationHandler<AlquilerReservadoDomainEvent>
    {
        private readonly IAlquilerRepository _alquilerRepository = alquilerRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        public async Task Handle(AlquilerReservadoDomainEvent notification, CancellationToken cancellationToken)
        {
            var alquiler = await _alquilerRepository.GetByIdAsync(notification.AlquierId, cancellationToken);

            if(alquiler is null)
            {
                return;
            }

            var user = await _userRepository.GetByIdAsync(alquiler.UserId!, cancellationToken);

            if(user is null)
            {
                return;
            }

            await _emailService.SendAsync(user.Email!, "Alquiler reservado", $"El alquiler {alquiler.Id} ha sido reservado", cancellationToken);
        }
    }
}
