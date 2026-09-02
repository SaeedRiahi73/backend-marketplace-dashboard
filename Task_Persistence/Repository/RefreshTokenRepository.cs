using Microsoft.EntityFrameworkCore;
using Task_Application.Contracts.Interfaces.RefreshTokens;
using Task_Domain.Entities;
using Task_Persistence.Context;

namespace Task_Persistence.Repository;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TaskDbContext _context;

    public RefreshTokenRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.SingleOrDefaultAsync(
            refreshToken => refreshToken.TokenHash == tokenHash,
            cancellationToken);
    }

    public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(
        Guid userId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(refreshToken =>
                refreshToken.UserId == userId &&
                refreshToken.RevokedAt == null &&
                refreshToken.ExpiresAt > utcNow)
            .ToListAsync(cancellationToken);
    }
}
