using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            builder.HasKey(ur => new { ur.RoleId, ur.UserId });

            builder
            .Property(ur => ur.UserId)
            .HasConversion(id => id!.Value, value => new UserId(value));

            
        }
    }
}