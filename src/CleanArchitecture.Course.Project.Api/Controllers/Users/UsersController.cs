using Asp.Versioning;
using CleanArchitecture.Course.Project.Api.Utils;
using CleanArchitecture.Course.Project.Application.Users.GetUserDapperPag;
using CleanArchitecture.Course.Project.Application.Users.GetUserPagination;
using CleanArchitecture.Course.Project.Application.Users.GetUserSession;
using CleanArchitecture.Course.Project.Application.Users.LoginUser;
using CleanArchitecture.Course.Project.Application.Users.RegisterUser;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Course.Project.Api.Controllers.Users
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    // [ApiVersion(ApiVersions.V2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController(
        ISender mediator
    ) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet("session")]
        [HasPermission(Permissions.ReadUser)]
        [MapToApiVersion(ApiVersions.V1)]
        public async Task<IActionResult> GetSession(CancellationToken cancellationToken)
        {
            var query = new GetUserSessionQuery();
            var result = await _mediator.Send(query, cancellationToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("login")]	
        [AllowAnonymous]
        [MapToApiVersion(ApiVersions.V1)]
        public async Task<IActionResult> LoginV1([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        // [HttpPost("login")]	
        // [AllowAnonymous]
        // [MapToApiVersion(ApiVersions.V2)]
        // public async Task<IActionResult> LoginV2([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        // {
        //     var command = new LoginCommand(request.Email, request.Password);
        //     var result = await _mediator.Send(command, cancellationToken);
        //     if (result.IsFailure)
        //     {
        //         return Unauthorized(result.Error);
        //     }

        //     return Ok(result.Value);
        // }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.Email!, request.Password!, request.Name!, request.LastName!);
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("getPagination", Name = "GetUserPagination")]
        [ProducesResponseType(typeof(PagedResult<User, UserId>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<User, UserId>>> GetPagination(
            [FromQuery] GetUserPaginationQuery request, 
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("getDapperPagination", Name = "GetUserDapperPagination")]
        [ProducesResponseType(typeof(PagedDapperResult<UserPagData>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedDapperResult<UserPagData>>> GetDapperPagination(
            [FromQuery] GetUserDapperPagQuery request, 
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result.Value);
        }
    }
}