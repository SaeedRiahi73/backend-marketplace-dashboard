using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Commands
{
    public record LoginUserRequest() : IRequest<ResultInfo<LoginResponseDto>>
    {
        public UserLoginDto UserLoginDto { get; set; }
    }
}
