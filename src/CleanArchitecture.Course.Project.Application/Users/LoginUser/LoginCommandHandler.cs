using CleanArchitecture.Course.Project.Application.Abstractions.Authentication;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Application.Users.LoginUser
{
    internal sealed class LoginCommandHandler
    (
        IUserRepository userRepository,
        IJwtProvider jwtProvider
    ) : ICommandHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);
            if (user is null)
            {
                return Result.Failure<string>(UserErros.NotFound);
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash!.Value))
            {
                return Result.Failure<string>(UserErros.InvalidCredentials);
            }

            var token = await _jwtProvider.GenerateToken(user);

            return token;

        }
    }
}