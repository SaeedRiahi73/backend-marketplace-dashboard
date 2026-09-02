using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Commands;

public sealed class CreateUserCommandRequest : IRequest<ResultInfo<UserDto>>
{
    public CreateUserDto CreateUser { get; set; } = new();
}
