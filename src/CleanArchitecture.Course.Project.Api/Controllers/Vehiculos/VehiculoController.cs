using CleanArchitecture.Course.Project.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using CleanArchitecture.Course.Project.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Course.Project.Api.Controllers.Vehiculos
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController(
        ISender mediator
    ) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HasPermission(Permissions.ReadUser)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVehiculos(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
        {
            var query = new SearchVehiculosQuery(startDate, endDate);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result.Value);
        }
    }
}