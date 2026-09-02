using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.RefreshToken;

namespace Task_Application.Features.Users.Requests.Commands;

public sealed record RefreshAccessTokenCommandRequest(
    string RefreshToken)
    : IRequest<ResultInfo<RefreshAccessTokenResponseDto>>;
