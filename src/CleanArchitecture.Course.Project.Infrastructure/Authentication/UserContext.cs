using CleanArchitecture.Course.Project.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Course.Project.Infrastructure.Authentication
{
    internal sealed class UserContext(
        IHttpContextAccessor httpContextAccessor
    ) : IUserContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GetEmail => _httpContextAccessor.HttpContext?.User.GetUserEmail() ?? throw new InvalidOperationException("User not found");

        public Guid GetUserId => _httpContextAccessor.HttpContext?.User.GetUserId() ?? throw new InvalidOperationException("User not found");
    }
}