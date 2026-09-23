using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Application.Common.Responses;
using Task_Application.Dtos.RefreshToken;
using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Api.Common.Cookies;

namespace Task_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] UserLoginDto user,
            CancellationToken cancellationToken)
        {

            LoginUserRequest  request = new LoginUserRequest { UserLoginDto = user };
            ResultInfo<LoginResponseDto> response = await _mediator.Send(
                request,
                cancellationToken);

            if (response.IsSuccess &&
                response.Data is not null &&
                response.Data.RefreshTokenCookie is not null &&
                !string.IsNullOrWhiteSpace(
                    response.Data.RefreshTokenCookie.Token))
            {
                RefreshTokenCookieHelper.Set(
                    Response,
                    response.Data.RefreshTokenCookie);
            }

            return StatusCode((int)response.Status, response);

        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            CancellationToken cancellationToken)
        {
            if (!Request.Cookies.TryGetValue(
                    RefreshTokenCookieHelper.CookieName,
                    out string? refreshToken) ||
                string.IsNullOrWhiteSpace(refreshToken))
            {
                ResultInfo<RefreshAccessTokenResponseDto> missingCookieResponse =
                    ResultInfo<RefreshAccessTokenResponseDto>.Failure(
                        new[] { "Refresh token cookie was not found." },
                        status: ResultStatus.Unauthorized);

                return StatusCode(
                    (int)missingCookieResponse.Status,
                    missingCookieResponse);
            }

            RefreshAccessTokenCommandRequest request = new(refreshToken);
            ResultInfo<RefreshAccessTokenResponseDto> response =
                await _mediator.Send(request, cancellationToken);

            if (response.IsSuccess &&
                response.Data?.RefreshTokenCookie is not null)
            {
                RefreshTokenCookieHelper.Set(
                    Response,
                    response.Data.RefreshTokenCookie);
            }
            else
            {
                RefreshTokenCookieHelper.Delete(Response);
            }

            return StatusCode((int)response.Status, response);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            CancellationToken cancellationToken)
        {
            ResultInfo<bool> response;

            if (Request.Cookies.TryGetValue(
                    RefreshTokenCookieHelper.CookieName,
                    out string? refreshToken) &&
                !string.IsNullOrWhiteSpace(refreshToken))
            {
                LogoutCommandRequest request = new(refreshToken);
                response = await _mediator.Send(request, cancellationToken);
            }
            else
            {
                response = ResultInfo<bool>.Success(
                    true,
                    "User logged out successfully.");
            }

            RefreshTokenCookieHelper.Delete(Response);

            return StatusCode((int)response.Status, response);
        }

        [AllowAnonymous]
        [EnableRateLimiting("demo-login")]
        [HttpPost("DemoLogin")]
        public async Task<IActionResult> DemoLogin(
            CancellationToken cancellationToken)
        {
            DemoLoginCommandRequest request = new DemoLoginCommandRequest();
            ResultInfo<LoginResponseDto> response = await _mediator.Send(
                request,
                cancellationToken);

            return StatusCode((int)response.Status, response);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = User.Identity?.Name
            });
        }

    }
}
