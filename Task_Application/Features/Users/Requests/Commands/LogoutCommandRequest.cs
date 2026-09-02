using MediatR;
using Task_Application.Common.Responses;

namespace Task_Application.Features.Users.Requests.Commands;

public sealed record LogoutCommandRequest(string RefreshToken)
    : IRequest<ResultInfo<bool>>;
