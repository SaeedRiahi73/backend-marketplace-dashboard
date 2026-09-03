using MediatR;
using Microsoft.Extensions.Options;
using Task_Application.Common.Responses;
using Task_Application.Common.Settings;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.RefreshToken;
using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;


namespace Task_Application.Features.Users.Handler.Command
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, ResultInfo<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RefreshTokenSettings _refreshTokenSettings;

        public LoginUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IOptions<RefreshTokenSettings> refreshTokenOptions)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _refreshTokenSettings = refreshTokenOptions.Value;
        }
        public async Task<ResultInfo<LoginResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            //User? user = await _userRepository.GetUserByUsernameAsync(request.UserLoginDto.UsernameOrEmail);


            User? user = await _userRepository.GetUserByUsernameOrEmailAsync(request.UserLoginDto.UsernameOrEmail);

            if (user == null)
                return ResultInfo<LoginResponseDto>.Failure(["Invalid username or email."],status: ResultStatus.Unauthorized);

            if (!_passwordHasher.VerifyPassword(request.UserLoginDto.Password, user.PasswordHash))
                return ResultInfo<LoginResponseDto>.Failure(["Invalid password"],status: ResultStatus.Unauthorized);

            if (!user.IsActive)
                return ResultInfo<LoginResponseDto>.Failure(["User account is inactive."],status: ResultStatus.Forbidden);

            bool isPersistent = request.UserLoginDto.RememberMe;
            int expirationDays = isPersistent
                ? _refreshTokenSettings.PersistentExpirationDays
                : _refreshTokenSettings.SessionExpirationDays;

            string rawRefreshToken = _refreshTokenService.GenerateToken();
            string refreshTokenHash = _refreshTokenService.HashToken(rawRefreshToken);
            DateTime refreshTokenExpiresAt = DateTime.UtcNow.AddDays(expirationDays);

            RefreshToken refreshToken = new RefreshToken(
                user.Id,
                refreshTokenHash,
                refreshTokenExpiresAt,
                isPersistent);

            await _refreshTokenRepository.AddAsync(refreshToken,cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            LoginResponseDto loginResponse = _jwtService.GenerateToken(user);
            loginResponse.RefreshTokenCookie = new RefreshTokenCookieDto
            {
                Token = rawRefreshToken,
                ExpiresAt = refreshTokenExpiresAt,
                IsPersistent = isPersistent
            };

            return ResultInfo<LoginResponseDto>.Success(loginResponse, "User login successfully");

        }
    }
}
