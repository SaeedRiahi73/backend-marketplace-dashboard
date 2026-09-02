using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Queries;

public sealed record GetUsersListQueryRequest(GetUsersFilterDto Filter)
    : IRequest<ResultInfo<PagedResultDto<UserDto>>>;
