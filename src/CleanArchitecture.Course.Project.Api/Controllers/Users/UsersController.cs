using CleanArchitecture.Course.Project.Application.Users.LoginUser;
using CleanArchitecture.Course.Project.Application.Users.RegisterUser;
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
    }
}