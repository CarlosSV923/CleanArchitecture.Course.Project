using CleanArchitecture.Course.Project.Application.Abstractions.Email;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Users.Events;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Users.RegisterUser
{
    internal sealed class UserCreatedDomainEventHandler(
        IUserRepository userRepository,
        IEmailService emailService
    ) : INotificationHandler<UserCreatedDomainEvent>
    {

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

            if(user is null){
                return;
            }

            await _emailService.SendAsync(
                user.Email!,
                "Welcome to the platform",
                "Welcome to the platform",
                cancellationToken
            );

            throw new NotImplementedException();
        }
    }
}