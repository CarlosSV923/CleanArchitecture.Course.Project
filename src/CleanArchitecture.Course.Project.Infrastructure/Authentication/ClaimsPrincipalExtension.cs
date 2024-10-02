using System.Security.Claims;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Newtonsoft.Json;

namespace CleanArchitecture.Course.Project.Infrastructure.Authentication
{
    internal static class ClaimsPrincipalExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal? claimsPrincipal)
        {
           return Guid.TryParse(claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : throw new InvalidOperationException("User not found");
        }

        public static string GetUserEmail(this ClaimsPrincipal? claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(ClaimTypes.Email) 
                ?? throw new InvalidOperationException("User not found");
        }
    }
}