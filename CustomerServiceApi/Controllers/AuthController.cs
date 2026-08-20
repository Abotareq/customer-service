using CustomerService.Application.Authentication.Commands.ConfirmEmail;
using CustomerService.Application.Authentication.Commands.Login;
using CustomerService.Application.Authentication.Commands.RefreshToken;
using CustomerService.Application.Authentication.Commands.Register;
using CustomerService.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ApiController
    {
        private readonly ISender _mediator;

        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(request.FullName, request.Email, request.Password);
            var result = await _mediator.Send(command);

            return result.Match(authResponse => Ok(authResponse), Problem);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return result.Match(authResponse => Ok(authResponse), Problem);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
            var result = await _mediator.Send(command);

            return result.Match(authResponse => Ok(authResponse), Problem);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
        {
            var command = new ConfirmEmailCommand(userId, token);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => Content("<h2>Email confirmed successfully. You may now log in.</h2>", "text/html"),
                Problem);
        }
    }
}
