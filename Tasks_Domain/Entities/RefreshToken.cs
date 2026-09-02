using Task_Domain.Common;

namespace Task_Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsPersistent { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }

    private RefreshToken() { }

    public RefreshToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        bool isPersistent)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id cannot be empty.");

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new DomainException("Refresh token hash cannot be empty.");

        DateTime createdAt = DateTime.UtcNow;

        if (expiresAt <= createdAt)
            throw new DomainException("Refresh token expiration must be in the future.");

        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        IsPersistent = isPersistent;
    }

    public bool IsActive(DateTime utcNow)
    {
        return RevokedAt is null && ExpiresAt > utcNow;
    }

    public void Revoke(Guid? replacedByTokenId = null)
    {
        if (RevokedAt.HasValue)
            throw new DomainException("Refresh token has already been revoked.");

        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenId = replacedByTokenId;
    }
}
