using Microsoft.EntityFrameworkCore;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Persistence.Context;

namespace Task_Persistence.Services.RefreshTokens;

public sealed class RefreshTokenCleanupService
    : IRefreshTokenCleanupService
{
    private readonly TaskDbContext _context;

    public RefreshTokenCleanupService(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<int> DeleteExpiredAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(refreshToken => refreshToken.ExpiresAt <= utcNow)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
