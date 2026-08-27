using MediatR;
using Task_Application.Common.Constants;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.Security;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Command
{
    public sealed class DemoLoginCommandHandler
        : IRequestHandler<DemoLoginCommandRequest, ResultInfo<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public DemoLoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<ResultInfo<LoginResponseDto>> Handle(
            DemoLoginCommandRequest request,
            CancellationToken cancellationToken)
        {
            User? demoUser = await _userRepository.GetUserByUsernameAsync(
                SystemUserNames.Demo,
                cancellationToken);

            if (demoUser is null)
            {
                return ResultInfo<LoginResponseDto>.Failure(
                    ["Demo user was not found."],
                    status: ResultStatus.NotFound);
            }

            if (demoUser.Role != UserRole.Demo)
            {
                return ResultInfo<LoginResponseDto>.Failure(
                    ["Demo user is not available."],
                    status: ResultStatus.Forbidden);
            }

            LoginResponseDto response = _jwtService.GenerateToken(
                demoUser,
                TimeSpan.FromMinutes(30));

            return ResultInfo<LoginResponseDto>.Success(
                response,
                "Demo login successfully");
        }
    }
}
