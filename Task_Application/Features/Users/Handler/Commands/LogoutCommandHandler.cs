using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Users.Handler.Command;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommandRequest, ResultInfo<bool>>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        IRefreshTokenService refreshTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultInfo<bool>> Handle(
        LogoutCommandRequest request,
        CancellationToken cancellationToken)
    {
        string tokenHash = _refreshTokenService.HashToken(
            request.RefreshToken);

        RefreshToken? refreshToken =
            await _refreshTokenRepository.GetByTokenHashAsync(
                tokenHash,
                cancellationToken);

        if (refreshToken is not null && !refreshToken.RevokedAt.HasValue)
        {
            refreshToken.Revoke();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return ResultInfo<bool>.Success(
            true,
            "User logged out successfully.");
    }
}
