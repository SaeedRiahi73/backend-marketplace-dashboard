using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.User;
using Task_Application.Features.Users.Requests.Commands;
using Task_Application.Features.Users.Requests.Queries;

namespace Task_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin,Demo")]
    [HttpGet]
    public async Task<IActionResult> GetUsersList(
        [FromQuery] GetUsersFilterDto filter,
        CancellationToken cancellationToken)
    {
        GetUsersListQueryRequest request = new(filter);

        ResultInfo<PagedResultDto<UserDto>> response =
            await _mediator.Send(request, cancellationToken);

        return StatusCode((int)response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        GetUserByIdQueryRequest request = new(id);

        ResultInfo<GetUserByIdDto> response = await _mediator.Send(
            request,
            cancellationToken);

        return StatusCode((int)response.Status, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserDto createUser,
        CancellationToken cancellationToken)
    {
        CreateUserCommandRequest request = new()
        {
            CreateUser = createUser
        };

        ResultInfo<UserDto> response = await _mediator.Send(
            request,
            cancellationToken);

        return StatusCode((int)response.Status, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeUserStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeUserStatusDto userStatus,
        CancellationToken cancellationToken)
    {
        ChangeUserStatusCommandRequest request = new(
            id,
            userStatus);

        ResultInfo<UserDto> response = await _mediator.Send(
            request,
            cancellationToken);

        return StatusCode((int)response.Status, response);
    }
}
