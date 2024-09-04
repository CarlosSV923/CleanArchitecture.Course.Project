using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using CleanArchitecture.Course.Project.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder
                .Property(rp => rp.PermissionId)
                .HasConversion(id => id!.Value, id => new PermissionId(id));
            
            builder.HasData(
                Create(Role.Cliente, Permissions.ReadUser),
                Create(Role.Administrador, Permissions.WriteUser),
                Create(Role.Administrador, Permissions.UpdateUser),
                Create(Role.Administrador, Permissions.DeleteUser),
                Create(Role.Administrador, Permissions.ReadUser)
            );

            
        }

        private static RolePermission Create(Role role, Permissions permission)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = new PermissionId((int)permission)
            };
        }
    }
}