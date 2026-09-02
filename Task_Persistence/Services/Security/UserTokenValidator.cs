using Microsoft.EntityFrameworkCore;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Models.Security;
using Task_Persistence.Context;

namespace Task_Persistence.Services.Security;

public sealed class UserTokenValidator : IUserTokenValidator
{
    private readonly TaskDbContext _context;
    private readonly IUserTokenValidationCache _cache;

    public UserTokenValidator(
        TaskDbContext context,
        IUserTokenValidationCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<bool> IsValidAsync(
        Guid userId,
        int tokenVersion,
        CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(userId, out UserTokenValidationState? cachedState) &&
            cachedState is not null)
        {
            return IsValid(cachedState, tokenVersion);
        }

        UserTokenValidationState? state = await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new UserTokenValidationState(
                user.IsActive,
                user.TokenVersion))
            .SingleOrDefaultAsync(cancellationToken);

        if (state is null)
            return false;

        _cache.Set(userId, state);

        return IsValid(state, tokenVersion);
    }

    private static bool IsValid(
        UserTokenValidationState state,
        int tokenVersion)
    {
        return state.IsActive &&
               state.TokenVersion == tokenVersion;
    }
}
