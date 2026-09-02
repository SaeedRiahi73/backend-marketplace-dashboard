using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Queries;

public sealed record GetUserByIdQueryRequest(Guid UserId)
    : IRequest<ResultInfo<GetUserByIdDto>>;
