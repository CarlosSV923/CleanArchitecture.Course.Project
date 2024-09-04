using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Application.Users.RegisterUser
{
    internal class RegisterUserCommandHandler (
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.IsUserExistsAsync(new Email(request.Email), cancellationToken);

            if (userExists)
            {
                return Result.Failure<Guid>(UserErros.EmailAlreadyExists);
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = User.Create(
                new Name(request.Name),
                new LastName(request.LastName),
                new Email(request.Email),
                new PasswordHash(passwordHash)
            );

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return user.Id!.Value;
        }
    }
}