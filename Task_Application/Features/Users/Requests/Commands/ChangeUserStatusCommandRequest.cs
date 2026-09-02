using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Commands;

public sealed record ChangeUserStatusCommandRequest(
    Guid UserId,
    ChangeUserStatusDto UserStatus)
    : IRequest<ResultInfo<UserDto>>;
