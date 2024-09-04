using CleanArchitecture.Course.Project.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(r => r.Id);

            builder.HasData(
                Role.GetValues()
            );

            builder
                .HasMany(x => x.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();
        }
    }
}