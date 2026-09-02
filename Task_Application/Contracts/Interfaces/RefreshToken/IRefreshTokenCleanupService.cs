namespace Task_Application.Contracts.Interfaces.RefreshTokens;

public interface IRefreshTokenCleanupService
{
    Task<int> DeleteExpiredAsync(DateTime utcNow,CancellationToken cancellationToken = default);
}
