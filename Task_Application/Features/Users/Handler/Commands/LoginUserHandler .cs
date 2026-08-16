using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;


namespace Task_Application.Features.Users.Handler.Command
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, ResultInfo<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<ResultInfo<LoginResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            //User? user = await _userRepository.GetUserByUsernameAsync(request.UserLoginDto.UsernameOrEmail);


            User? user = await _userRepository.GetUserByUsernameOrEmailAsync(request.UserLoginDto.UsernameOrEmail);

            if (user == null)
                return ResultInfo<LoginResponseDto>.Failure(["Invalid username or email."]);

            if (!_passwordHasher.VerifyPassword(request.UserLoginDto.Password, user.PasswordHash))
                return ResultInfo<LoginResponseDto>.Failure(["Invalid password"]);

            LoginResponseDto loginResponse = _jwtService.GenerateToken(user);

            return ResultInfo<LoginResponseDto>.Success(loginResponse, "User login successfully");

        }
    }
}
