using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Security;

namespace Task_Application.Features.Users.Requests.Commands
{
    public sealed record DemoLoginCommandRequest
        : IRequest<ResultInfo<LoginResponseDto>>;
}
