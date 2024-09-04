using FluentValidation;

namespace CleanArchitecture.Course.Project.Application.Alquileres.Reservar
{
    public class ReservarAlquilerCommandValidator : AbstractValidator<ReservarAlquilerCommand>
    {
        public ReservarAlquilerCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.VehiculoId).NotEmpty();
            RuleFor(x => x.FechaInicio).NotEmpty();
            RuleFor(x => x.FechaFin).NotEmpty();
            RuleFor(x => x.FechaInicio).LessThan(x => x.FechaFin);
        }
    }
}