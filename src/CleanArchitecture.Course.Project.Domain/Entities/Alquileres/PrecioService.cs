using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public class PrecioService
    {
        public PrecioDetalle CalcularPrecio(Vehiculo vehiculo, DateRange duracion)
        {
            var moneda = vehiculo.Precio!.TipoMoneda;

            var precioPeriodo = new Moneda(vehiculo.Precio!.Monto * duracion.CantidadDias, moneda);

            decimal porcentageChange = vehiculo.Accesorios!.Sum(accesorio => accesorio switch
                {
                    Accesorio.AppleCarPlay or Accesorio.AndroidAuto => 0.05m,
                    Accesorio.AireAcondicionado => 0.01m,
                    Accesorio.GPS => 0.01m,
                    _ => 0
                });

            var precioAccesorios = porcentageChange > 0 ? new Moneda(precioPeriodo.Monto * porcentageChange, moneda) : Moneda.Zero(moneda);

            var precioTotal = Moneda.Zero(moneda);

            precioTotal += precioPeriodo;

            if(!vehiculo.Mantenimiento!.IsZero())
            {
                precioTotal += vehiculo.Mantenimiento;
            }

            precioTotal += precioAccesorios;

            return new PrecioDetalle(precioPeriodo, vehiculo.Mantenimiento, precioAccesorios, precioTotal);

        }

    }
}