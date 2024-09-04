using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Users.LoginUser
{
    public record LoginCommand(string Email, string Password) : ICommand<string>;
}