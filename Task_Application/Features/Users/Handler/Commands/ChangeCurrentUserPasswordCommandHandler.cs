using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Entities;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Commands;

public sealed class ChangeCurrentUserPasswordCommandHandler : IRequestHandler<ChangeCurrentUserPasswordCommandRequest, ResultInfo<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserTokenValidationCache _tokenValidationCache;

    public ChangeCurrentUserPasswordCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPasswordHasher passwordHasher,
        IUserTokenValidationCache tokenValidationCache)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
        _tokenValidationCache = tokenValidationCache;
    }

    public async Task<ResultInfo<Unit>> Handle(ChangeCurrentUserPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUserService.UserId;

        if (currentUserId is null)
            return ResultInfo<Unit>.Failure(["The user is not authenticated."], status: ResultStatus.Unauthorized);

        User? user = await _userRepository.GetByIdAsync(currentUserId.Value, cancellationToken);

        if (user is null)
            return ResultInfo<Unit>.Failure(["User not found."], status: ResultStatus.NotFound);

        if (user.Role == UserRole.Demo)
            return ResultInfo<Unit>.Failure(["Demo users cannot change their password."], status: ResultStatus.Forbidden);

        bool currentPasswordIsValid = _passwordHasher.VerifyPassword(request.Password.CurrentPassword, user.PasswordHash);

        if (!currentPasswordIsValid)
            return ResultInfo<Unit>.Failure(["Current password is incorrect."], status: ResultStatus.BadRequest);

        string newPasswordHash = _passwordHasher.GenerateHash(request.Password.NewPassword);

        IReadOnlyList<RefreshToken> activeRefreshTokens =
            await _refreshTokenRepository.GetActiveByUserIdAsync(
                user.Id,
                DateTime.UtcNow,
                cancellationToken);

        user.ChangePassword(newPasswordHash);

        foreach (RefreshToken refreshToken in activeRefreshTokens)
            refreshToken.Revoke();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _tokenValidationCache.Remove(user.Id);

        return ResultInfo<Unit>.Success(Unit.Value, "Password changed successfully. Please log in again.");
    }
}
