using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.RefreshToken;
using Task_Application.Dtos.Security;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Users.Handler.Command;

public sealed class RefreshAccessTokenCommandHandler
    : IRequestHandler<
        RefreshAccessTokenCommandRequest,
        ResultInfo<RefreshAccessTokenResponseDto>>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserTokenValidationCache _tokenValidationCache;

    public RefreshAccessTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        IUserTokenValidationCache tokenValidationCache)
    {
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
        _tokenValidationCache = tokenValidationCache;
    }

    public async Task<ResultInfo<RefreshAccessTokenResponseDto>> Handle(
        RefreshAccessTokenCommandRequest request,
        CancellationToken cancellationToken)
    {
        string tokenHash = _refreshTokenService.HashToken(request.RefreshToken);

        RefreshToken? currentRefreshToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (currentRefreshToken is null)
            return InvalidRefreshToken();

        User? user = await _userRepository.GetByIdAsync(currentRefreshToken.UserId, cancellationToken);

        if (user is null)
            return InvalidRefreshToken();

        DateTime utcNow = DateTime.UtcNow;

        if (currentRefreshToken.RevokedAt.HasValue)
        {
            await RevokeAllSessionsAsync(
                user,
                utcNow,
                cancellationToken);

            user.InvalidateAllSessions();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _tokenValidationCache.Remove(user.Id);

            return InvalidRefreshToken();
        }

        if (!currentRefreshToken.IsActive(utcNow))
            return InvalidRefreshToken();

        if (!user.IsActive)
        {
            await RevokeAllSessionsAsync(user, utcNow, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return InvalidRefreshToken();
        }

        string newRawRefreshToken = _refreshTokenService.GenerateToken();
        string newRefreshTokenHash = _refreshTokenService.HashToken(newRawRefreshToken);

        RefreshToken newRefreshToken = new RefreshToken(
            user.Id,
            newRefreshTokenHash,
            currentRefreshToken.ExpiresAt,
            currentRefreshToken.IsPersistent
         );

        currentRefreshToken.Revoke(newRefreshToken.Id);

        await _refreshTokenRepository.AddAsync(
            newRefreshToken,
            cancellationToken
         );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        LoginResponseDto accessToken = _jwtService.GenerateToken(user);

        RefreshAccessTokenResponseDto response = new()
        {
            Token = accessToken.Token,
            UserName = accessToken.UserName,
            RoleId = accessToken.RoleId,
            Role = accessToken.Role,
            ExpireAt = accessToken.ExpireAt,
            RefreshTokenCookie = new RefreshTokenCookieDto
            {
                Token = newRawRefreshToken,
                ExpiresAt = newRefreshToken.ExpiresAt,
                IsPersistent = newRefreshToken.IsPersistent
            }
        };

        return ResultInfo<RefreshAccessTokenResponseDto>.Success(response, "Access token refreshed successfully.");
    }

    private async Task RevokeAllSessionsAsync(User user, DateTime utcNow, CancellationToken cancellationToken)
    {
        IReadOnlyList<RefreshToken> activeRefreshTokens =
            await _refreshTokenRepository.GetActiveByUserIdAsync(
                user.Id,
                utcNow,
                cancellationToken);

        foreach (RefreshToken refreshToken in activeRefreshTokens)
            refreshToken.Revoke();
    }

    private static ResultInfo<RefreshAccessTokenResponseDto>
        InvalidRefreshToken()
    {
        return ResultInfo<RefreshAccessTokenResponseDto>.Failure(["Invalid refresh token."], status: ResultStatus.Unauthorized);
    }
}
