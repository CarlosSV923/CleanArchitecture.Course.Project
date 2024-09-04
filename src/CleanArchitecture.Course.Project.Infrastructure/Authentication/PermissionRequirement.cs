using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Course.Project.Infrastructure.Authentication
{
    public class PermissionRequirement(
        string? permission  
    ) : IAuthorizationRequirement {
        public string? Permission { get; } = permission;
    }
}