using Asp.Versioning;
using CleanArchitecture.Course.Project.Api.Utils;
using CleanArchitecture.Course.Project.Application.Vehiculos.PaginationLinq;
using CleanArchitecture.Course.Project.Application.Vehiculos.PaginationVehiculos;
using CleanArchitecture.Course.Project.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using CleanArchitecture.Course.Project.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Course.Project.Api.Controllers.Vehiculos
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(ApiVersions.V1)]
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

        [AllowAnonymous]
        [HttpGet("searchPagination", Name = "SearchVehiculosPagination")]
        [ProducesResponseType(typeof(PaginationResult<Vehiculo, VehiculoId>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationResult<Vehiculo, VehiculoId>>> SearchVehiculosPagination(
            [FromQuery] PaginationVehiculosQuery query,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("getPagination", Name = "GetVehiculosPagination")]   
        [ProducesResponseType(typeof(PagedResult<Vehiculo, VehiculoId>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<Vehiculo, VehiculoId>>> GetPagination(
            [FromQuery] GetPaginationLinqQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result.Value);
        }     
    }
}