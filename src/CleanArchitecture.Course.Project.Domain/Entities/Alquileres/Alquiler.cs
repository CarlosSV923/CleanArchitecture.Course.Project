using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres.Events;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public sealed class Alquiler : Entity<AlquilerId>
    {
        private Alquiler() { }
        private Alquiler(
            AlquilerId id,
            VehiculoId vehiculoId,
            UserId userId,
            AlquilerStatus? status,
            DateRange? duracion,
            Moneda? precioPeriodo,
            Moneda? precioMantenimiento,
            Moneda? precioAccesorios,
            Moneda? precioTotal,
            DateTime? fechaCreacion
        ) : base(id)
        {
            VehiculoId = vehiculoId;
            UserId = userId;
            Status = status;
            Duracion = duracion;
            PrecioPeriodo = precioPeriodo;
            PrecioMantenimiento = precioMantenimiento;
            PrecioAccesorios = precioAccesorios;
            PrecioTotal = precioTotal;
            FechaCreacion = fechaCreacion;
        }

        public VehiculoId? VehiculoId { get; private set; }
        public UserId? UserId { get; private set; }
        public AlquilerStatus? Status { get; private set; }

        public DateRange? Duracion { get; private set; }

        public Moneda? PrecioPeriodo { get; private set; }

        public Moneda? PrecioMantenimiento { get; private set; }

        public Moneda? PrecioAccesorios { get; private set; }

        public Moneda? PrecioTotal { get; private set; }

        public DateTime? FechaCreacion { get; private set; }

        public DateTime? FechaConfirmacion { get; private set; }

        public DateTime? FechaCompletado { get; private set; }

        public DateTime? FechaRechazo { get; private set; }

        public DateTime? FechaCancelacion { get; private set; }

        public static Alquiler Reservar(
            Vehiculo vehiculo,
            UserId userId,
            DateRange duracion,
            DateTime fechaCreacion,
            PrecioService precioService
        )
        {
            var precioDetalle = precioService.CalcularPrecio(vehiculo, duracion);
            var alquiler =  new Alquiler(
                AlquilerId.New(),
                vehiculo.Id!,
                userId,
                AlquilerStatus.Reservado,
                duracion,
                precioDetalle.PrecioPeriodo,
                precioDetalle.Mantenimiento,
                precioDetalle.Accesorios,
                precioDetalle.Total,
                fechaCreacion
            );

            alquiler.AddDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id!));
            vehiculo.FechaUltimoAlquiler = fechaCreacion;
            return alquiler;
        }

        public Result Confirmar(DateTime fechaConfirmacion)
        {
            if (Status != AlquilerStatus.Reservado)
            {
                return Result.Failure(AlquilerErrors.NotReserved);
            }

            Status = AlquilerStatus.Confirmado;
            FechaConfirmacion = fechaConfirmacion;
            AddDomainEvent(new AlquilerConfirmadoDomainEvent(Id!));
            return Result.Success();
        }

        public Result Rechazar(DateTime fechaRechazo)
        {
            if (Status != AlquilerStatus.Reservado)
            {
                return Result.Failure(AlquilerErrors.NotReserved);
            }

            Status = AlquilerStatus.Rechazado;
            FechaRechazo = fechaRechazo;
            AddDomainEvent(new AlquilerRechazadoDomainEvent(Id!));
            return Result.Success();
        }

        public Result Cancelar(DateTime fechaCancelacion)
        {
            if (Status != AlquilerStatus.Confirmado)
            {
                return Result.Failure(AlquilerErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if(currentDate > Duracion!.Start)
            {
                return Result.Failure(AlquilerErrors.AlreadyStarted);
            }

            Status = AlquilerStatus.Cancelado;
            FechaCancelacion = fechaCancelacion;
            AddDomainEvent(new AlquilerCanceladoDomainEvent(Id!));
            return Result.Success();
        }

        public Result Completar(DateTime fechaCompletado)
        {
            if (Status != AlquilerStatus.Confirmado)
            {
                return Result.Failure(AlquilerErrors.NotConfirmed);
            }

            Status = AlquilerStatus.Completado;
            FechaCompletado = fechaCompletado;
            AddDomainEvent(new AlquilerCompletadoDomainEvent(Id!));
            return Result.Success();	
        }
    }
}