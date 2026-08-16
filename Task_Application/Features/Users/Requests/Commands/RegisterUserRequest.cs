using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.User;

namespace Task_Application.Features.Users.Requests.Commands
{
    public class RegisterUserRequest : IRequest<ResultInfo<Guid>>
    {
        public CreateUserDto CreateUser { get; set; }
        //public RegisterUserRequest(string userName, string password)
        //{
        //    this.userName = userName;
        //    this.password = password;
        //}
    }
}
