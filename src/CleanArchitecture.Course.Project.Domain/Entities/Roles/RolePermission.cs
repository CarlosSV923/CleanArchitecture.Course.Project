using CleanArchitecture.Course.Project.Domain.Entities.Permissions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Roles
{
    public sealed class RolePermission
    {
        public int RoleId { get; set; }
        public PermissionId? PermissionId { get; set; }
    }
    
}