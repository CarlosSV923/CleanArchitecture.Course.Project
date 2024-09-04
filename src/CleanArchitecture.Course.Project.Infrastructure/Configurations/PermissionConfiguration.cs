using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasConversion(x => x!.Value, x => new PermissionId(x));

            builder
                .Property(x => x.Name)
                .HasConversion(x => x!.Value, x => new Name(x));

            IEnumerable<Permission> permissionData = Enum.GetValues<Permissions>()
                .Select(p => new Permission(new PermissionId((int)p), new Name(p.ToString())));
            
            builder.HasData(permissionData);

        }
    }
}