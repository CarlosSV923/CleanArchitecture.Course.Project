using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserSession
{
    public sealed record GetUserSessionQuery : IQuery<UserResponse>;
}