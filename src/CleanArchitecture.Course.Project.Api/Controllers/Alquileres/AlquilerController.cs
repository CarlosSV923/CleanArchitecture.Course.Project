using Asp.Versioning;
using CleanArchitecture.Course.Project.Api.Utils;
using CleanArchitecture.Course.Project.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Course.Project.Application.Alquileres.Reservar;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Course.Project.Api.Controllers.Alquileres
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    // [ApiVersion(ApiVersions.V2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlquileresController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAlquiler(Guid Id, CancellationToken cancellationToken)
        {
            var query = new GetAlquilerQuery(Id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReservarAlquiler(AlquilerReservaRequest request, CancellationToken cancellationToken)
        {
            var command = new ReservarAlquilerCommand
            {
                UserId = request.UserId,
                VehiculoId = request.VehiculoId,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin
            };
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetAlquiler), new { Id = result.Value }, result.Value);
        }
    }
}