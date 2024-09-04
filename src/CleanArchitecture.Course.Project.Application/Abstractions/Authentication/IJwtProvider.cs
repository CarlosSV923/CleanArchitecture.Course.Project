using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<string> GenerateToken(User user);

        
    }
}