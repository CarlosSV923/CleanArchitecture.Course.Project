using CleanArchitecture.Course.Project.Application.Users.GetUserPagination;
using CleanArchitecture.Course.Project.Application.Users.LoginUser;
using CleanArchitecture.Course.Project.Application.Users.RegisterUser;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Course.Project.Api.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(
        ISender mediator
    ) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpPost("login")]	
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

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
    }
}