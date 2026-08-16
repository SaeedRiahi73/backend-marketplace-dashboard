using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;
using Task_Application.Features.Users.Requests.Commands;

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
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {

            LoginUserRequest  request = new LoginUserRequest { UserLoginDto = user };
            ResultInfo<LoginResponseDto> response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUser)
        {
            RegisterUserRequest request = new RegisterUserRequest { CreateUser = createUser };
            ResultInfo<Guid> response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);

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
