using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Reviews;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
            .ToTable("reviews");

            builder.HasKey(r => r.Id);

            builder
            .Property(r => r.Id)
            .HasConversion(id => id!.Value, value => new ReviewId(value));

            builder
            .Property(r => r.Rating)
            .HasConversion(rating => rating!.Value, value => Rating.Create(value).Value);

            builder
            .Property(r => r.Comentario)
            .HasMaxLength(500)
            .HasConversion(comentario => comentario!.Value, value => new Comentario(value));

            builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(r => r.VehiculoId);

            builder.HasOne<Alquiler>()
            .WithMany()
            .HasForeignKey(r => r.AlquilerId);

            builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
        }
    }
}