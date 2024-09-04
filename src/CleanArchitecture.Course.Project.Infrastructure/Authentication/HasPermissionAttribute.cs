using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Course.Project.Infrastructure.Authentication
{
    public class HasPermissionAttribute(Permissions permission) : AuthorizeAttribute(policy: permission.ToString())
    {
    }
}