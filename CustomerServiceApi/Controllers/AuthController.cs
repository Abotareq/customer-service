using CustomerService.Application.Authentication.Commands.ConfirmEmail;
using CustomerService.Application.Authentication.Commands.ForgotPassword;
using CustomerService.Application.Authentication.Commands.Login;
using CustomerService.Application.Authentication.Commands.RefreshToken;
using CustomerService.Application.Authentication.Commands.Register;
using CustomerService.Application.Authentication.Commands.ResetPassword;
using CustomerService.Application.Users.Queries.GetUserProfile;
using CustomerService.Contracts.Authentication;
using CustomerService.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ApiController
    {
        private readonly ISender _mediator;

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue("sub")!);
        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role)!;
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var command = new ForgotPasswordCommand(request.Email);
            var result = await _mediator.Send(command);
            return result.Match(_ => Ok(new { message = "If that email exists, a reset link has been sent." }), Problem);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.UserId, request.Token, request.NewPassword);
            var result = await _mediator.Send(command);
            return result.Match(_ => Ok(new { message = "Password reset successfully." }), Problem);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetUserProfileQuery(CurrentUserId, CurrentUserRole);
            var result = await _mediator.Send(query);
            return result.Match(response => Ok(response), Problem);
        }
    }
}
