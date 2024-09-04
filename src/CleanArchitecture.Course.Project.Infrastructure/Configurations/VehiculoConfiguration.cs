using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Course.Project.Infrastructure.Configurations
{
    internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
    {
        public void Configure(EntityTypeBuilder<Vehiculo> builder)
        {
            builder
            .ToTable("vehiculos")
            .HasKey(v => v.Id);

            builder
            .Property(v => v.Id)
            .HasConversion(id => id!.Value, value => new VehiculoId(value));

            builder.OwnsOne(v => v.Direccion);

            builder
            .Property(v => v.Modelo)
            .HasMaxLength(200)
            .HasConversion(modelo => modelo!.Value, value => new Modelo(value));

            builder
            .Property(v => v.Vin)
            .HasMaxLength(500)
            .HasConversion(vin => vin!.Value, value => new Vin(value));

            builder
            .OwnsOne(v => v.Precio, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .OwnsOne(v => v.Mantenimiento, priceBuilder =>
            {
                priceBuilder
                .Property(moneda => moneda.TipoMoneda)
                .HasConversion(moneda => moneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder
            .Property<uint>("Version").IsRowVersion();
        }
    }
}