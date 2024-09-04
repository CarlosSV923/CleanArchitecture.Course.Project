using CleanArchitecture.Course.Project.Domain.Entities.Shared;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public record PrecioDetalle(Moneda PrecioPeriodo, Moneda Mantenimiento, Moneda Accesorios, Moneda Total);

}