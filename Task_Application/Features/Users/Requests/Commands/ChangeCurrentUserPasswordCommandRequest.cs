using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.User.Password;

namespace Task_Application.Features.Users.Requests.Commands;

public sealed record ChangeCurrentUserPasswordCommandRequest(
    ChangeCurrentUserPasswordDto Password)
    : IRequest<ResultInfo<Unit>>;
