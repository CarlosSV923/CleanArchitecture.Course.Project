using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    internal sealed class AlquilerCongfiguration : IEntityTypeConfiguration<Alquiler>
    {
        public void Configure(EntityTypeBuilder<Alquiler> builder)
        {
            builder
            .ToTable("alquileres");

            builder
            .HasKey(a => a.Id);

            builder
            .Property(a => a.Id)
            .HasConversion(id => id!.Value, value => new AlquilerId(value));

            builder
            .OwnsOne(a => a.PrecioPeriodo, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .OwnsOne(a => a.PrecioTotal, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .OwnsOne(a => a.PrecioAccesorios, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .OwnsOne(a => a.PrecioMantenimiento, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .OwnsOne(a => a.Duracion);

            builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(a => a.VehiculoId);

            builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId);
        }
    }
}