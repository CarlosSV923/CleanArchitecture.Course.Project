using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Users.RegisterUser
{
    public record RegisterUserCommand(string Email, string Password, string Name, string LastName) : ICommand<Guid>;
}