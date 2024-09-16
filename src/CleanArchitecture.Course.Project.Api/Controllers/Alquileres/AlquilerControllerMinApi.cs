using MediatR;
using CleanArchitecture.Course.Project.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Course.Project.Application.Alquileres.Reservar;
using Asp.Versioning;
using Asp.Versioning.Builder;
using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
namespace CleanArchitecture.Course.Project.Api.Controllers.Alquileres
{
    public static class AlquillerControllerMinApi
    {
        public static IEndpointRouteBuilder MapAlquilerEndpoints(this IEndpointRouteBuilder endpoints)
        {

            endpoints.MapGet(
                "alquileres/{Id}",
                GetAlquiler
            )
            .RequireAuthorization(
                Permissions.ReadUser.ToString()
            )
            .WithName(nameof(GetAlquiler));
            // .WithApiVersionSet(apiVersionSet);

            endpoints.MapPost(
                "alquileres",
                ReservarAlquiler
            )
            .RequireAuthorization(
                Permissions.ReadUser.ToString(),
                Permissions.WriteUser.ToString()
            )
            .WithName(nameof(ReservarAlquiler));
            // .WithApiVersionSet(apiVersionSet);

            return endpoints;
        }

        private static async Task<IResult> GetAlquiler(Guid Id, ISender mediator, CancellationToken cancellationToken)
        {
            var query = new GetAlquilerQuery(Id);
            var result = await mediator.Send(query, cancellationToken);
            return result.IsSucceeded ? Results.Ok(result.Value) : Results.NotFound();
        }

        private static async Task<IResult> ReservarAlquiler(AlquilerReservaRequest request, ISender mediator, CancellationToken cancellationToken)
        {
            var command = new ReservarAlquilerCommand
            {
                UserId = request.UserId,
                VehiculoId = request.VehiculoId,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin
            };
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.CreatedAtRoute(nameof(GetAlquiler), new { Id = result.Value }, result.Value);
        }
    }
}